using AutoMapper;
using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using System.Linq;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class ScheduleWeekService : IScheduleWeekService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleWeekRepository _weekRepository;
        private readonly IScheduleDayRepository _dayRepository;
        private readonly IMapper _mapper;

        public ScheduleWeekService(IMapper mapper, IScheduleWeekRepository weekRepository, IScheduleRepository scheduleRepository, IScheduleDayRepository dayRepository)
        {
            _mapper = mapper;
            _weekRepository = weekRepository;
            _scheduleRepository = scheduleRepository;
            _dayRepository = dayRepository;
        }

        public async Task<ScheduleWeekDTO> CreateWeekAsync(CreateScheduleWeekDTO dto)
        {
            if (await _scheduleRepository.BelongsToUserAsync(dto.ScheduleId, dto.TelegramUserId) == false)
                throw new InvalidOperationException($"Schedule not belongs to this user (id {dto.TelegramUserId})");

            var week = await _weekRepository.AddAsync(new ScheduleWeek
            {
                Position = null,
                ScheduleId = dto.ScheduleId,
            });

            return _mapper.Map<ScheduleWeekDTO>(week);
        }

        public async Task<ScheduleWeekWithDaysDTO?> GetWeekByIdAsync(int weekId)
        {
            var week = await _weekRepository.GetByIdAsync(weekId);

            if (week == null)
                return null;

            var defaultDaysDTOs = Enumerable.Range(0, 7).Select(i => new ScheduleDayDTO { Position = i });
            var existsDays = await _dayRepository.GetByWeekIdAsync(weekId);
            var existsDaysDTOs = _mapper.Map<IEnumerable<ScheduleDayDTO>>(existsDays);

            var weekDTO = _mapper.Map<ScheduleWeekWithDaysDTO>(week);
            weekDTO.Days = defaultDaysDTOs.UnionBy(existsDaysDTOs, day => day.Position);

            return weekDTO;
        }

        public async Task<IEnumerable<ScheduleWeekDTO>> GetWeeks(int scheduleId)
        {
            var weeks = await _weekRepository.GetByScheduleIdAsync(scheduleId);
            return _mapper.Map<IEnumerable<ScheduleWeekDTO>>(weeks);
        }
    }
}
