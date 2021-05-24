using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter3
    {
        public IEnumerable<Course> Courses { get; set; }
        public Enrollment Enrollment { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
