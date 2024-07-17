using Microsoft.EntityFrameworkCore;
using StudentDB.API.DbContexts;
using StudentDB.API.Models;
using StudentDB.API.Entities;

namespace StudentDB.API.Services
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly StudentDbContext _context;

        public DepartmentRepository(StudentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<DepartmentDto> AddDepartment(DepartmentDto department)
        {

            var dept = new Department
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };

            var result = await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                DepartmentId = result.Entity.DepartmentId,
                DepartmentName = result.Entity.DepartmentName
            };
        }

        public async Task DeleteDepartment(int id)
        {
            var result = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (result != null)
            {
                _context.Departments.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            });
        }

        public async Task<DepartmentDto> GetDepartmentById(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (department == null)
                return null;

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };
        }

        public async Task<DepartmentDto> UpdateDepartment(DepartmentDto department)
        {
            var result = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId);
            if (result != null)
            {
                result.DepartmentName = department.DepartmentName;
                await _context.SaveChangesAsync();
                return new DepartmentDto
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName
                };
            }
            return null;
        }
    }
}
