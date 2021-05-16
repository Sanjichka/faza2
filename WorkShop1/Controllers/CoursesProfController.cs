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
    public class CoursesProfController : Controller
    {
        private readonly WorkShop1Context _context;

        public CoursesProfController(WorkShop1Context context)
        {
            _context = context;
        }

        // GET: CoursesProf
        public async Task<IActionResult> Index(int? id)
        {
            var workShop1Context = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Where(c => c.FirstTeacherId == id || c.SecondTeacherId == id);
            ViewData["Prof"] = _context.Teacher.Where(s => s.TeacherId == id).Select(s => s.FullName).FirstOrDefault();
            return View(await workShop1Context.ToListAsync());
        }

        public IActionResult StudPoCourse(int? id, int year)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
            enrollments = enrollments.Include(c => c.Student).Include(c => c.Course);
            enrollments = enrollments.Where(s => s.CourseID == id);
            if (year != 0)
            {
                enrollments = enrollments.Where(s => s.Year == year);
            }

            IEnumerable<long> enrolsostud = enrollments.OrderBy(e => e.StudentID).Select(e => e.StudentID).Distinct();

            students = students.Include(c => c.Enrollments).ThenInclude(c => c.Course);

            students = students.Where(s => enrolsostud.Contains(s.ID));

            var vm = new Filter4
            {
                Enrollments = enrollments,
                Students = students
            };

            ViewData["Predmet"] = _context.Course.Where(s => s.CourseID == id).Select(s => s.Title).FirstOrDefault();

            return View(vm);
        }

        // GET: CoursesProf/Details/5
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

        // GET: CoursesProf/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName");
            return View();
        }

        // POST: CoursesProf/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: CoursesProf/Edit/5
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

        // POST: CoursesProf/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: CoursesProf/Delete/5
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

        // POST: CoursesProf/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
