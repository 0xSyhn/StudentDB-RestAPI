using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDB.API.Models
{
    public class StudentCoursesDto
    {
        [Key]
        public int StudentCourseId {  get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }
    }
}
