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

        public IActionResult Administrator_Create()
        {

            var students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);
            EnrolAdminVM viewmodel = new EnrolAdminVM
            {
                StudentList = new MultiSelectList(students, "ID", "FullName"),
                SelectedStudents = (IEnumerable<long>)_context.Enrollment.Select(m => m.StudentID)
            };
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseID", "Title");
            return View(viewmodel);
        }



        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Administrator_Create(EnrolAdminVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<long> listStudents = viewmodel.SelectedStudents;
                    IQueryable<Student> toBeRemoved = _context.Student.Where(s => !listStudents.Contains(s.ID) && s.ID == viewmodel.StudentID); //?
                    _context.Student.RemoveRange(toBeRemoved);
                    IEnumerable<long> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == viewmodel.CourseID).Select(s => s.StudentID);
                    IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    /*var enrollment = await _context.Enrollment.FindAsync(id);*/
                    foreach (int studentId in newStudents)
                        _context.Enrollment.Add(new Enrollment
                        {
                            StudentID = studentId,
                            CourseID = viewmodel.CourseID,
                            Year = viewmodel.Year,
                            Semester = viewmodel.Semester,
                            Grade = null,
                            SeminalUrl = null,
                            ProjectUrl = null,
                            ExamPoints = null,
                            SeminalPoints = null,
                            AdditionalPoints = null,
                            ProjectPoints = null,
                            FinishDate = null
                        });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(viewmodel.EnrollmentID)) { return NotFound(); }
                    else { throw; }
                }
                ViewData["CourseId"] = new SelectList(_context.Course, "CourseID", "Title", viewmodel.CourseID);
                return RedirectToAction("Index", "Enrollments");
            }
            return View(viewmodel);
        }

        public IActionResult Administrator_Dismiss()
        {

            var students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);
            DismAdmin viewmodel = new DismAdmin
            {
                StudentList = new MultiSelectList(students, "ID", "FullName"),/*
                SelectedStudents = _context.Enrollment.Select(m => m.StudentId)*/
            };
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseID", "Title");
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Administrator_Dismiss(DismAdmin viewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<long> listStudents = viewmodel.SelectedStudents;
                    IQueryable<Student> toBeRemoved = _context.Student.Where(s => !listStudents.Contains(s.ID) && s.ID == viewmodel.StudentID); //?
                    _context.Student.RemoveRange(toBeRemoved);
                    IEnumerable<Enrollment> existEnrollments = _context.Enrollment.Where(s => listStudents.Contains(s.StudentID) && s.CourseID == viewmodel.CourseID);
                    /*IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));*/
                    /*var enrollment = await _context.Enrollment.FindAsync(id);*/
                    foreach (Enrollment enrollment in existEnrollments)
                        enrollment.FinishDate = viewmodel.FinishDate;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(viewmodel.EnrollmentID)) { return NotFound(); }
                    else { throw; }
                }
                ViewData["CourseId"] = new SelectList(_context.Course, "CourseID", "Title", viewmodel.CourseID);
                return RedirectToAction("Index", "Enrollments");
            }
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
