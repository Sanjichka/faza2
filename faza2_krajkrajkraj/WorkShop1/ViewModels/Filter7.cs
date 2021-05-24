using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter7
    {
    
    public Course Course { get; set; }
    public IEnumerable<Enrollment> Enrollments { get; set; }
    public IEnumerable<long> SelectedStudents { get; set; }
    public IEnumerable<SelectListItem> StudentList { get; set; }

    }
}
