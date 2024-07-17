using StudentDB.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string CourseNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? CourseDescription { get; set; }

        public int CourseUnits { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        public int InstructorId { get; set; }
    }
}
