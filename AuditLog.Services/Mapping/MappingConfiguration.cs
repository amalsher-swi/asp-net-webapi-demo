using AuditLog.Data.MySql.Entities;
using AuditLog.Services.Models;
using AutoMapper;

namespace AuditLog.Services.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<UserEntity, User>().ReverseMap();
            CreateMap<AuditLogEntity, AuditLogModel>().ReverseMap();
        }
    }
}
