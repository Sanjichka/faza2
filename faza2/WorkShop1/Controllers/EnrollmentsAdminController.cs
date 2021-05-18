using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkShop1.Data;
using WorkShop1.Models;
using WorkShop1.ViewModels;

namespace WorkShop1.Controllers
{
    public class EnrollmentsAdminController : Controller
    {
        private readonly WorkShop1Context _context;

        public EnrollmentsAdminController(WorkShop1Context context)
        {
            _context = context;
        }

        // GET: EnrollmentsAdmin
        public async Task<IActionResult> Index()
        {
            var workShop1Context = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await workShop1Context.ToListAsync());
        }

        // GET: EnrollmentsAdmin/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: EnrollmentsAdmin/Create
        public IActionResult Create(int id)
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

        // POST: EnrollmentsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, Filter7 vm)
        {
            IEnumerable<long> listStudents = vm.SelectedStudents;

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

        // GET: EnrollmentsAdmin/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = _context.Course.Where(m => m.CourseID == id).Include(m => m.Enrollments).First();

            if (course == null)
            {
                return NotFound();
            }

            IEnumerable<Student> students = _context.Student.AsEnumerable();


            var viewmodel = new Filter7
            {
                Course = course,
                StudentList = new MultiSelectList(students.AsEnumerable(), "StudentId", "StudentId"),
                SelectedStudents = course.Enrollments.Select(m => m.StudentID),
                Enrollments = _context.Enrollment.Where(m => m.CourseID == id)
            };
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentId", "StudentId");
            return View(viewmodel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Filter7 viewmodel)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    IEnumerable<long> listStudents = viewmodel.SelectedStudents;

                    IEnumerable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentID) && s.CourseID == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<long> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == id).Select(s => s.StudentID);
                    IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    foreach (long stId in newStudents)
                        _context.Enrollment.Add(new Enrollment { StudentID = stId, CourseID = id });


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Set<Student>(), "StudentId", "FullName", viewmodel.SelectedStudents);
            return View(viewmodel);
        }

        // GET: EnrollmentsAdmin/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: EnrollmentsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(long id)
        {
            return _context.Enrollment.Any(e => e.EnrollmentID == id);
        }
    }
}
