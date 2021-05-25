using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkShop1.Data;
using WorkShop1.Models;
using WorkShop1.ViewModels;

namespace WorkShop1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly WorkShop1Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentsController(WorkShop1Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Students
        [Authorize(Roles = "Admin")]
        public IActionResult Index(string searchIme, string searchStudentId)
        {
            IEnumerable<Student> students = _context.Student.AsEnumerable();

            if (!string.IsNullOrEmpty(searchIme))
            {
                students = students.Where(s => s.FullName.ToLower().Contains(searchIme.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchStudentId))
            {
                students = students.Where(s => s.StudentId.Contains(searchStudentId));
            }

            var VM2 = new Filter2
            {
                Students = students
            };


            return View(VM2);
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentVM Vmodel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(Vmodel);

                Student student = new Student
                {
                    ProfilePicture = uniqueFileName,
                    StudentId = Vmodel.StudentId,
                    FirstName = Vmodel.FirstName,
                    LastName = Vmodel.LastName,
                    EnrollmentDate = Vmodel.EnrollmentDate,
                    AcquiredCredits = Vmodel.AcquiredCredits,
                    CurrentSemestar = Vmodel.CurrentSemestar,
                    EducationLevel = Vmodel.EducationLevel,
                    Enrollments = Vmodel.Enrollments,
                };

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        private string UploadedFile(StudentVM model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            StudentVM Vmodel = new StudentVM
            {
                ID = student.ID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentId = student.StudentId,
                EnrollmentDate = student.EnrollmentDate,
                AcquiredCredits = student.AcquiredCredits,
                CurrentSemestar = student.CurrentSemestar,
                EducationLevel = student.EducationLevel,
                Enrollments = student.Enrollments
            };
            return View(Vmodel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, StudentVM Vmodel)
        {
            if (id != Vmodel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(Vmodel);

                    Student student = new Student
                    {
                        ID = Vmodel.ID,
                        FirstName = Vmodel.FirstName,
                        LastName = Vmodel.LastName,
                        ProfilePicture = uniqueFileName,
                        EnrollmentDate = Vmodel.EnrollmentDate,
                        CurrentSemestar = Vmodel.CurrentSemestar,
                        AcquiredCredits = Vmodel.AcquiredCredits,
                        StudentId = Vmodel.StudentId,
                        EducationLevel = Vmodel.EducationLevel,
                        Enrollments = Vmodel.Enrollments
                    };
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(Vmodel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Vmodel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
