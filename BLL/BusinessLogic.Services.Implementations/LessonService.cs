using System;
using BusinessLogic.Abstractions;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.Services.HttpClients;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    /// <summary>
    /// Сервис работы с уроками
    /// </summary>
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IService1HttpClient _service1HttpClient;

        public LessonService(
            IMapper mapper,
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository,
            IService1HttpClient service1HttpClient)
        {
            _mapper = mapper;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _service1HttpClient = service1HttpClient;
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="page">номер страницы</param>
        /// <param name="pageSize">объем страницы</param>
        /// <returns></returns>
        public async Task<ICollection<LessonDto>> GetPaged(int page, int pageSize)
        {
            ICollection<Lesson> entities = await _lessonRepository.GetPagedAsync(page, pageSize);
            return _mapper.Map<ICollection<Lesson>, ICollection<LessonDto>>(entities);
        }

        /// <summary>
        /// Получить
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>ДТО урока</returns>
        public async Task<LessonDto> GetById(int id)
        {
            var lesson = await _lessonRepository.GetAsync(id);
            //await _service1HttpClient.SendRequestAsync(); //тестовый вызов микросервиса по http
            return _mapper.Map<LessonDto>(lesson);
        }

        /// <summary>
        /// Создать
        /// </summary>
        /// <param name="lessonDto">ДТО урока</param>
        /// <returns>идентификатор</returns>
        public async Task<int> Create(LessonDto lessonDto)
        {
            var entity = _mapper.Map<LessonDto, Lesson>(lessonDto);
            entity.CourseId = lessonDto.CourseId;
            if (!await _courseRepository.GetAll().AnyAsync(c => c.Id == lessonDto.CourseId))
            {
                throw new Exception($"Курса с идентификатором {lessonDto.CourseId} не существует");
            };
            var res = await _lessonRepository.AddAsync(entity);
            await _lessonRepository.SaveChangesAsync();
            return res.Id;
        }

        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="lessonDto">ДТО урока</param>
        public async Task Update(int id, LessonDto lessonDto)
        {
            var entity = _mapper.Map<LessonDto, Lesson>(lessonDto);
            entity.Id = id;
            _lessonRepository.Update(entity);
            await _lessonRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">идентификатор</param>
        public async Task Delete(int id)
        {
            var lesson = await _lessonRepository.GetAsync(id);
            lesson.Deleted = true; 
            await _lessonRepository.SaveChangesAsync();
        }
    }
}