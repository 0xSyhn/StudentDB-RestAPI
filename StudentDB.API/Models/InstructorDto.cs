using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDB.API.Models
{
    public class InstructorDto
    {
        [Key]
        public int InstructorId { get; set; }

     
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Status {  get; set; } = string.Empty;
        
        public DateTime? HireDate { get; set; }

        public int AnnualSalary { get; set; }

        public int DepartmentId { get; set; }

    }
}
