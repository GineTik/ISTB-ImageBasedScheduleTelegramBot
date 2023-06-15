using AutoMapper;
using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class ScheduleDayService : IScheduleDayService
    {
        private readonly IScheduleDayRepository _dayRepository;
        private readonly IMapper _mapper;

        public ScheduleDayService(IScheduleDayRepository dayRepository, IMapper mapper)
        {
            _dayRepository = dayRepository;
            _mapper = mapper;
        }

        public async Task<ScheduleDayDTO> GetByDayNumber(GetByDayNumberDTO dto)
        {
            if (dto.DayNumber < 0 || dto.DayNumber > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.DayNumber));
            }

            var day = await _dayRepository.GetByDayNumber(dto.DayNumber, dto.WeekId);

            if (day == null)
            {
                day = await _dayRepository.AddAsync(new ScheduleDay()
                {
                    Position = (uint)dto.DayNumber,
                    ScheduleWeekId = dto.WeekId,
                });
            }
            
            return _mapper.Map<ScheduleDayDTO>(day);
        }
    }
}
