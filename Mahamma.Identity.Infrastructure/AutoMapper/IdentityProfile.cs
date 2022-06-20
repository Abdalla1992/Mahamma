using AutoMapper;
using System;
using Mahamma.ApiClient.Dto.Company;
using Mahamma.Identity.Domain.Language.Dto;
using System.Linq;

namespace Mahamma.Identity.Infrastructure.AutoMapper
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            UserMapping();
            RoleMapping();
            MemberMapping();
            LanguageMapping();
        }

        

        private void UserMapping()
        {
            CreateMap<Domain.User.Entity.User, Domain.User.Dto.UserDto>()
                .ForMember(dto => dto.CompanyId, dto => dto.MapFrom(u => u.CompanyId.HasValue ? u.CompanyId.Value : default));
        }
        private void MemberMapping()
        {
            CreateMap<Domain.User.Entity.User, MemberDto>()
                .ForMember(dto => dto.UserId, act => act.MapFrom(src => src.Id));
        }

        private void RoleMapping()
        {
            CreateMap<Domain.Role.Entity.Page, Domain.Role.Dto.PageDto>();
            CreateMap<Domain.Role.Entity.PagePermission, Domain.Role.Dto.PagePermissionDto>()
                .ForMember(dto => dto.Page, act => act.MapFrom(src => src.Page));
            CreateMap<Domain.Role.Entity.Role, Domain.Role.Dto.RoleDto>()
                .ForMember(dto => dto.PagePermissions, act => act.MapFrom(src => src.RolePagePermissions.Select(rp => rp.PagePermission)));
        }



        private void LanguageMapping()
        {
            CreateMap<Domain.Language.Entity.Language, LanguageDto>();
        }
    }
}
