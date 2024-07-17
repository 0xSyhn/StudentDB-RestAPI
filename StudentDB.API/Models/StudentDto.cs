using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Models
{
    public class StudentDto
    {
        [Key]
        public int StudentId { get; set; }

     
        public string? FirstName { get; set; } = string.Empty;

     
        public string? LastName { get; set; } = string.Empty;

        public DateTime? EnrollmentDate { get; set; }

        public DateTime? GraduationDate { get; set; }

    }
}
