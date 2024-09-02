using System;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.Common;
using Cortside.AspNetCore.EntityFramework.Tests.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cortside.AspNetCore.EntityFramework.Tests {
    public class DateTimeHandlingTest {
        private readonly ServiceCollection services;

        public DateTimeHandlingTest() {
            services = new ServiceCollection();

            // add SubjectPrincipal for auditing
            services.AddSubjectPrincipal();
            services.AddTransient<ISubjectFactory<Subject>, DefaultSubjectFactory>();
        }

        [Fact]
        public async Task ShouldHaveLocalDateTimeHandling() {
            // assemble
            services.AddDbContext<LocalContext<Subject>>(options => {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            var provider = services.BuildServiceProvider();

            var context = provider.GetRequiredService<LocalContext<Subject>>();
            var person = new Person() { Name = "Elmer" };

            // act
            context.Add(person);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(InternalDateTimeHandling.Local, context.DateTimeHandling);
            person.CreatedDate.Should().BeIn(DateTimeKind.Local);
            person.CreatedDate.Should().BeOnOrBefore(DateTime.Now);
        }

        [Fact]
        public async Task ShouldHaveUtcDateTimeHandling() {
            // assemble
            services.AddDbContext<UtcContext<Subject>>(options => {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            var provider = services.BuildServiceProvider();

            var context = provider.GetRequiredService<UtcContext<Subject>>();
            var person = new Person() { Name = "Elmer" };

            // act
            context.Add(person);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(InternalDateTimeHandling.Utc, context.DateTimeHandling);
            person.CreatedDate.Should().BeIn(DateTimeKind.Utc);
            person.CreatedDate.Should().BeOnOrBefore(DateTime.UtcNow);
        }
    }
}
