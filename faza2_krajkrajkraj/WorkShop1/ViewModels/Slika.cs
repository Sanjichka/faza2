using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkShop1.ViewModels
{
    public class Slika
    {
        
            [Required(ErrorMessage = "Please enter first name")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Please enter last name")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            [Required(ErrorMessage = "Please choose profile image")]
            [Display(Name = "Profile Picture")]
            public IFormFile ProfileImage { get; set; }
        
    }

}
