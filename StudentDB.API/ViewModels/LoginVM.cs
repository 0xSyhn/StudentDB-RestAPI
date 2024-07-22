using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.ViewModels
{
    public class LoginVM
    {
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
