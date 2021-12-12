﻿using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Dto.CreateClassroomDto
{
    public class CreateClassroomDtoValidator : AbstractValidator<CreateClassroomDto>
    {
        private readonly IClassroomRepository _classroomRepository;

        public CreateClassroomDtoValidator(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Kod klasy nie może być pusty")
                .MaximumLength(5).WithMessage("Maksymalna długość kodu to 5 znaków")
                .MustAsync(IsCodeUnique).WithMessage(x => $"Już istnieje klasa z kodem: {x.Code}");

            RuleFor(x => x.NumberOfSeats)
                .NotEmpty().WithMessage("Ilość miejsc nie może być pusta")
                .GreaterThan(0).WithMessage("Minimalna ilość miejsc w sali to 1");
            
        }

        public async Task<bool> IsCodeUnique(string value, CancellationToken cancellationToken)
        {
            return ! await _classroomRepository.AnyAsync(x => x.Code == value);
        }
    }
}
