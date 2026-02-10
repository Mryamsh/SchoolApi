namespace SchoolApi.Models
{
    public class CourseStudent
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }


    public class EnrollStudentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
