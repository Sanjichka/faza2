using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Models;

namespace WorkShop1.ViewModels
{
    public class Filter1
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public string searchIme { get; set; }
        public string searchPrezime { get; set; }
        public string searchDegree { get; set; }
        public SelectList DegreeList { get; set; }
        public string searchRank { get; set; }
        public SelectList RankList { get; set; }
    }
}
