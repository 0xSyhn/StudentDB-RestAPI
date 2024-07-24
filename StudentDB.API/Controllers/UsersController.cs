using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDB.API.DbContexts;
using StudentDB.API.Entities;
using StudentDB.API.Utils;
using StudentDB.API.ViewModels;

namespace StudentDB.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly StudentDbContext _studentDbContext;
        public UsersController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext?? throw new ArgumentNullException(nameof(studentDbContext));
        }
        [HttpPost]
        public async Task<ActionResult<Users>> AddUser(CreateUserVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(user == null) return BadRequest("User Data is null");
                var newUser = new Users { 
                    UserName = user.UserName,
                    Password = EncryptDecrypt.Encrypt(user.Password),
                    UserEmail = user.UserEmail,
                    isActive=true,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                var result = await _studentDbContext.Users.AddAsync(newUser);
                await _studentDbContext.SaveChangesAsync();

                foreach(var roleId in user.Roles)
                {
                    var role = await _studentDbContext.UserRoles.FindAsync(roleId);
                    if(role == null)
                    {
                        return NotFound("Course not found");
                    }
                    var newUur = new UserUserRoles
                    {
                        UserID = newUser.UserID,
                        UserRoleID = role.UserRoleID,

                    };
                    _studentDbContext.UserUsersRoles.Add(newUur);
                    await _studentDbContext.SaveChangesAsync();
                }

                return Ok(result.Entity);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error seeding User data");
            }
        }
    }
}
