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
                Position = dto.Position,
                ScheduleId = dto.ScheduleId,
            });

            return _mapper.Map<ScheduleWeekDTO>(week);
        }

        public async Task<ScheduleWeekDTO> GetTodayWeekAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);

            if (schedule == null)
                throw new InvalidOperationException("schedule is null");

            var selectedWeekNumber = schedule.ChosenAsCurrentWeekPosition;
            var dateTimeWhenWeekChosen = schedule.DateTimeWhenWeekChosen;

            var elapsedWeeks = getNumberWeeksElapsedSinceDate(dateTimeWhenWeekChosen);
            var totalWeeksCount = await _weekRepository.GetWeeksCountByScheduleIdAsync(scheduleId);

            var todayWeekNumber = (selectedWeekNumber + elapsedWeeks) % totalWeeksCount;
            var todayWeek = await _weekRepository.GetByPositionAsync(scheduleId, (uint)todayWeekNumber);

            return _mapper.Map<ScheduleWeekDTO>(todayWeek);
        }

        private static int getNumberWeeksElapsedSinceDate(DateTime date)
        {
            var unnecessaryDays = (int)date.DayOfWeek;
            date.AddDays(-unnecessaryDays); // substract days

            var elapsedWeeks = (DateTime.UtcNow - date).TotalDays / 7;
            return (int)Math.Ceiling(elapsedWeeks) - 1;
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

        public async Task ChooseWeekLikeCurrentAsync(ChooseCurrentWeekDTO dto)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);

            if (schedule == null)
                throw new InvalidOperationException("schedule is null");

            schedule.ChosenAsCurrentWeekPosition = dto.WeekPosition;
            schedule.DateTimeWhenWeekChosen = DateTime.UtcNow;
            await _scheduleRepository.UpdateAsync(schedule);
        }
    }
}
