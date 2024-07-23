using Microsoft.EntityFrameworkCore;
using StudentDB.API.Entities;
using StudentDB.API.Models;


namespace StudentDB.API.DbContexts
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserUserRoles> UserUsersRoles { get; set; }

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<Course>()
              .HasOne(c => c.Department)
              .WithMany()
              .HasForeignKey(c => c.DepartmentId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany()
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany()
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<StudentCourse>()
              .HasOne(sc => sc.Student)
              .WithMany()
              .HasForeignKey(sc => sc.StudentId);
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany()
                .HasForeignKey(sc => sc.CourseId);


            modelBuilder.Entity<Department>()
                .Property(d => d.DepartmentId)
                .ValueGeneratedNever();

            modelBuilder.Entity<Instructor>()
                .Property(i => i.InstructorId)
                .ValueGeneratedNever();

 


            modelBuilder.Entity<Student>().HasData(new Student
            {
                StudentId = 1,
                FirstName = "Sayhan",
                LastName = "Ali",
                EnrollmentDate = new DateTime(2021, 8, 13),
                GraduationDate = new DateTime(2025,8,10)
            });

            modelBuilder.Entity<Department>().HasData(new Department
            {
                DepartmentId = 1,
                DepartmentName = "Computers"
            });

        }
    }
}
