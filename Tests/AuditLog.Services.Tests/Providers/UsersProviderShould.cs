using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Mapping;
using AuditLog.Services.Models;
using AuditLog.Services.Providers;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace AuditLog.Services.Tests.Providers
{
    public class UsersProviderShould
    {
        private const string SearchText = "test";
        private const int UserId = 1;

        private readonly IMapper _mapper;

        public UsersProviderShould()
        {
            _mapper = new Mapper(new MapperConfiguration(exp => exp.AddProfile<MappingConfiguration>()));
        }

        #region GetAllAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForGetAll()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.GetAllAsync(CancellationToken.None);

            usersRepositoryMock.Verify(x => x.FilterByAsync(null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MapAllEntitiesToModels()
        {
            var entities = GetUserEntities();
            var expected = _mapper.Map<IEnumerable<User>>(entities);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.FilterByAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.GetAllAsync(CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region SearchAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForSearch()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.SearchAsync(SearchText, CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.FilterByAsync(It.IsNotNull<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapEntitiesToModelsWhenSearching()
        {
            var entities = GetUserEntities();
            var expected = _mapper.Map<IEnumerable<User>>(entities);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.FilterByAsync(It.IsNotNull<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.SearchAsync(SearchText, CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForSingleUser()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.GetByIdAsync(UserId, CancellationToken.None);

            usersRepositoryMock.Verify(x => x.GetByIdAsync(UserId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MapEntityToModelForSingleUser()
        {
            var entity = GetUserEntities().First();
            var expected = _mapper.Map<User>(entity);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.GetByIdAsync(UserId, CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region InsertAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenInserting()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.InsertAsync(It.IsAny<User>(), CancellationToken.None);

            usersRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapModelToEntityAndBackWhenInserting()
        {
            var model = GetUserModels().First();

            var entity = _mapper.Map<UserEntity>(model);

            var expected = _mapper.Map<User>(entity);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.InsertAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);
            
            var result = await sut.InsertAsync(model, CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region InsertManyAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenInsertingMany()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.InsertAsync(It.IsAny<IReadOnlyCollection<User>>(), CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.InsertAsync(It.IsAny<IReadOnlyCollection<UserEntity>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapModelsToEntitiesWhenInsertingMany()
        {
            var models = GetUserModels();

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.InsertAsync(It.IsAny<IReadOnlyCollection<UserEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.InsertAsync(models, CancellationToken.None);

            result.Should().BeTrue();
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenUpdating()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.UpdateAsync(It.IsAny<User>(), CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapModelToEntityWhenUpdating()
        {
            var model = GetUserModels().First();

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.UpdateAsync(model, CancellationToken.None);

            result.Should().BeTrue();
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenDeleting()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.DeleteAsync(It.IsAny<int>(), CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        #endregion

        #region DeleteManyAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenDeletingMany()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            await sut.DeleteAsync(It.IsAny<IReadOnlyCollection<int>>(), CancellationToken.None);

            usersRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<IReadOnlyCollection<int>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        #endregion

        private static IEnumerable<UserEntity> GetUserEntities()
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

        private static List<User> GetUserModels()
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
    }
}
