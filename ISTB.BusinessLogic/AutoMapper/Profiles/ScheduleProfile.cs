using AutoMapper;
using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.DataAccess.Entities;

namespace ISTB.BusinessLogic.AutoMapper.Profiles
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile() : base()
        {
            CreateMap<Schedule, ScheduleDTO>();
            CreateMap<Schedule, ScheduleWithWeeksDTO>();
            CreateMap<ScheduleWeek, ScheduleWeekDTO>();
            CreateMap<ScheduleWeek, ScheduleWeekWithDaysDTO>();
            CreateMap<ScheduleDay, ScheduleDayDTO>();
        }
    }
}
