using Microsoft.AspNetCore.Mvc.Rendering;
using WorkShop1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkShop1.ViewModels
{
    public class DismAdmin
    {
        public long EnrollmentID { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public long StudentID { get; set; }
        public Student Student { get; set; }

        [StringLength(10)]
        public string Semester { get; set; }

        public int Year { get; set; }

        public Nullable<int> Grade { get; set; }

        [StringLength(255)]
        [Display(Name = "Seminal Url")]
        public string SeminalUrl { get; set; }

        [StringLength(255)]
        [Display(Name = "Project Url")]
        public string ProjectUrl { get; set; }

        [Display(Name = "Exam Points")]
        public Nullable<int> ExamPoints { get; set; }

        [Display(Name = "Seminal Points")]
        public Nullable<int> SeminalPoints { get; set; }

        [Display(Name = "Additional Points")]
        public Nullable<int> AdditionalPoints { get; set; }

        [Display(Name = "Project Points")]
        public Nullable<int> ProjectPoints { get; set; }

        [Display(Name = "Finish Date")]
        public Nullable<DateTime> FinishDate { get; set; }

        public string Seminal { get; set; }
        public IEnumerable<long> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentList { get; set; }
    }
}

