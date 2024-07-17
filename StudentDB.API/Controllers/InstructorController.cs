using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Entities;
using StudentDB.API.Services;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Controllers
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public InstructorController(IInstructorRepository instructorRepository, IDepartmentRepository departmentRepository)
        {
            _instructorRepository = instructorRepository ?? throw new ArgumentNullException(nameof(instructorRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        [HttpPost]
        public async Task<ActionResult<Instructor>> AddInstructor(InstructorVM instructor)
        {
            try
            {
                if (instructor == null)
                {
                    return BadRequest("Instructor Data is null");
                }
                var department = await _departmentRepository.GetDepartmentById(instructor.DepartmentId);
                if (department == null)
                {
                    ModelState.AddModelError("DepartmentID", $"Department with id = {instructor.DepartmentId} not found.");
                    return BadRequest(ModelState);
                }
                var existingInstructor = await _instructorRepository.GetInstructorById(instructor.InstructorId);
                if (existingInstructor != null)
                {
                    return Conflict($"A department with ID {department.DepartmentId} already exists.");
                }
                var newInstructor = new Instructor
                {
                    InstructorId = instructor.InstructorId,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    Status = instructor.Status,
                    HireDate = instructor.HireDate,
                    AnnualSalary = instructor.AnnualSalary,
                    DepartmentId = instructor.DepartmentId,

                };
                var createdInstructor = await _instructorRepository.AddInstructor(newInstructor);
                return CreatedAtAction(nameof(GetInstructorById), new { id = createdInstructor.InstructorId }, createdInstructor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating instructor data.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructorById(int id)
        {
            try
            {
                var result = await _instructorRepository.GetInstructorById(id);
                if (result == null)
                {
                    return NotFound($"Instructor with id = {id} not found.");
                }
                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from database.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<Instructor>> GetInstructors()
        {
            try
            {
                return Ok(await _instructorRepository.GetAllInstructors());
            }
            catch (Exception)
            {


                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from database.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Instructor>> UpdateInstructor(int id, Instructor instructor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != instructor.InstructorId)
                {
                    return BadRequest("ID mismatch");
                }

                var updatedInstructor = await _instructorRepository.UpdateInstructor(instructor);
                if (updatedInstructor == null)
                {
                    return NotFound();
                }

                return Ok(updatedInstructor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error updating instructor data.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Instructor>> DeleteInstructor(int id)
        {
            try
            {
                var result = await _instructorRepository.GetInstructorById(id);
                if(result == null)
                {
                    return NotFound($"Instructor with id = {id} not found.");
                }
                await _instructorRepository.DeleteInstructor(id);
                await _instructorRepository.ResetAutoIncrementAsync();
                return Ok($"Instructor with id = {id} was deleted successfully");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error deleting instructor data.");
            }
        }

    }
}
