namespace ISTB.BusinessLogic.DTOs.User
{
    public class UserDTO
    {
        public long TelegramUserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
