using Microsoft.EntityFrameworkCore;
using StudentDB.API.DbContexts;
using StudentDB.API.Entities;
using StudentDB.API.Models;

namespace StudentDB.API.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;
        public StudentRepository(StudentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Student> AddStudent(Student student)
        {
           
            var result = await _context.AddAsync(student);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteStudentById(int id)
        {
            var result = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            if(result != null)
            {
                _context.Students.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

       public async Task DeleteStudentCourse(int id)
        {
            var result = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == id);
            if (result != null) {
                _context.StudentCourses.Remove(result);
                await _context.SaveChangesAsync();
            }

        }


        public async Task<IEnumerable<StudentDto>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return students.Select(s => MapToStudentDto(s)).ToList();
        }

        public async Task<StudentDto> GetStudentById(int id)
        {
            var student =  await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            if(student == null)
            {
                return null;
            }
            return MapToStudentDto(student);
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<StudentCourse> GetStudentCourseByStudentId(int studentId)
        {
            return await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId);
        }


        public async Task AddStudentCourse(StudentCourse studentCourse)
        {
            await _context.StudentCourses.AddAsync(studentCourse);
            await _context.SaveChangesAsync();
        }



        public async Task<StudentCourse>UpdateStudentCourse(StudentCourse studentCourse)
        {
            var result = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentCourseId == studentCourse.StudentCourseId);
            if(result != null)
            {
                result.StudentId = studentCourse.StudentId;
                result.CourseId = studentCourse.CourseId;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<StudentDto> UpdateStudent(StudentDto student)
        {
            var result = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == student.StudentId);
            if (result != null)
            {
                result.FirstName = student.FirstName;
                result.LastName = student.LastName;
                result.EnrollmentDate = student.EnrollmentDate;
                result.GraduationDate = student.GraduationDate;

                await _context.SaveChangesAsync();
                return MapToStudentDto(result);
            }
            return null;
            
        }

        private StudentDto MapToStudentDto(Student student)
        {
            return new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate,
                GraduationDate = student.GraduationDate,

            };
        }

        public async Task ResetAutoIncrementAsync()
        {
            
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Students', RESEED, 0)");
        }

      
    }
}
