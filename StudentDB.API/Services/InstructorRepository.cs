using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDB.API.DbContexts;
using StudentDB.API.Entities;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Services
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly StudentDbContext _context;

        public InstructorRepository(StudentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Instructor> AddInstructor(Instructor instructor)
        {
            if(instructor.Department != null)
            {
                _context.Entry(instructor.Department).State = EntityState.Unchanged;
            }
            var result = await _context.Instructors.AddAsync(instructor);
            await _context.SaveChangesAsync();
            return result.Entity;

        }

        public async Task DeleteInstructor(int id)
        {
            var result = await _context.Instructors.FirstOrDefaultAsync(i => i.InstructorId == id);
            if (result != null)
            {
                _context.Instructors.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructors()
        {
            return await _context.Instructors
                .Include(i => i.Department)
                .ToListAsync();
        }

        public async Task<Instructor> GetInstructorById(int id)
        {
            return await _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(i => i.InstructorId == id);
        }

        public async Task<Instructor>UpdateInstructor(Instructor instructor)
        {
            var result = await _context.Instructors.FirstOrDefaultAsync(i => i.InstructorId == instructor.InstructorId);
            if (result != null)
            {
                result.FirstName = instructor.FirstName;
                result.LastName = instructor.LastName;
                result.Status = instructor.Status;
                result.HireDate = instructor.HireDate;
                result.AnnualSalary = instructor.AnnualSalary;
                if(result.DepartmentId != 0)
                {
                    result.DepartmentId = instructor.DepartmentId;
                }
                else if (result.DepartmentId != null)
                {
                    result.DepartmentId = instructor.Department.DepartmentId;
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
