﻿using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _timetableRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILessonRepository _lessonRepository;

        public TimetableService(ITimetableRepository timetableRepository, IMapper mapper
            , IUserContextService userContextService, IUserRepository userRepository, ISubjectRepository subjectRepository
            , ILessonRepository lessonRepository)
        {
            _timetableRepository = timetableRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
            _lessonRepository = lessonRepository;
        }

        public async Task<int> CreateTimetable()
        {
            int loggedUserId = _userContextService.GetUserId ?? 0;
            var loggedUser = await _userRepository.GetByIdAsync(loggedUserId);
            
            var timetable = new TimeTable();
            timetable.CurrentPhase = 1;
            timetable.CreatorId = loggedUserId;
            await _timetableRepository.AddAsync(timetable);
            loggedUser.CurrentTimetableId = timetable.Id;
            await _userRepository.UpdateAsync(loggedUser);

            return timetable.Id;
        }

        public async Task<int> GetCurrentUserTimetableId()
        {
            int loggedUserId = _userContextService.GetUserId ?? 0;
            var loggedUser = await _userRepository.GetByIdAsync(loggedUserId);
            if(loggedUser.CurrentTimetableId is null) 
            { throw new BadRequestException("Użytkownik nie ma żadnego przypisanego planu lekcji"); }
            return (int)loggedUser.CurrentTimetableId;
        }

        public async Task ChangePhaseNumber(int timetableId, int phaseNumber)
        {
            var timetable = await _timetableRepository.GetByIdAsync(timetableId);
            if (timetable == null) { throw new NotFoundException($"Timetable with id: {timetableId} hasn't been found"); }
            int currentPhaseNumber = timetable.Id;
            timetable.CurrentPhase = phaseNumber;
            await _timetableRepository.UpdateAsync(timetable);
        }

        public async Task<int> GetCurrentPhase(int timetableId)
        {
            var timetable = await _timetableRepository.GetByIdAsync(timetableId);
            if (timetable == null) { throw new NotFoundException($"Timetable with id: {timetableId} hasn't been found"); }
            return timetable.CurrentPhase;
        }

        public async Task<IEnumerable<TimetableOutcomeVm>> GetTimetableGeneretingOutcome(int timetableId)
        {
            var subjects = await _subjectRepository.GetAllSubjectsWithLessonsJoins(timetableId);
            var subjectsByClass = subjects.GroupBy(s => s.Class?.Name).ToList();
            List<TimetableOutcomeVm> outcomeList = new List<TimetableOutcomeVm>();

            foreach (var subject in subjectsByClass)
            {
                var aggregatedLessons = subject.Aggregate(new List<LessonVm>(), (a, b) =>
                {
                    a.AddRange(_mapper.Map<List<LessonVm>>(b.Lessons));
                    return a;
                }).GroupBy(x => x.DayOfWeek, (key, value) =>
                new DayOfWeekOutcomeVm{ DayOfWeek = MatchDayOfWeekByNumber(key), DayOfWeekNumber=key
                , Lessons = value.OrderBy(x=>x.StartsAt).ToList()})
                .OrderBy(x => x.DayOfWeekNumber).ToList();

                outcomeList.Add(new TimetableOutcomeVm
                {
                    ClassName = subject.Key,
                    DayOfWeeks = aggregatedLessons
                });
            }
            return outcomeList;
        }

        public async Task ChangeUserCurrentTimetable(int timetableId)
        {
            int loggedUserId = _userContextService.GetUserId ?? 0;
            var timetable = _timetableRepository.SingleOrDefaultAsync(x=>x.CreatorId == loggedUserId && x.Id == timetableId);
            if(timetable == null) { throw new ForbidException("Nie masz uprawnień do tego planu"); }

            var loggedUser = await _userRepository.GetByIdAsync(loggedUserId);
            loggedUser.CurrentTimetableId = timetableId;
            await _userRepository.UpdateAsync(loggedUser);          
        }

        public async Task<IEnumerable<TimetableVm>> GetUserTimetables()
        {
            int loggedUserId = _userContextService.GetUserId ?? 0;
            var result = await _timetableRepository.GetWhereAsync(x => x.CreatorId == loggedUserId);
            var mappedResult = _mapper.Map<IEnumerable<TimetableVm>>(result);
            return mappedResult;
        }

        private string MatchDayOfWeekByNumber(int numberOfWeek) => numberOfWeek switch
        {
            1 => "Poniedziałek",
            2 => "Wtorek",
            3 => "Środa",
            4 => "Czwartek",
            5 => "Piątek",
            _ => "Błąd"
        };


    }
}