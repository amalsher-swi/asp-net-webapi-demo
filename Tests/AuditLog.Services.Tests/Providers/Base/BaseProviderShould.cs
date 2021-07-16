using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Interfaces;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Interfaces;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Mapping;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace AuditLog.Services.Tests.Providers.Base
{
    public abstract class BaseProviderShould<TEntity, TModel, TIdType, TRepository>
        where TEntity : class, IBaseEntity<TIdType>
        where TModel : class, IBaseModel<TIdType>
        where TRepository : class, IBaseRepository<TEntity, TIdType>
    {
        protected readonly IMapper Mapper;
        
        protected BaseProviderShould()
        {
            Mapper = new Mapper(new MapperConfiguration(exp => exp.AddProfile<MappingConfiguration>()));
        }
        
        #region GetAllAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForGetAll()
        {
            var repositoryMock = new Mock<TRepository>();

            var sut = GetProvider(repositoryMock.Object);

            await sut.GetAllAsync(CancellationToken.None);

            repositoryMock.Verify(x => x.FilterByAsync(null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MapAllEntitiesToModels()
        {
            var entities = GetEntities();
            var expected = Mapper.Map<IEnumerable<TModel>>(entities);

            var repositoryMock = new Mock<TRepository>();
            repositoryMock
                .Setup(x => x.FilterByAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            var sut = GetProvider(repositoryMock.Object);

            var result = await sut.GetAllAsync(CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodForSingleEntity()
        {
            var repositoryMock = new Mock<TRepository>();

            var sut = GetProvider(repositoryMock.Object);

            await sut.GetByIdAsync(GetEntityId(), CancellationToken.None);

            repositoryMock.Verify(x => x.GetByIdAsync(GetEntityId(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MapEntityToModelForSingleEntity()
        {
            var entity = GetEntities().First();
            var expected = Mapper.Map<TModel>(entity);

            var repositoryMock = new Mock<TRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(GetEntityId(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var sut = GetProvider(repositoryMock.Object);

            var result = await sut.GetByIdAsync(GetEntityId(), CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region InsertAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenInserting()
        {
            var repositoryMock = new Mock<TRepository>();

            var sut = GetProvider(repositoryMock.Object);

            await sut.InsertAsync(It.IsAny<TModel>(), CancellationToken.None);

            repositoryMock.Verify(x => x.InsertAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task MapModelToEntityAndBackWhenInserting()
        {
            var model = GetModels().First();

            var entity = Mapper.Map<TEntity>(model);

            var expected = Mapper.Map<TModel>(entity);

            var repositoryMock = new Mock<TRepository>();
            repositoryMock
                .Setup(x => x.InsertAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var sut = GetProvider(repositoryMock.Object);
            
            var result = await sut.InsertAsync(model, CancellationToken.None);

            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region InsertManyAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenInsertingMany()
        {
            var repositoryMock = new Mock<TRepository>();
        
            var sut = GetProvider(repositoryMock.Object);
        
            await sut.InsertAsync(It.IsAny<IReadOnlyCollection<TModel>>(), CancellationToken.None);
        
            repositoryMock.Verify(
                x => x.InsertAsync(It.IsAny<IReadOnlyCollection<TEntity>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public async Task MapModelsToEntitiesWhenInsertingMany()
        {
            var models = GetModels();
        
            var repositoryMock = new Mock<TRepository>();
            repositoryMock
                .Setup(x => x.InsertAsync(It.IsAny<IReadOnlyCollection<TEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
        
            var sut = GetProvider(repositoryMock.Object);
        
            var result = await sut.InsertAsync(models, CancellationToken.None);
        
            result.Should().BeTrue();
        }
        #endregion
        
        #region UpdateAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenUpdating()
        {
            var repositoryMock = new Mock<TRepository>();
        
            var sut = GetProvider(repositoryMock.Object);
        
            await sut.UpdateAsync(It.IsAny<TModel>(), CancellationToken.None);
        
            repositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public async Task MapModelToEntityWhenUpdating()
        {
            var model = GetModels().First();
        
            var repositoryMock = new Mock<TRepository>();
            repositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
        
            var sut = GetProvider(repositoryMock.Object);
        
            var result = await sut.UpdateAsync(model, CancellationToken.None);
        
            result.Should().BeTrue();
        }
        #endregion
        
        #region DeleteAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenDeleting()
        {
            var repositoryMock = new Mock<TRepository>();
        
            var sut = GetProvider(repositoryMock.Object);
        
            await sut.DeleteAsync(It.IsAny<TIdType>(), CancellationToken.None);
        
            repositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<TIdType>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        #endregion
        
        #region DeleteManyAsync
        [Fact]
        public async Task InvokeRelatedRepositoryMethodWhenDeletingMany()
        {
            var repositoryMock = new Mock<TRepository>();
        
            var sut= GetProvider(repositoryMock.Object);
        
            await sut.DeleteAsync(It.IsAny<IReadOnlyCollection<TIdType>>(), CancellationToken.None);
        
            repositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<IReadOnlyCollection<TIdType>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
        #endregion

        protected abstract IBaseProvider<TModel, TIdType> GetProvider(TRepository repository);
        protected abstract IEnumerable<TEntity> GetEntities();
        protected abstract List<TModel> GetModels();
        protected abstract TIdType GetEntityId();
    }
}
