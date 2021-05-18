using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Data;

namespace WorkShop1.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WorkShop1Context(
                serviceProvider.GetRequiredService<DbContextOptions<WorkShop1Context>>()))
            {
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

