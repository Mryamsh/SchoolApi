namespace SchoolApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CourseDatetime { get; set; }
        public int? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
        public ICollection<CourseStudent>? CourseStudents { get; set; }

    }
}
