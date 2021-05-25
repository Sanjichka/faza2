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
    public class TeachersController : Controller
    {
        private readonly WorkShop1Context _context;

        public TeachersController(WorkShop1Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Teachers
        public IActionResult Index(string searchIme, string searchPrezime, string searchDegree, string searchRank)
        {
            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<string> query = _context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct();
            IQueryable<string> query1 = _context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct();

            if (!string.IsNullOrEmpty(searchDegree))
            {
                teachers = teachers.Where(s => s.Degree.ToLower() == searchDegree.ToLower());
            }
            if (!string.IsNullOrEmpty(searchRank))
            {
                teachers = teachers.Where(s => s.AcademicRank.ToLower() == searchRank.ToLower());
            }
            if (!string.IsNullOrEmpty(searchIme))
            {
                teachers = teachers.Where(s => s.FirstName.ToLower().Contains(searchIme.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPrezime))
            {
                teachers = teachers.Where(s => s.LastName.ToLower().Contains(searchPrezime.ToLower()));
            }

            var VM1 = new Filter1
            {
                DegreeList = new SelectList(query.AsEnumerable()),
                RankList = new SelectList(query1.AsEnumerable()),
                Teachers = teachers.AsEnumerable()
            };


            return View(VM1);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Courses(int? id)
        {


            IEnumerable<Course> courses = (IEnumerable<Course>)_context.Courses
                .Where(m => m.FirstTeacherId == id || m.SecondTeacherId == id);

            ViewData["Teacher"] = _context.Teacher.Where(s => s.TeacherId == id).Select(s => s.FullName).FirstOrDefault();

            return View(courses);
        }

        // GET: Teachers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherId == id);
        }
    }
}
