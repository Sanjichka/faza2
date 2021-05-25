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
    public class CoursesStudController : Controller
    {
        private readonly WorkShop1Context _context;

        public CoursesStudController(WorkShop1Context context)
        {
            _context = context;
        }

        // GET: CoursesStud
        [Authorize(Roles = "Student")]
        public IActionResult Index(long? id)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
            enrollments = enrollments.Include(c => c.Student).Include(c => c.Course);
            enrollments = enrollments.Where(s => s.StudentID == id);

            IEnumerable<int> enrolsostud = enrollments.OrderBy(e => e.CourseID).Select(e => e.CourseID).Distinct();

            courses = courses.Include(c => c.Enrollments).ThenInclude(c => c.Student);

            courses = courses.Where(s => enrolsostud.Contains(s.CourseID));

            var vm = new Filter5
            {
                Enrollments = enrollments,
                Courses = courses
            };

            ViewData["Student"] = _context.Student.Where(s => s.ID.Equals(id)).Select(s => s.FullName).FirstOrDefault();

            return View(vm);
        }

        // GET: CoursesStud/Details/5
        [Authorize(Roles = "Student")]
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

        // GET: CoursesStud/Create
        [Authorize(Roles = "Nikoj")]
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName");
            return View();
        }

        // POST: CoursesStud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Nikoj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: CoursesStud/Edit/5
        [Authorize(Roles = "Nikoj")]
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // POST: CoursesStud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Nikoj")]
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
                return RedirectToAction(
            nameof(Index), // action name
            nameof(HomeController).Replace("Controller", "") // controller name
        );
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: CoursesStud/Delete/5
        [Authorize(Roles = "Nikoj")]
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

        // POST: CoursesStud/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Nikoj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
