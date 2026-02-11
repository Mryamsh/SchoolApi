using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Lecturer)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    CourseDatetime = c.CourseDatetime,
                    LecturerName = c.Lecturer.Name
                })
                .ToListAsync();

            return Ok(courses);
        }
        [HttpGet("byDate")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByDate()
        {
            var courses = await _context.Courses
                .Include(c => c.Lecturer)
                .Where(c => c.CourseDatetime >= DateTime.Now) // 👈 filter not expired
                .OrderBy(c => c.CourseDatetime) // 👈 closest first
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    CourseDatetime = c.CourseDatetime,
                    LecturerName = c.Lecturer.Name
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.CourseStudents)
                    .ThenInclude(cs => cs.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            return course;
        }


        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.Id)
                return BadRequest();

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpGet("student/{studentId}")]
        public IActionResult GetCoursesByStudent(int studentId)
        {
            var courses = _context.Courses
                .Where(c => c.CourseStudents.Any(cs => cs.StudentId == studentId))
                .OrderBy(c => c.CourseDatetime) // 👈 THIS LINE
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    c.CourseDatetime,
                    LecturerName = c.Lecturer.Name
                })
                .ToList();

            return Ok(courses);
        }


        // [HttpPost("Enroll")]
        // public async Task<IActionResult> EnrollStudent([FromBody] CourseStudent enrollment)
        // {
        //     var studentExists = await _context.Students.AnyAsync(s => s.Id == enrollment.StudentId);
        //     var courseExists = await _context.Courses.AnyAsync(c => c.Id == enrollment.CourseId);

        //     if (!studentExists || !courseExists)
        //         return BadRequest("Invalid student or course ID");

        //     _context.CourseStudent.Add(enrollment);
        //     await _context.SaveChangesAsync();

        //     return Ok(enrollment);
        // }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(c => c.Id == id);
        }
    }
}
