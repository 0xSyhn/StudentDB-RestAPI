using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
 
        [Required]
        [MaxLength(250)]
        public string UserEmail { get; set; }

        public ICollection<int> Roles { get; set; }
    }
}
