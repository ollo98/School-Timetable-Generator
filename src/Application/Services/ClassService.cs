﻿using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Dto.CreateClassDto;
using Shared.Dto.CreateStudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITimetableRepository _timetableRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IStudentRepository studentRepository
            ,ITimetableRepository timetableRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _timetableRepository = timetableRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateClass(CreateClassDto model)
        {
            var classToAdd = _mapper.Map<Class>(model);
            await _classRepository.AddAsync(classToAdd);
            return classToAdd.Id;
        }

        public async Task<int> CreateStudent(CreateStudentDto model)
        {
            var student = _mapper.Map<Student>(model);
            await _studentRepository.AddAsync(student);
            return student.Id;
        }

        public async Task<IEnumerable<string>> GetAllClassessNames(int timetableId)
        {
            var result = await _classRepository.GetWhereAsync(x => x.TimetableId == timetableId);
            return result.Select(x => x.Name);
        }

    }
}
