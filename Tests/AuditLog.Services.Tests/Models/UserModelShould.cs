using AuditLog.Services.Models;
using Xunit;

namespace AuditLog.Services.Tests.Models
{
    public class UserModelShould
    {
        [Fact]
        public void HaveEmptyFirstNameWhenCreated()
        {
            var sut = new User();

            Assert.NotNull(sut.FirstName);
            Assert.Empty(sut.FirstName);
        }

        [Fact]
        public void HaveNullLastNameWhenCreated()
        {
            var sut = new User();

            Assert.Null(sut.LastName);
        }
    }
}
