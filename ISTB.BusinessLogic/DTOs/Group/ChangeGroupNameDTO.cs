namespace ISTB.BusinessLogic.DTOs.Group
{
    public class ChangeGroupNameDTO
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
        public long TelegramUserId { get; set; }
    }
}
