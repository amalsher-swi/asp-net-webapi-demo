using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Common.Models;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Extensions;
using AuditLog.Services.Helpers;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using AuditLog.Services.Providers;
using AuditLog.Services.Tests.Providers.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace AuditLog.Services.Tests.Providers
{
    public class AuditLogsProviderShould : BaseProviderShould<AuditLogEntity, AuditLogModel, int, IAuditLogsRepository>
    {
        private const int AuditLogId = 1;

        #region FilterByAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForFiltering()
        {
            var auditLogsRepositoryMock = new Mock<IAuditLogsRepository>();

            var sut = new AuditLogsProvider(Mapper, auditLogsRepositoryMock.Object);

            await sut.FilterByAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None);

            auditLogsRepositoryMock.Verify(
                x => x.GetPagedAsync(
                    It.IsAny<Expression<Func<AuditLogEntity, bool>>>(),
                    It.IsAny<Expression<Func<AuditLogEntity, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SendFilterExpressionWhenFilterByIsNotNull()
        {
            const string filterBy = nameof(AuditLogEntity.PartnerName);
            const string filterValue = "test";
            
            var auditLogsRepositoryMock = new Mock<IAuditLogsRepository>();

            var sut = new AuditLogsProvider(Mapper, auditLogsRepositoryMock.Object);

            await sut.FilterByAsync(filterBy, filterValue,
                It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None);

            var propertyInfo = typeof(AuditLogEntity).GetPropertyIgnoringCase(filterBy);
            var filterExp = ExpressionHelpers.FilterToLambda<AuditLogEntity>(propertyInfo!, filterValue);

            auditLogsRepositoryMock.Verify(
                x => x.GetPagedAsync(
                    It.Is<Expression<Func<AuditLogEntity, bool>>>(exp => exp.ToString() == filterExp.ToString()),
                    It.IsAny<Expression<Func<AuditLogEntity, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapEntitiesToModelsWhenSearching()
        {
            var pagedEntities = new PagedDataResult<AuditLogEntity>
            {
                Data = GetEntities(),
                Total = 10
            };

            var expected = Mapper.Map<PagedDataResult<AuditLogModel>>(pagedEntities);

            var auditLogsRepositoryMock = new Mock<IAuditLogsRepository>();
            auditLogsRepositoryMock
                .Setup(x => x.GetPagedAsync(
                    It.IsAny<Expression<Func<AuditLogEntity, bool>>>(),
                    It.IsAny<Expression<Func<AuditLogEntity, object>>>(),
                    It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedEntities);

            var sut = new AuditLogsProvider(Mapper, auditLogsRepositoryMock.Object);

            var result = await sut.FilterByAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion
        
        protected override IBaseProvider<AuditLogModel, int> GetProvider(IAuditLogsRepository repository)
        {
            return new AuditLogsProvider(Mapper, repository);
        }

        protected override IEnumerable<AuditLogEntity> GetEntities()
        {
            return new List<AuditLogEntity>
            {
                new AuditLogEntity
                {
                    Id = 1,
                    Timestamp = DateTime.Now,
                    UserId = 1,
                    PartnerId = 1,
                    PartnerName = "Test1"
                },
                new AuditLogEntity
                {
                    Id = 2,
                    Timestamp = DateTime.Now,
                    UserId = 2,
                    PartnerId = 2,
                    PartnerName = "Test2"
                }
            };
        }

        protected override List<AuditLogModel> GetModels()
        {
            return new List<AuditLogModel>
            {
                new AuditLogModel
                {
                    Id = 1,
                    Timestamp = DateTime.Now,
                    UserId = 1,
                    PartnerId = 1,
                    PartnerName = "Test1"
                },
                new AuditLogModel
                {
                    Id = 2,
                    Timestamp = DateTime.Now,
                    UserId = 2,
                    PartnerId = 2,
                    PartnerName = "Test2"
                }
            };
        }

        protected override int GetEntityId()
        {
            return AuditLogId;
        }
    }
}
