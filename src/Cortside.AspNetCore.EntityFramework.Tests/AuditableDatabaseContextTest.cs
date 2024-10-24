using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.EntityFramework.Tests.Contexts;
using Cortside.Common.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cortside.AspNetCore.EntityFramework.Tests {
    public class AuditableDatabaseContextTest {
        private readonly ServiceCollection services;

        public AuditableDatabaseContextTest() {
            services = new ServiceCollection();

            services.AddTransient<ISubjectFactory<Subject>, DefaultSubjectFactory>();
            services.AddDbContext<UtcContext<Subject>>(options => {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
        }

        [Fact]
        public async Task ShouldSetNoUserAuditStamps() {
            // assemble
            services.AddSubjectPrincipal();
            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<UtcContext<Subject>>();

            var person = new Person() { Name = "Elmer" };

            // act
            context.Add(person);
            var now = DateTime.UtcNow;
            await context.SaveChangesAsync();

            // assert
            person.CreatedDate.Should().BeOnOrAfter(now);
            person.CreatedSubject.SubjectId.Should().Be(Guid.Empty);
            person.CreatedSubject.UserPrincipalName.Should().BeNull();
            person.LastModifiedDate.Should().Be(person.CreatedDate);
            person.LastModifiedSubject.SubjectId.Should().Be(person.CreatedSubject.SubjectId);
        }

        [Fact]
        public async Task ShouldSetUserAuditStamps() {
            // assemble
            var claims = new List<Claim>() {
                new Claim("sub", Guid.NewGuid().ToString()),
                new Claim("upn", "elmer")
            };
            var subjectPrincipal = new SubjectPrincipal(claims);

            services.AddScoped<ISubjectPrincipal, SubjectPrincipal>((sp) => {
                return subjectPrincipal;
            });

            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<UtcContext<Subject>>();

            var person = new Person() { Name = "Elmer" };

            // act
            context.Add(person);
            var now = DateTime.UtcNow;
            await context.SaveChangesAsync();

            // assert
            person.CreatedDate.Should().BeOnOrAfter(now);
            person.CreatedSubject.SubjectId.Should().Be(subjectPrincipal.SubjectId);
            person.CreatedSubject.UserPrincipalName.Should().Be(subjectPrincipal.UserPrincipalName);
            person.LastModifiedDate.Should().Be(person.CreatedDate);
            person.LastModifiedSubject.SubjectId.Should().Be(person.CreatedSubject.SubjectId);
        }

        [Fact]
        public async Task ShouldUpdateAuditStamps() {
            // assemble
            services.AddSubjectPrincipal();
            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<UtcContext<Subject>>();

            var person = new Person() { Name = "Elmer" };

            // act
            context.Add(person);
            var now = DateTime.UtcNow;
            await context.SaveChangesAsync();

            person.Name = "Elmer Fudd";
            await context.SaveChangesAsync();

            // assert
            person.CreatedDate.Should().BeOnOrAfter(now);
            person.CreatedSubject.SubjectId.Should().Be(Guid.Empty);
            person.CreatedSubject.UserPrincipalName.Should().BeNull();
            person.LastModifiedDate.Should().BeAfter(person.CreatedDate);
            person.LastModifiedSubject.SubjectId.Should().Be(person.CreatedSubject.SubjectId);
        }
    }
}
