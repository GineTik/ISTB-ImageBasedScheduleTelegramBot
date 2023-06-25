using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<string?> GetRoleByTelegramUserIdAsync(long telegramUserId)
        {
            var user = await _userRepository.GetByTelegramUserIdAsync(telegramUserId);

            if (user == null)
            {
                return null;
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId);

            return role?.Name;
        }
    }
}
