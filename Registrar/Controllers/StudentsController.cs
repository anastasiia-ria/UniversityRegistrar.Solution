using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Registrar.Controllers
{
  public class StudentsController : Controller
  {
    private readonly RegistrarContext _db;

    public StudentsController(RegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Students.ToList());
    }

    public ActionResult Create()
    {
      var courses = _db.Courses.Select(course => new { CourseId = course.CourseId, FullName = string.Format("{0}{1}", course.Name, course.Number) }).ToList();
      ViewBag.CourseId = new SelectList(courses, "CourseId", "FullName");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Student student, int CourseId)
    {
      _db.Students.Add(student);
      _db.SaveChanges();
      if (CourseId != 0)
      {
        _db.CourseStudent.Add(new CourseStudent() { CourseId = CourseId, StudentId = student.StudentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisStudent = _db.Students
          .Include(student => student.JoinEntities)
          .ThenInclude(join => join.Course)
          .FirstOrDefault(student => student.StudentId == id);
      return View(thisStudent);
    }

    public ActionResult Edit(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      var courses = _db.Courses.Select(course => new { CourseId = course.CourseId, FullName = string.Format("{0}{1}", course.Name, course.Number) }).ToList();
      ViewBag.CourseId = new SelectList(courses, "CourseId", "FullName");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult Edit(Student student, int CourseId)
    {
      if (CourseId != 0)
      {
        _db.CourseStudent.Add(new CourseStudent() { CourseId = CourseId, StudentId = student.StudentId });
      }
      _db.Entry(student).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCourse(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      var courses = _db.Courses.Select(course => new { CourseId = course.CourseId, FullName = string.Format("{0}{1}", course.Name, course.Number) }).ToList();
      var status = new List<SelectListItem>();
      status.Add(new SelectListItem() { Text = "Completed", Value = "Completed" });
      status.Add(new SelectListItem() { Text = "Incomplete", Value = "Incomplete" });
      ViewBag.Status = status;
      ViewBag.CourseId = new SelectList(courses, "CourseId", "FullName");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult AddCourse(Student student, string Status, int CourseId)
    {
      if (CourseId != 0)
      {
        _db.CourseStudent.Add(new CourseStudent() { CourseId = CourseId, StudentId = student.StudentId, Status = Status });
        _db.SaveChanges();
      }

      return RedirectToAction("Index");
    }

    public ActionResult EditCourses(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      var status = new List<SelectListItem>();
      status.Add(new SelectListItem() { Text = "Completed", Value = "Completed" });
      status.Add(new SelectListItem() { Text = "Incomplete", Value = "Incomplete" });
      ViewBag.Status = status;
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult EditCourses(int CourseStudentId, string Status)
    {
      var thisCourseStudent = _db.CourseStudent.FirstOrDefault(coursestudent => coursestudent.CourseStudentId == CourseStudentId);
      thisCourseStudent.Status = Status;
      _db.Entry(thisCourseStudent).State = EntityState.Modified;
      _db.SaveChanges();

      return RedirectToAction("Details", new { id = thisCourseStudent.StudentId });
    }
    public ActionResult Delete(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      return View(thisStudent);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      _db.Students.Remove(thisStudent);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCourse(int joinId)
    {
      var joinEntry = _db.CourseStudent.FirstOrDefault(entry => entry.CourseStudentId == joinId);
      _db.CourseStudent.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
