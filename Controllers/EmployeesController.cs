using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DevTest_2.Models;

namespace DevTest_2.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeDbContext db = new EmployeeDbContext();

        // GET: Employees
        public ActionResult Index(string name)
        {
            //Get the list of employees
            var employees = from e
                            in db.Employees
                            select e;

            //Sort by birthday
            employees = employees.OrderBy((emp) => emp.BirthDate);

            //Add optional filtering by name
            if (!string.IsNullOrEmpty(name))
                employees = employees.Where((emp) => emp.Name == name);

            return View(employees.ToList());
        }

        public ActionResult Create() => View(new Employee() { BirthDate = DateTime.Parse("01/01/2000").Date });

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,LastName,RFC,BirthDate,Status")] Employee employee)
        {

            string message = employee.ValidateFields();
            if (message != null)
            {
                ViewBag.Message = message;
                return View(employee);
            }

            if (db.Employees.SingleOrDefault((e) => e.RFC == employee.RFC) != null)
            {
                ViewBag.Message = "There is already an employee with that RFC registered";
                return View(employee);
            }

            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Employee employee = db.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            employee.BirthDate = employee.BirthDate.Date;

            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Name,LastName,RFC,BirthDate,Status")] Employee employee)
        {
            if (!ModelState.IsValid || !employee.ValidateRFC())
                return View(employee);

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Employee employee = db.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
