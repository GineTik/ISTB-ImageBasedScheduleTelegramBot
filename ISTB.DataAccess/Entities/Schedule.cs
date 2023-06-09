﻿using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class Schedule : Entity
    {
        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int ChosenAsCurrentWeekPosition { get; set; }
        [Required]
        public DateTime DateTimeWhenWeekChosen { get; set; }

        public ICollection<ScheduleWeek> Weeks { get; set; }
    }
}
