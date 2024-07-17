using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; } = string.Empty;

        [Required]
        public DateTime? EnrollmentDate { get; set; }

        [Required]
        public DateTime? GraduationDate { get; set; }
    }
}
