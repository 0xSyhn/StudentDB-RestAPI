using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Entities;

namespace StudentDB.API.Services
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> GetCourseById(int id);
        Task<Course> AddCourse(Course course);
        Task<Course> UpdateCourse(Course course);
        Task DeleteCourseById(int id);
        Task ResetAutoIncrementAsync();
    }
}
