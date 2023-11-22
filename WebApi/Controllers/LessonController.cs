﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class LessonController: ControllerBase
    {
        private ILessonService _service;
        private readonly ILogger<LessonController> _logger;
        private IMapper _mapper;

        public LessonController(ILessonService service, ILogger<LessonController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(_mapper.Map<LessonModel>(await _service.GetById(id)));
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(LessonModel lessonDto)
        {
            if (lessonDto.CourseId == 0)
            {
                return BadRequest("CourseId должен быть больше нуля");
            }
            if (string.IsNullOrWhiteSpace(lessonDto.Subject))
            {
                return BadRequest("Поле Subject не должно быть пустым");
            }
            return Ok(await _service.Create(_mapper.Map<LessonDto>(lessonDto)));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, LessonModel lessonDto)
        {
            await _service.Update(id, _mapper.Map<LessonDto>(lessonDto));
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
        
        [HttpGet("list/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetList(int page, int itemsPerPage)
        {
            return Ok(_mapper.Map<List<LessonModel>>(await _service.GetPaged(page, itemsPerPage)));
        }
    }
}