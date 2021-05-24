using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;
using WorkShop1.ViewModels;

namespace WorkShop1.ViewModels
{
    public class Filter2
    {
        public IEnumerable<Student> Students { get; set; }

        public string searchStudentId { get; set; }
        public string searchIme { get; set; }
    }
}
