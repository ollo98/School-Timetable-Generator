﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TimeTable : AuditableEntity
    {
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public int CurrentPhase { get; set; }
        public DateTime GenereteTime { get; set; }
        public virtual ICollection<Class> Classess { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<User> ActiveUserTimetables { get; set; }
    }
}