﻿using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Dto.CreateAvailabilityDto
{
    public class CreateAvailabilityDtoValidator : AbstractValidator<CreateAvailabilityDto>
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITimetableRepository _timetableRepository;
        private string[] daysOfWeek = { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

        public CreateAvailabilityDtoValidator(ITeacherRepository teacherRepository, ITimetableRepository timetableRepository)
        {
            _teacherRepository = teacherRepository;
            _timetableRepository = timetableRepository;

            RuleFor(x => x.DayOfWeek)
                .NotEmpty().WithMessage("Podany dzień tygodnia jest pusty")
                .MustAsync(DefinedDayOfWeekValid).WithMessage("Podany dzień tygodnia jest nieprawidłowy");

            RuleFor(x => x.StartsAt)
                .InclusiveBetween(8, 16).WithMessage("Podano nieprawidłową czas startu");

            RuleFor(x => x.EndsAt)
                .InclusiveBetween(9, 17).WithMessage("Podano nie prawidłowy czas zakończenia");

            RuleFor(x => x.TeacherId)
                .MustAsync(TeacherExists).WithMessage("Podany nauczyciel nie istnieje");

            RuleFor(x => x.TimetableId)
                .GreaterThan(0).WithMessage("Podano nie poprawny plan lekcji")
                .MustAsync(TimetableExists).WithMessage("Podany plan lekcji nie istnieje");
        }

        public Task<bool> DefinedDayOfWeekValid(string value, CancellationToken cancellationToken)
        {
            return Task.FromResult(daysOfWeek.Contains(value));
        }

        public async Task<bool> TeacherExists(int value, CancellationToken cancellationToken)
        {
            return await _teacherRepository.AnyAsync(x => x.Id == value);
        }

        private async Task<bool> TimetableExists(int value, CancellationToken cancellationToken)
        {
            return await _timetableRepository.AnyAsync(x => x.Id == value);
        }
    }
}