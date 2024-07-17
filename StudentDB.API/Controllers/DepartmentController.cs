using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Models;
using StudentDB.API.Services;

namespace StudentDB.API.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository) 
        { 
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }


        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> AddDepartment(DepartmentDto department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (department == null) return BadRequest("Department data is null");

                var existingDepartment = await _departmentRepository.GetDepartmentById(department.DepartmentId);
                if (existingDepartment != null)
                {
                    return Conflict($"A department with ID {department.DepartmentId} already exists.");
                }

                var createdDepartment = await _departmentRepository.AddDepartment(department);

                return CreatedAtAction(nameof(GetDepartmentById),
                    new { id = createdDepartment.DepartmentId }, createdDepartment);
            }
            catch (Exception ex)
            {
            
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Creating Department."+ ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            try
            {
                return Ok(await _departmentRepository.GetAllDepartments());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
        {
            try
            {
                var result = await _departmentRepository.GetDepartmentById(id);
                if (result == null)
                {
                    return NotFound($"Department with id = {id} was not found");
                }
                return result;

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from database");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, DepartmentDto department)
        {
            try
            {
                if (id != department.DepartmentId) return BadRequest("Department ID Mismatch");
                var departmentToUpdate = await _departmentRepository.GetDepartmentById(id);
                if (departmentToUpdate == null)
                {
                    return NotFound($"Department with id = {id} Not Found.");
                }
                return await _departmentRepository.UpdateDepartment(departmentToUpdate);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Updating Department");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                var result = await _departmentRepository.GetDepartmentById(id);
                if (result == null)
                {
                    return NotFound();
                }
                await _departmentRepository.DeleteDepartment(id);
                return Ok($"Department with id = {id} was deleted successfully");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Deleting Department Data.");
            }
        }
    }
}
