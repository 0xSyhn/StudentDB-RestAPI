using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Models
{
    public class DepartmentDto
    {
        [Key]
        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; } = string.Empty;
    }
}
