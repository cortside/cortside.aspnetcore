using Cortside.AspNetCore.EntityFramework.Interceptors;
using Cortside.AspNetCore.EntityFramework.Tests.Contexts;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cortside.AspNetCore.EntityFramework.Tests {
    public class QueryHintCommandInterceptorTest {
        [Fact]
        public void ManipulateCommand() {
            // arrange
            string sql;
            using (var context = new DatabaseContext()) {
                sql = context.People.TagWith("Use option: RECOMPILE").TagWith("foo").ToQueryString();
            }

            var command = new TestDbCommand(sql);

            // act
            QueryHintCommandInterceptor.ManipulateCommand(command);

            // assert
            Assert.EndsWith("OPTION (RECOMPILE)", command.CommandText);
        }
    }
}
