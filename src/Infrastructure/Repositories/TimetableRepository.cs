﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TimetableRepository : EfRepository<TimeTable>, ITimetableRepository
    {
        public TimetableRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}