using AutoMapper;
using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository repository, IMapper mapper, IUserRepository userRepository)
        {
            _groupRepository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public Task ChangeGroupNameAsync(ChangeGroupNameDTO dto)
        {
            return _groupRepository.ChangeGroupNameAsync(dto.OldName, dto.NewName, dto.TelegramUserId);
        }

        public async Task<GroupDTO> CreateGroupAsync(CreateGroupDTO dto)
        {
            var user = await _userRepository.GetByTelegramUserIdAsync(dto.TelegramUserId);

            if (user == null)
            {
                user = await _userRepository.AddAsync(new User
                {
                    TelegramUserId = dto.TelegramUserId,
                });
            }

            var group = await _groupRepository.AddAsync(new Group
            {
                Name = dto.Name,
                UserId = user.Id
            });
            return _mapper.Map<GroupDTO>(group);
        }

        public async Task<ICollection<GroupDTO>> GetGroupsByTelegramUserIdAsync(long telegramUserId)
        {
            var groups = await _groupRepository.GetListByTelegramUserIdAsync(telegramUserId);
            return _mapper.Map<ICollection<GroupDTO>>(groups);
        }

        public async Task RemoveGroupAsync(RemoveGroupDTO dto)
        {
            await _groupRepository.RemoveByNameAsync(dto.Name, dto.TelegramUserId);
        }
    }
}
