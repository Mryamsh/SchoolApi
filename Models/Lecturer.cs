namespace SchoolApi.Models
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? LevelOfStudy { get; set; }
        public string? Password { get; set; }
        public ICollection<Course>? Courses { get; set; }

    }
}
