﻿using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.CreateTeacherDto;
using Shared.Dto.UpdateTeacherDto;
using Shared.Responses;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherDto model)
        {
            int result = await _teacherService.CreateTeacher(model);
            return Ok(new OkResult<int>(result, "Pomyślnie dodano nauczyciela"));
        }

        [HttpGet("getallfromtimetable")]
        public async Task<IActionResult> GetAllFromTimetable()
        {
            var result = await _teacherService.GetAllTeachersFromTimetable();
            return Ok(new OkResult<IEnumerable<TeacherVm>>(result, "Pomyślnie zwrócono nauczycieli"));
        }

        [HttpGet("getcount")]
        public async Task<IActionResult> GetTeachersCount()
        {
            int result = await _teacherService.GetTeachersCount();
            return Ok(new OkResult<int>(result, "Pomyślnie zwrócono liczbę nauczycieli"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher([FromQuery] int teacherId)
        {
            await _teacherService.DeleteTeacher(teacherId);
            return Ok(new Shared.Responses.OkResult("Pomyślnie usunięto wybranego nauczyciela"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] UpdateTeacherDto model)
        {
            await _teacherService.UpdateTeacher(model);
            return Ok(new Shared.Responses.OkResult("Pomyślnie zaktualizowano wybranego nauczyciela"));
        }

        [HttpGet("teacherexists")]
        public async Task<IActionResult> TeacherExists([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var result = await _teacherService.TeacherExists(firstName, lastName);
            return Ok(new OkResult<bool>(result, "Pomyślnie sprawdzono czy nauczyciel istnieje"));
        }


    }
}