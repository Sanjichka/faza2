using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter
    {
        public IEnumerable<Course> Courses { get; set; }

        public string searchTitle { get; set; }
        public SelectList SemsList { get; set; }
        public int searchSem { get; set; }
        public string searchProgr { get; set; }

    }
}
