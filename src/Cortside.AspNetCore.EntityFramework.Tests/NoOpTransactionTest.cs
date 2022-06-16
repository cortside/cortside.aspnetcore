using System.Threading.Tasks;
using Xunit;

namespace Cortside.AspNetCore.EntityFramework.Tests {
    public class NoOpTransactionTest {
        [Fact]
        public void ShouldDispose() {
            var tx = new NoOpTransaction();
            tx.Dispose();
            Assert.True(true);
        }

        [Fact]
        public void ShouldAllowCommit() {
            var tx = new NoOpTransaction();
            tx.Commit();
            Assert.True(true);
        }

        [Fact]
        public async Task ShouldAllowCommitAsync() {
            var tx = new NoOpTransaction();
            await tx.CommitAsync();
            Assert.True(true);
        }
    }
}
