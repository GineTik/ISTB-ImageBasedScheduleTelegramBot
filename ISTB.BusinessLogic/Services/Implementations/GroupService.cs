using AutoMapper;
using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ICollection<GroupDTO> GetGroupsByTelegramUserId(long telegramUserId)
        {
            var groups = _repository.GetListByTelegramUserId(telegramUserId);
            return _mapper.Map<ICollection<GroupDTO>>(groups);
        }
    }
}
