using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDB.API.DbContexts;
using StudentDB.API.Entities;

namespace StudentDB.API.Services
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentDbContext _context;
        public CourseRepository(StudentDbContext context) 
        { 
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Course> AddCourse(Course course)
        {
            if (course.Department != null)
            {
                _context.Entry(course.Department).State = EntityState.Unchanged;
            }

            if (course.Instructor!= null)
            {
                _context.Entry(course.Instructor).State = EntityState.Unchanged;
            }

            var result = await _context.Courses.AddAsync(course);
             await _context.SaveChangesAsync();
             return result.Entity;

        }

        public async Task DeleteCourseById(int id)
        {
            var result = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
            if (result != null)
            {
                _context.Courses.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c =>c.CourseId == id);
        }

        public async Task<Course> UpdateCourse(Course course)
        {
            var result = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId==course.CourseId);
            if (result != null)
            {
                result.CourseNumber = course.CourseNumber;
                result.CourseName = course.CourseName;
                result.CourseDescription = course.CourseDescription;
                result.CourseUnits = course.CourseUnits;

                if(result.DepartmentId != 0)
                {
                    result.DepartmentId = course.DepartmentId;
                }

                else if(result.InstructorId != 0)
                {
                    result.InstructorId = course.InstructorId;
                }
                else if(result.DepartmentId!=null){
                    result.DepartmentId = course.Department.DepartmentId;
                }
                else if (result.InstructorId != null)
                {
                    result.InstructorId = course.Instructor.InstructorId;
                }
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task ResetAutoIncrementAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Students', RESEED, 0)");
        }
    }

}
