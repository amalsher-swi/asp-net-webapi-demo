using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using AuditLog.Services.Providers;
using AuditLog.Services.Tests.Providers.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace AuditLog.Services.Tests.Providers
{
    public class UsersProviderShould : BaseProviderShould<UserEntity, User, int, IUsersRepository>
    {
        private const string SearchText = "test";
        private const int UserId = 1;

        #region SearchAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForSearch()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(Mapper, usersRepositoryMock.Object);

            await sut.SearchAsync(SearchText, CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.FilterByAsync(It.IsNotNull<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapEntitiesToModelsWhenSearching()
        {
            var entities = GetEntities();
            var expected = Mapper.Map<IEnumerable<User>>(entities);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.FilterByAsync(It.IsNotNull<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            var sut = new UsersProvider(Mapper, usersRepositoryMock.Object);

            var result = await sut.SearchAsync(SearchText, CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        protected override IBaseProvider<User, int> GetProvider(IUsersRepository repository)
        {
            return new UsersProvider(Mapper, repository);
        }

        protected override IEnumerable<UserEntity> GetEntities()
        {
            return new List<UserEntity>
            {
                new UserEntity
                {
                    Id = 1,
                    FirstName = "Test1",
                    LastName = "Test2"
                },
                new UserEntity
                {
                    Id = 2,
                    FirstName = "Test3",
                    LastName = "Test4"
                }
            };
        }

        protected override List<User> GetModels()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Test1",
                    LastName = "Test2"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Test3",
                    LastName = "Test4"
                }
            };
        }

        protected override int GetEntityId()
        {
            return UserId;
        }
    }
}
