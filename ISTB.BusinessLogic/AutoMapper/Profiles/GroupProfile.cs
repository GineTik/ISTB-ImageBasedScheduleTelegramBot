using AutoMapper;
using ISTB.BusinessLogic.DTOs.Group;
using ISTB.DataAccess.Entities;

namespace ISTB.BusinessLogic.AutoMapper.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile() : base()
        {
            CreateMap<Group, GroupDTO>();
        }
    }
}
