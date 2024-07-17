using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Entities;
using StudentDB.API.Services;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;   
        private readonly IInstructorRepository _instructorRepository;
        public CourseController(ICourseRepository courseRepository, IDepartmentRepository departmentRepository
            , IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(_departmentRepository));
            _instructorRepository = instructorRepository ?? throw new ArgumentNullException(nameof(instructorRepository));
        }

        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse(CourseVM course)
        {
            try
            {
                if (course == null) BadRequest("Course Data is null");
                var department = await _departmentRepository.GetDepartmentById(course.DepartmentId);
                if (department == null)
                {
                    ModelState.AddModelError("DepartmentID", $"Department with id = {course.DepartmentId} not found.");
                    return BadRequest(ModelState);
                }
                var instructor = await _instructorRepository.GetInstructorById(course.InstructorId);
                if (instructor == null)
                {
                    ModelState.AddModelError("InstructorID", $"Instructor with id = {instructor.DepartmentId} not found.");
                    return BadRequest(ModelState);
                }
                var existingCourse = await _courseRepository.GetCourseById(course.CourseId);
                if (existingCourse != null)
                {
                    return Conflict($"A Course with ID {course.CourseId} already exists.");
                }
                var newCourse = new Course
                {
                    CourseNumber = course.CourseNumber,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    CourseUnits = course.CourseUnits,
                    DepartmentId = course.DepartmentId,
                    InstructorId = course.InstructorId,
                };
                var createdCourse = await _courseRepository.AddCourse(newCourse);
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseId }, createdCourse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating course data.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                return Ok(await _courseRepository.GetAllCourses());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            try
            {
                var result = await _courseRepository.GetCourseById(id);
                if (result == null)
                {
                    return NotFound($"Course with ID = {id} was not found.");
                }
                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Course>> UpdateCourse(int id, Course course)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != course.CourseId)
                {
                    return BadRequest("ID Mismatch");
                }
                var updatedCourse = await _courseRepository.UpdateCourse(course);
                if(updatedCourse == null)
                {
                    return NotFound();
                }
                return Ok(updatedCourse);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error updating course data.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            try
            {
                var result = await _courseRepository.GetCourseById(id);
                if(result == null)
                {
                    return NotFound($"Course with ID = {id} was not found.");
                }
                await _courseRepository.DeleteCourseById(id);
                await _courseRepository.ResetAutoIncrementAsync();
                return Ok($"Course with id = {id} was deleted successfully");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting course data.");
            }
        }

    }
}
