using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter6
    {
        public Course Course { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public int year { get; set; }
        public string semester { get; set; }
        public IEnumerable<long> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentList { get; set; }
    }
}
