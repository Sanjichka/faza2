﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkShop1.Models
{
    // project verzija 1 cz na obichnata zaglaviv so internal server error, se nadevam nema da ima ponatamoshni greshki
    // i da stignam do verzija 100 :P
    public class Teacher
    {
            [Required]
            public int TeacherId { get; set; }
            [Required]
            [StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [StringLength(50)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [StringLength(50)]
            public string Degree { get; set; }
            [Display(Name = "Academic Rank")]
            [StringLength(25)]
            public string AcademicRank { get; set; }
            [Display(Name = "Office Number")]
            [StringLength(10)]
            public string OfficeNumber { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Hire Date")]
            public DateTime? HireDate { get; set; }

            [Display(Name = "Full Name")]

            public string FullName
            {
                get { return FirstName + " " + LastName; }
            }

            public ICollection<Course> Course1 { get; set; }
            public ICollection<Course> Course2 { get; set; }


    }
}
