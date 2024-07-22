using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class UserRoles
    {
        [Key]
        public int UserRoleID { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserRoleName { get; set; } = string.Empty;
    }
}
