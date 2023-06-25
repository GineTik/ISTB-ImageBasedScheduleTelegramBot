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
        private readonly IScheduleWeekRepository _weekRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleWeekService _weekService;
        private readonly IMapper _mapper;

        public ScheduleDayService(IScheduleDayRepository dayRepository, IMapper mapper, IScheduleWeekRepository weekRepository, IScheduleRepository scheduleRepository, IScheduleWeekService weekService)
        {
            _dayRepository = dayRepository;
            _mapper = mapper;
            _weekRepository = weekRepository;
            _scheduleRepository = scheduleRepository;
            _weekService = weekService;
        }

        public async Task EditDescriptionAsync(EditDescriptionDTO dto)
        {
            var day = await _dayRepository.GetByIdAsync(dto.DayId);

            if (day == null)
                throw new InvalidOperationException("День не знайдений");

            day.Description = dto.Description;
            await _dayRepository.UpdateAsync(day);
        }

        public async Task EditPhotoAsync(EditPhotoIdDTO dto)
        {
            var day = await _dayRepository.GetByIdAsync(dto.DayId);

            if (day == null)
                throw new InvalidOperationException("День не знайдений");

            day.ImageFileUrl = dto.PhotoId;
            await _dayRepository.UpdateAsync(day);
        }

        public async Task<ScheduleDayDTO> GetByDayNumberAsync(GetByDayNumberDTO dto)
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
                    Position = dto.DayNumber,
                    ScheduleWeekId = dto.WeekId,
                });
            }
            
            return _mapper.Map<ScheduleDayDTO>(day);
        }

        public async Task<ScheduleDayDTO> GetByIdAsync(int id)
        {
            var day = await _dayRepository.GetByIdAsync(id);
            return _mapper.Map<ScheduleDayDTO>(day);
        }

        public async Task<ScheduleDayDTO> GetTodayByScheduleIdAsync(int scheduleId)
        {
            var todayWeek = await _weekService.GetTodayWeekAsync(scheduleId);

            int dayNumber = getTodayDayNumber();

            var day = await GetByDayNumberAsync(new GetByDayNumberDTO
            {
                DayNumber = dayNumber,
                WeekId = todayWeek.Id,
            });
            return _mapper.Map<ScheduleDayDTO>(day);
        } 

        private static int getTodayDayNumber()
        {
            var dayNumber = (int)DateTime.Today.DayOfWeek;
            return dayNumber == 0 ? 6 : dayNumber - 1; // 6 is Sunday
        }
    }
}
