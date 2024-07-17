using StudentDB.API.Models;

namespace StudentDB.API.Services
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartments();
        Task<DepartmentDto> GetDepartmentById(int id);
        Task<DepartmentDto> AddDepartment(DepartmentDto department);
        Task<DepartmentDto> UpdateDepartment(DepartmentDto department);
        Task DeleteDepartment(int id);
    }
}
