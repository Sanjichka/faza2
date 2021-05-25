using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkShop1.Data;
using WorkShop1.Models;
using WorkShop1.ViewModels;

namespace WorkShop1.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly WorkShop1Context _context;

        public EnrollmentsController(WorkShop1Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var workShop1Context = _context.Enrollments.Include(e => e.Student).Include(e => e.Course).ThenInclude(e => e.FirstTeacher);
            workShop1Context = workShop1Context.Include(e => e.Course).ThenInclude(e => e.SecondTeacher);
            return View(await workShop1Context.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index1()
        {

            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID");
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID");


            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index1([Bind("EnrollmentID,CourseID,StudentID,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index2(int? id)
        {
            var course = _context.Course.Where(m => m.CourseID == id).Include(m => m.Enrollments).First();
            IEnumerable<Student> students = _context.Student.AsEnumerable();

            var vm = new Filter7
            {
                Course = course,
                StudentList = new MultiSelectList(students.AsEnumerable(), "StudentId", "StudentId"),
                SelectedStudents = course.Enrollments.Select(m => m.StudentID)
            };


            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID");


            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index2(int id, Filter7 vm)
        {
            IEnumerable<long> listStudents = vm.SelectedStudents;

            //IEnumerable<long> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == id).Select(s => s.StudentID);
            //IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
            

            if (ModelState.IsValid)
            {
                foreach (long stID in listStudents)
            {
                _context.Enrollment.Add(new Enrollment { CourseID = id, StudentID = stID });
            }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }



        // GET: Enrollments/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID");
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("EnrollmentID,CourseID,StudentID,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EnrollmentID,CourseID,StudentID,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID))
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
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool EnrollmentExists(long id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}
