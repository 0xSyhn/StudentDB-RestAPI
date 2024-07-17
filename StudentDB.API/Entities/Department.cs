using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(80)]
        public string? DepartmentName { get; set; } = string.Empty;
    }
}
