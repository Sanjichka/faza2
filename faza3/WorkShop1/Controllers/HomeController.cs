using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorkShop1.Data;
using WorkShop1.Models;
using WorkShop1.ViewModels;

namespace WorkShop1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WorkShop1Context dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, WorkShop1Context context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexNew()
        {
            var slika = await dbContext.Slik.ToListAsync();
            return View(slika);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Slika model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Slik slika = new Slik
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImage = uniqueFileName,
                };

                dbContext.Add(slika);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(Slika model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [Authorize(Roles = "Teacher, Admin, Student")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Teacher, Admin, Student")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


