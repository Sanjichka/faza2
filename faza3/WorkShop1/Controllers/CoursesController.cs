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
    public class CoursesController : Controller
    {
        private readonly WorkShop1Context _context;

        public CoursesController(WorkShop1Context context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        // GET: Courses
        public IActionResult Index(string searchProgr, string searchTitle, int searchSem)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> query = _context.Course.OrderBy(m => m.Semester).Select(m => m.Semester).Distinct();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                courses = courses.Where(s => s.Title.ToLower().Contains(searchTitle.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchProgr))
            {
                courses = courses.Where(s => s.Programme.ToLower().Contains(searchProgr.ToLower()));
            }
            if (searchSem != 0)
            {
                courses = courses.Where(s => s.Semester == searchSem);
            }

            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);

            var VM = new Filter
            {
                SemsList = new SelectList(query.AsEnumerable()),
                Courses = courses.AsEnumerable()
            };


            return View(VM);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult StudentsPoCourse(int? id)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
            enrollments = enrollments.Include(c => c.Student).Include(c => c.Course);
            enrollments = enrollments.Where(s => s.CourseID == id);

            IEnumerable<long> enrolsostud = enrollments.OrderBy(e => e.StudentID).Select(e => e.StudentID).Distinct();

            students = students.Include(c => c.Enrollments).ThenInclude(c => c.Course);

            students = students.Where(s => enrolsostud.Contains(s.ID));

            ViewData["Predmet"] = _context.Course.Where(s => s.CourseID == id).Select(s => s.Title).FirstOrDefault();

            return View(students);
        }

        [Authorize(Roles = "Admin")]
        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.SecondTeacherId);
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.SecondTeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", course.SecondTeacherId);
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
