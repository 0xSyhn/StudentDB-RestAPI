namespace StudentDB.API.ViewModels
{
    public class InstructorVM
    {
        public int InstructorId { get; set; }


        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public int AnnualSalary { get; set; }

        public int DepartmentId { get; set; }
    }
}
