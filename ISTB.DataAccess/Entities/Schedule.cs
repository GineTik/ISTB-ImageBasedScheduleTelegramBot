﻿using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class Schedule : Entity
    {
        [Required]
        public uint Position { get; set; }

        public ICollection<ScheduleDay> Days { get; set; }
    }
}
