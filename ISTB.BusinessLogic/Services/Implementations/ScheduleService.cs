using AutoMapper;
using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ScheduleService(IScheduleRepository repository, IMapper mapper, IUserRepository userRepository)
        {
            _scheduleRepository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public Task ChangeNameAsync(ChangeScheduleNameDTO dto)
        {
            return _scheduleRepository.ChangeNameAsync(dto.OldName, dto.NewName, dto.TelegramUserId);
        }

        public async Task<ScheduleDTO> CreateAsync(CreateScheduleDTO dto)
        {
            var user = await _userRepository.GetByTelegramUserIdAsync(dto.TelegramUserId);

            if (user == null)
            {
                user = await _userRepository.AddAsync(new User
                {
                    TelegramUserId = dto.TelegramUserId,
                });
            }

            var schedule = await _scheduleRepository.AddAsync(new Schedule
            {
                Name = dto.Name,
                UserId = user.Id
            });
            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task<ScheduleDTO?> GetByIdAsync(int id)
        {
            return _mapper.Map<ScheduleDTO>(await _scheduleRepository.GetByIdAsync(id));
        }

        public async Task<ScheduleDTO?> GetByNameAsync(GetScheduleByNameDTO dto)
        {
            var schedule = await _scheduleRepository.GetByNameAsync(dto.Name, dto.TelegramUserId);
            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task<ICollection<ScheduleDTO>> GetListByTelegramUserIdAsync(long telegramUserId)
        {
            var schedules = await _scheduleRepository.GetListByTelegramUserIdAsync(telegramUserId);
            return _mapper.Map<ICollection<ScheduleDTO>>(schedules);
        }

        public async Task RemoveByNameAsync(RemoveScheduleDTO dto)
        {
            await _scheduleRepository.RemoveByNameAsync(dto.Name, dto.TelegramUserId);
        }

        public async Task RemoveByIdAsync(RemoveScheduleByIdDTO dto)
        {
            if (await _scheduleRepository.ScheduleByIdBelongsToUserAsync(dto.Id, dto.TelegramUserId) == false)
                throw new InvalidOperationException($"Schedule not belongs to this user (id {dto.TelegramUserId})");
            
            await _scheduleRepository.RemoveById(dto.Id);
        }
    }
}
