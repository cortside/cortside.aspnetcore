using System;
using Cortside.AspNetCore.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.EntityFramework {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Registers sql server database context with connection string from Database:ConnectionString in configuration
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="action"></param>
        /// <param name="sqlAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext<TInterface, TImplementation>(
            this IServiceCollection services, IConfiguration configuration,
            Action<DbContextOptionsBuilder> action = null, Action<SqlServerDbContextOptionsBuilder> sqlAction = null)
            where TImplementation : DbContext, TInterface, IUnitOfWork
            where TInterface : class {
            var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");
            services.AddDatabaseContext<TInterface, TImplementation>(connectionString, action, sqlAction);

            return services;
        }

        /// <summary>
        /// Registers sql server database context with connection string passed in
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext<TInterface, TImplementation>(
            this IServiceCollection services, string connectionString, Action<DbContextOptionsBuilder> action = null,
            Action<SqlServerDbContextOptionsBuilder> sqlAction = null)
            where TImplementation : DbContext, TInterface, IUnitOfWork
            where TInterface : class {
            services.AddDbContext<TImplementation>(opt => {
                opt.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions => {
                        // can not use EnableRetryOnFailure because of the use of user initiated transactions

                        // instruct ef to use multiple queries instead of large joined queries
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        sqlAction?.Invoke(sqlOptions);
                    });
                opt.EnableServiceProviderCaching();
                opt.AddInterceptors(new QueryHintCommandInterceptor());

                action?.Invoke(opt);
            });

            // register the dbcontext interface that is to be used
            services.AddScoped<TInterface>(provider => provider.GetService<TImplementation>());

            // Register the service and implementation for the database context
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<TImplementation>());

            // for DbContextCheck in cortside.health
            services.AddTransient<DbContext, TImplementation>();

            return services;
        }
    }
}
