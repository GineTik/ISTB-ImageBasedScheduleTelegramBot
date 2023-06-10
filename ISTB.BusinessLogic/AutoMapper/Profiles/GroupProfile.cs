using AutoMapper;
using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.DataAccess.Entities;

namespace ISTB.BusinessLogic.AutoMapper.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile() : base()
        {
            CreateMap<Schedule, ScheduleDTO>();
        }
    }
}
