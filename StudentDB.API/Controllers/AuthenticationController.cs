using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentDB.API.DbContexts;
using StudentDB.API.Entities;
using StudentDB.API.Utils;
using StudentDB.API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentDB.API.Controllers
{
    [Route("authentication")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly StudentDbContext _context;
        public AuthenticationController(StudentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        [AllowAnonymous]
        [Route("Login"), HttpPost]
        public async Task<IActionResult> Login(LoginVM request)
        {
            try
            {
                var user = await _context.Users
                 .Where(u => u.UserName == request.Username)
                 .FirstOrDefaultAsync();

                if (EncryptDecrypt.Decrypt(user.Password) != request.Password) return BadRequest("dfnhoihnoti");

                var claims = new List<Claim>
                { 
                new Claim("username", request.Username)
                };

                var userRoles = _context.UserUsersRoles
                    .Where(uur => uur.UserID == user.UserID)
                    .Join(_context.UserRoles,
                    uur => uur.UserRoleID,
                    ur => ur.UserRoleID,
                    (uur, ur) => ur.UserRoleName)
                    .ToList();

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("adijfiowvoismic1128449us8b8fufb9d"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new JwtSecurityToken(
                            issuer: "https://localhost:3000",
                            audience: "https://localhost:3000",
                            expires: DateTime.UtcNow.AddDays(1),
                            claims: claims,
                            signingCredentials: creds
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
                return Ok(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
