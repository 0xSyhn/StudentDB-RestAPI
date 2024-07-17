using Microsoft.AspNetCore.Mvc;
using StudentDB.API.Entities;

namespace StudentDB.API.Services
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task<Instructor> GetInstructorById(int id);
        Task<Instructor>AddInstructor(Instructor instructor);
        Task<Instructor> UpdateInstructor(Instructor instructor);
        Task DeleteInstructor(int id);

        Task ResetAutoIncrementAsync();

    }
}
