using System;
using System.Net;
using System.Threading.Tasks;
using WebApi.Integration.Services;
using WebApi.Models;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Integration.Tests
{
    public class LessonValidationTests_Scenario_With_Authorization : IClassFixture<TestFixture>
    {
        private readonly LessonService _lessonService;
        private readonly CourseService _courseService;
        private readonly string _lessonApiToken;
        private readonly string _courseApiCookie;

        public LessonValidationTests_Scenario_With_Authorization(TestFixture testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            RequestContext.SetRequestSetCorId(Guid.NewGuid().ToString());
            _lessonService = serviceProvider.GetService<LessonService>();
            _courseService = serviceProvider.GetService<CourseService>();
            _lessonApiToken = testFixture.Token;
            _courseApiCookie = testFixture.AuthCookie;
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully()
        {
            //Arrange
            var courseId = await _courseService.CreateRandomCourseAsync(_courseApiCookie);
            var lessonModel = new LessonModel
            {
                CourseId = courseId,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var addLessonResponse = await _lessonService.AddLessonInternalAsync(lessonModel, _lessonApiToken);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, addLessonResponse.StatusCode);
            var lessonId = int.Parse(await addLessonResponse.Content.ReadAsStringAsync());
            var lesson = await _lessonService.GetLessonAsync(lessonId);
            Assert.Equal(lessonModel.CourseId, lesson.CourseId);
            Assert.Equal(lessonModel.Subject, lesson.Subject);
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully2()
        {
            //Arrange
            var courseId = await _courseService.CreateRandomCourseAsync(_courseApiCookie);
            var lessonModel = new LessonModel
            {
                CourseId = courseId,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var addLessonResponse = await _lessonService.AddLessonInternalAsync(lessonModel, _lessonApiToken);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, addLessonResponse.StatusCode);
            var lessonId = int.Parse(await addLessonResponse.Content.ReadAsStringAsync());
            var lesson = await _lessonService.GetLessonAsync(lessonId);
            Assert.Equal(lessonModel.CourseId, lesson.CourseId);
            Assert.Equal(lessonModel.Subject, lesson.Subject);
        }
    }
}