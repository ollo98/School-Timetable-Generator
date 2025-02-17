﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Teacher : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Class Class { get; set; }
        public int? HoursAvailability { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public int? TimetableId { get; set; }
        public virtual TimeTable TimeTable { get; set; }
        public virtual ICollection<UnassignedLesson> UnassignedLessons { get; set; }
    }
}