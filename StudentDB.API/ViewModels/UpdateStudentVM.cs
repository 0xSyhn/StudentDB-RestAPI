namespace StudentDB.API.ViewModels
{
    public class UpdateStudentVM
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; } = string.Empty;


        public string? LastName { get; set; } = string.Empty;

        public DateTime? EnrollmentDate { get; set; }

        public DateTime? GraduationDate { get; set; }

        public int CourseId { get; set; }
    }
}
