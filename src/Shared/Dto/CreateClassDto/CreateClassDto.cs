﻿using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto.CreateClassDto
{
    public class CreateClassDto : IMap
    {
        public string Name { get; set; }
        public string TeacherName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateClassDto, Class>();
        }
    }
}