using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Areas.Identity.Data;
using WorkShop1.Data;

namespace WorkShop1.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {

            // default user, pa za od nego roles, go krstiv admin namesto user :D

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<WorkShop1Admin>>();
            IdentityResult roleResult;
            IdentityResult roleResult1;
            IdentityResult roleResult2;
            IdentityResult roleResult3;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            WorkShop1Admin admin = await UserManager.FindByEmailAsync("admin@workshop1.com");
            if (admin == null)
            {
                var User = new WorkShop1Admin();
                User.Email = "admin@workshop1.com";
                User.UserName = "admin@workshop1.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            //Add Teacher Role
            var roleCheck1 = await RoleManager.RoleExistsAsync("Teacher");
            if (!roleCheck1) { roleResult1 = await RoleManager.CreateAsync(new IdentityRole("Teacher")); }
            WorkShop1Admin teacher = await UserManager.FindByEmailAsync("teacher@workshop1.com");
            if (teacher == null)
            {
                var User = new WorkShop1Admin();
                User.Email = "teacher@workshop1.com";
                User.UserName = "teacher@workshop1.com";
                string userPWD = "Teacher123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Teacher Admin
                if (chkUser.Succeeded) { var result2 = await UserManager.AddToRoleAsync(User, "Teacher"); }
            }

            //Add Student Role
            var roleCheck2 = await RoleManager.RoleExistsAsync("Student");
            if (!roleCheck2) { roleResult2 = await RoleManager.CreateAsync(new IdentityRole("Student")); }
            WorkShop1Admin student = await UserManager.FindByEmailAsync("student@workshop1.com");
            if (student == null)
            {
                var User = new WorkShop1Admin();
                User.Email = "student@workshop1.com";
                User.UserName = "student@workshop1.com";
                string userPWD = "Student123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Student Admin
                if (chkUser.Succeeded) { var result3 = await UserManager.AddToRoleAsync(User, "Student"); }
            }

            //Add Nikoj Role
            var roleCheck3 = await RoleManager.RoleExistsAsync("Nikoj");
            if (!roleCheck3) { roleResult3 = await RoleManager.CreateAsync(new IdentityRole("Nikoj")); }
            WorkShop1Admin nikoj = await UserManager.FindByEmailAsync("nikoj@workshop1.com");
            if (nikoj == null)
            {
                var User = new WorkShop1Admin();
                User.Email = "nikoj@workshop1.com";
                User.UserName = "nikoj@workshop1.com";
                string userPWD = "Nikoj123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Nikoj Admin
                if (chkUser.Succeeded) { var result4 = await UserManager.AddToRoleAsync(User, "Nikoj"); }
            }
        }



        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WorkShop1Context(
                serviceProvider.GetRequiredService<DbContextOptions<WorkShop1Context>>()))
            {

                CreateUserRoles(serviceProvider).Wait();

                if (context.Students.Any() || context.Teacher.Any() || context.Enrollments.Any() || context.Course.Any()) { return; }

                context.Students.AddRange(new Student { StudentId = "1", FirstName = "Sanja", LastName = "Siljanoska", EnrollmentDate = DateTime.Parse("2021-1-1"), AcquiredCredits = 60, CurrentSemestar = 3, EducationLevel = "Dodiplomski" },
                    new Student { StudentId = "2", FirstName = "V", LastName = "V", EnrollmentDate = DateTime.Parse("2021-2-2"), AcquiredCredits = 90, CurrentSemestar = 4, EducationLevel = "Dodiplomski" });

                context.Course.AddRange(new Course { Title = "RSWEB", Credits = 6, Semester = 4, Programme = "KTI", EducationLevel = "Dodiplomski", FirstTeacherId = 1, SecondTeacherId = 2 },
                    new Course { Title = "MPB", Credits = 6, Semester = 3, Programme = "KTI", EducationLevel = "Dodiplomski", FirstTeacherId = 1, SecondTeacherId = 3 });

                context.Teacher.AddRange(new Teacher { FirstName = "D", LastName = "D", Degree = "Dr", AcademicRank = "Dr", OfficeNumber = "4", HireDate = DateTime.Parse("2018-2-2") },
                    new Teacher { FirstName = "P", LastName = "L", Degree = "Dr", AcademicRank = "Dr", OfficeNumber = "5", HireDate = DateTime.Parse("2018-2-2") },
                    new Teacher { FirstName = "T", LastName = "S", Degree = "Dr", AcademicRank = "Dr", OfficeNumber = "6", HireDate = DateTime.Parse("2017-2-2") });

                context.Enrollments.AddRange(new Enrollment { CourseID = 1, StudentID = 2, Semester = "4", Year = 2, Grade = 10, SeminalUrl="", ProjectUrl = "#", ExamPoints = 95, SeminalPoints = 60, AdditionalPoints = 10, ProjectPoints = 10, FinishDate = DateTime.Parse("2020-4-4") },
                    new Enrollment { CourseID = 2, StudentID = 1, Semester = "3", Year = 2, SeminalUrl = "", ProjectUrl = "#", ExamPoints = 95, SeminalPoints = 60, AdditionalPoints = 10, ProjectPoints = 10, FinishDate = DateTime.Parse("2020-4-4") });

                context.SaveChanges();
            }
        }
    }
}

