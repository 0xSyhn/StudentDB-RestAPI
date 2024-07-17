using StudentDB.API.Entities;
using StudentDB.API.Models;

namespace StudentDB.API.Services
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDto>> GetAllStudents();
        Task<StudentDto> GetStudentById(int id);
        Task<Course> GetCourseById(int courseid);
        Task AddStudentCourse(StudentCourse studentCourse);
        Task<Student> AddStudent(Student student);
        Task<StudentDto> UpdateStudent(StudentDto student);
        Task DeleteStudentById(int id);

        Task ResetAutoIncrementAsync();

    }
}
