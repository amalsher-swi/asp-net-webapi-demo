using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Mapping;
using AuditLog.Services.Models;
using AuditLog.Services.Providers;
using AutoMapper;
using Moq;
using Xunit;

namespace AuditLog.Services.Tests.Providers
{
    public class UsersProviderShould
    {
        private readonly IMapper _mapper;
        
        public UsersProviderShould()
        {
            _mapper = new Mapper(new MapperConfiguration(exp => exp.AddProfile<MappingConfiguration>()));
        }
        
        [Fact]
        public async Task Test1()
        {
            var entities = new List<UserEntity>
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

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(x => x.FilterByAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            var sut = new UsersProvider(_mapper, usersRepositoryMock.Object);

            var result = await sut.GetAllAsync(CancellationToken.None);

            var expected = _mapper.Map<IEnumerable<User>>(entities);

            Assert.Equal(expected, result);
        }
    }
}
