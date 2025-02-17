﻿using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.CreateClassDto;
using Shared.Dto.CreateStudentDto;
using Shared.Dto.UpdateClassDto;
using Shared.Responses;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassDto model)
        {
            int result = await _classService.CreateClass(model);
            return Ok(new OkResult<int>(result, "Pomyślnie utworzono nową klasę"));
        }

        [HttpGet("getallclassess")]
        public async Task<IActionResult> GetAllClassess()
        {
            var result = await _classService.GetAllClassess();
            return Ok(new OkResult<IEnumerable<ClassVm>>(result, "Pomyślnie zwrócono klasy"));
        }

        [HttpGet("getallnames")]
        public async Task<IActionResult> GetAllClassessNames()
        {
            var result = await _classService.GetAllClassessNames();
            return Ok(new OkResult<IEnumerable<string>>(result, "Pomyślnie zwrócono nazwy klas"));
        }

        [HttpGet("getbyname")]
        public async Task<IActionResult> GetClassByName([FromQuery] string name)
        {
            var result = await _classService.GetClassByName(name);
            return Ok(new OkResult<ClassVm>(result, "Pomyślnie zwrócono klasę"));
        }

        [HttpGet("getstudentsbyclassname")]
        public async Task<IActionResult> GetStudentsFromClass([FromQuery] string name)
        {
            var result = await _classService.GetStudentsFromClass(name);
            return Ok(new OkResult<IEnumerable<StudentVm>>(result, "Pomyślnie zwrócono uczniów"));
        }

        [HttpGet("getcount")]
        public async Task<IActionResult> GetClassessCount()
        {
            int result = await _classService.GetClassessCount();
            return Ok(new OkResult<int>(result, "Pomyslnie zwrócono liczbę klasy"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClass([FromQuery] int classId)
        {
            await _classService.DeleteClass(classId);
            return Ok(new Shared.Responses.OkResult("Pomyślnie usunięto wybraną klasę"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDto model)
        {
            await _classService.UpdateClass(model);
            return Ok(new Shared.Responses.OkResult("Pomyślnie zaktualizowano wybraną klasę"));
        }
    }
}