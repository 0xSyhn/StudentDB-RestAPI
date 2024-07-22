using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDB.API.Entities
{
    public class UserUserRoles
    {
        [Key]
        public int UserUserRoleID { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public Users? User { get; set; }

        [ForeignKey("UserRoleID")]
        public int UserRoleID { get; set; }
        public UserRoles? UserRole { get; set; }
    }
}
