using AuditLog.Common.Models;
using AuditLog.Data.MySql.Entities;
using AuditLog.Services.Models;
using AutoMapper;

namespace AuditLog.Services.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap(typeof(PagedDataResult<>), typeof(PagedDataResult<>));
            
            CreateMap<UserEntity, User>().ReverseMap();
            CreateMap<AuditLogEntity, AuditLogModel>().ReverseMap();
        }
    }
}
