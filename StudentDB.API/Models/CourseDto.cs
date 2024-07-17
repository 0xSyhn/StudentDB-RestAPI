using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDB.API.Models
{
    public class CourseDto
    {
        [Key]
        public int CourseId {  get; set; }
        
      
        public string CourseNumber { get; set; }

    
        public string CourseName { get; set; } = string.Empty;

     
        public string? CourseDescription { get; set; }

        public int CourseUnits { get; set; }


        public int DepartmentId{ get; set; }

  

        public int InstructorId{ get; set; }


    }
}
