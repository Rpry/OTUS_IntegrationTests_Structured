namespace WebApi.Models
{
    public class LessonModel
    {
        /// <summary>
        /// Идентификатор курса
        /// </summary>
        //[Range(1, Int32.MaxValue)]
        public int CourseId { get; set; }
        
        /// <summary>
        /// Тема
        /// </summary>
        public string Subject { get; set; }
    }
}