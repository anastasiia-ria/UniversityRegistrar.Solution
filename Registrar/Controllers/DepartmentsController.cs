using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;
using System.Linq;

namespace Registrar.Controllers
{
  public class DepartmentsController : Controller
  {
    private readonly RegistrarContext _db;

    public DepartmentsController(RegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Departments.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Department Department)
    {
      _db.Departments.Add(Department);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisDepartment = _db.Departments.FirstOrDefault(Department => Department.DepartmentId == id);
      var count = _db.CourseStudent.Where(cs => cs.Course.DepartmentId == id && cs.Status == "Incomplete").ToList().Count();
      ViewBag.Count = count;
      return View(thisDepartment);
    }

    public ActionResult Edit(int id)
    {
      var thisDepartment = _db.Departments.FirstOrDefault(Department => Department.DepartmentId == id);
      return View(thisDepartment);
    }

    [HttpPost]
    public ActionResult Edit(Department Department)
    {
      _db.Entry(Department).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // public ActionResult AddCourse(int id)
    // {
    //   var thisDepartment = _db.Departments.FirstOrDefault(Department => Department.DepartmentId == id);
    //   var courses = _db.Courses.Select(course => new { CourseId = course.CourseId, FullName = string.Format("{0}{1}", course.Name, course.Number) }).ToList();
    //   ViewBag.CourseId = new SelectList(courses, "CourseId", "FullName");
    //   return View(thisDepartment);
    // }

    // [HttpPost]
    // public ActionResult AddCourse(Department Department, int CourseId)
    // {
    //   if (CourseId != 0)
    //   {
    //     _db.Course.Add(new CourseStudent() { CourseId = CourseId, DepartmentId = Department.DepartmentId });
    //     _db.SaveChanges();
    //   }
    //   return RedirectToAction("Index");
    // }

    public ActionResult Delete(int id)
    {
      var thisDepartment = _db.Departments.FirstOrDefault(Department => Department.DepartmentId == id);
      return View(thisDepartment);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisDepartment = _db.Departments.FirstOrDefault(Department => Department.DepartmentId == id);
      _db.Departments.Remove(thisDepartment);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
