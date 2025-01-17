﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Entities;
using StudentDB.API.Models;
using StudentDB.API.Services;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> AddStudent(CreateStudentVM student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (student == null) return BadRequest("Student data is null");
                var newStudent = new Student
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    EnrollmentDate = student.EnrollmentDate,
                    GraduationDate = student.GraduationDate,
                };

                var course = await _studentRepository.GetCourseById(student.CourseId);
                if (course == null)
                {
                    return BadRequest("Course not Found");
                }   

                var createdStudent = await _studentRepository.AddStudent(newStudent);

                var newStudentCourse = new StudentCourse
                {
                    CourseId = course.CourseId,
                    StudentId = createdStudent.StudentId,
                };

                await _studentRepository.AddStudentCourse(newStudentCourse);

                return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.StudentId }, createdStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error Creating Student.");
            }
          
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            try
            {
                return Ok(await _studentRepository.GetAllStudents());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            try
            {
                var result = await _studentRepository.GetStudentById(id);
                if(result == null)
                {
                    return NotFound($"Student with id = {id} was not found");
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
        public async Task<ActionResult<StudentDto>> UpdateStudent(int id, UpdateStudentVM updateStudent)
        {
            try
            {
                if (id != updateStudent.StudentId) return BadRequest("Student ID Mismatch");
                var studentToUpdate = await _studentRepository.GetStudentById(id);
                if (studentToUpdate == null)
                {
                    return NotFound($"Student with id = {id} Not Found.");
                }

                var update = await _studentRepository.UpdateStudent(studentToUpdate);

                var existingStudentCourse = await _studentRepository.GetStudentCourseByStudentId(id);
                if(existingStudentCourse !=  null && existingStudentCourse.CourseId != updateStudent.CourseId)
                {
                    var newCourse = await _studentRepository.GetCourseById(updateStudent.CourseId);
                    if(newCourse == null)
                    {
                        return BadRequest("Course Not Found");
                    }
                    existingStudentCourse.CourseId  = updateStudent.CourseId;
                    await _studentRepository.UpdateStudentCourse(existingStudentCourse);
                }
                else if(existingStudentCourse == null)
                {
                    var newStudentCourse = new StudentCourse
                    {
                        StudentId = id,
                        CourseId = updateStudent.CourseId,

                    };
                    await _studentRepository.AddStudentCourse(newStudentCourse);
                }

                return Ok(update);
                
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Updating Student");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var result = await _studentRepository.GetStudentById(id);
                if( result == null)
                {
                    return NotFound();
                }
                var studentCourse = _studentRepository.GetStudentCourseByStudentId(id);
                if(studentCourse == null)
                {
                    return NotFound();
                }
                await _studentRepository.DeleteStudentCourse(id);
                await _studentRepository.DeleteStudentById(id);
                await _studentRepository.ResetAutoIncrementAsync();
                return Ok($"Student with id = {id} was deleted successfully");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Deleting Student Data.");
            }
        }



    }
}
