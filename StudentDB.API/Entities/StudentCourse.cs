using StudentDB.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentDB.API.Entities
{
    public class StudentCourse
    {
        [Key]
        public int StudentCourseId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        public int StudentId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public int CourseId { get; set; }
    }
}
