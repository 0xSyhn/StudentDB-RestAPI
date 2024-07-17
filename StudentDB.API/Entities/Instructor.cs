using StudentDB.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public int AnnualSalary { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public int DepartmentId { get; set; }

    }
}
