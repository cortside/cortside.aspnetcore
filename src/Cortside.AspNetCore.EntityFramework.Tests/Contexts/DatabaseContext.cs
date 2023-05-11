using Cortside.AspNetCore.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.EntityFramework.Tests.Contexts {
    public class DatabaseContext : DbContext {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFQuerying.Tags;Trusted_Connection=True")
                .AddInterceptors(new QueryHintCommandInterceptor());
        }
    }
}
