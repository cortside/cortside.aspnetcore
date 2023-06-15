using Cortside.AspNetCore.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
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
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext<TInterface, TImplementation>(this IServiceCollection services, IConfiguration configuration)
                where TImplementation : DbContext, TInterface, IUnitOfWork
                where TInterface : class {

            var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");
            services.AddDatabaseContext<TInterface, TImplementation>(connectionString);

            return services;
        }

        /// <summary>
        /// Registers sql server database context with connection string passed in
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext<TInterface, TImplementation>(this IServiceCollection services, string connectionString)
                where TImplementation : DbContext, TInterface, IUnitOfWork
                where TInterface : class {
            services.AddDbContext<TImplementation>(opt => {
                opt.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions => {
                        // can not use EnableRetryOnFailure because of the use of user initiated transactions

                        // instruct ef to use multiple queries instead of large joined queries
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                opt.EnableServiceProviderCaching();
                opt.AddInterceptors(new QueryHintCommandInterceptor());
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
