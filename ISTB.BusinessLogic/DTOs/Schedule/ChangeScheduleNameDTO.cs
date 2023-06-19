namespace ISTB.BusinessLogic.DTOs.Schedule
{
    public class ChangeScheduleNameDTO
    {
        //public string OldName { get; set; }
        public int ScheduleId { get; set; }
        public string NewName { get; set; }
        public long TelegramUserId { get; set; }
    }
}
