using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter4
    {
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public int year { get; set; }
        public IEnumerable<Student> Students { get; set; }
        
    }
}
