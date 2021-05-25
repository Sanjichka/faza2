using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkShop1.Areas.Identity.Data;
using WorkShop1.Models;

namespace WorkShop1.Data
{
    public class WorkShop1Context : IdentityDbContext<WorkShop1Admin>
    {
        public WorkShop1Context (DbContextOptions<WorkShop1Context> options)
            : base(options)
        {
        }

        public DbSet<WorkShop1.Models.Course> Course { get; set; }
        public DbSet<WorkShop1.Models.Student> Student { get; set; }
        public DbSet<WorkShop1.Models.Teacher> Teacher { get; set; }
        public DbSet<WorkShop1.Models.Enrollment> Enrollment { get; set; }

        public DbSet<WorkShop1.Models.Slik> Slik { get; set; }



        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Slik> Sliks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Course>()
                    .HasOne(m => m.FirstTeacher)
                    .WithMany(t => t.Course1)
                    .HasForeignKey(m => m.FirstTeacherId).OnDelete(DeleteBehavior.NoAction);
            // .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
           .HasOne(m => m.SecondTeacher)
           .WithMany(t => t.Course2)
           .HasForeignKey(m => m.SecondTeacherId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Slik>().HasKey(m => m.id);

            modelBuilder.Entity<Enrollment>()
            .HasOne(m => m.Course)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(m => m.CourseID).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrollment>()
          .HasOne(m => m.Student)
                  .WithMany(t => t.Enrollments)
                  .HasForeignKey(m => m.StudentID).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
