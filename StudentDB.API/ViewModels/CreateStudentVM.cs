using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.ViewModels
{
    public class CreateStudentVM
    {
        

        public string? FirstName { get; set; } = string.Empty;


        public string? LastName { get; set; } = string.Empty;

        public DateTime? EnrollmentDate { get; set; }

        public DateTime? GraduationDate { get; set; }

        public int CourseId { get; set; }
    }
}
