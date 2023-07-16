using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADO_Example.DAL;
using ADO_Example.Models;

namespace ADO_Example.Controllers
{
    public class EmployeeController : Controller
    {
        Employee_DAL _EmployeeDAL = new Employee_DAL();

        
        // GET: Employee
        public ActionResult Index()
        {
            var EmpList = _EmployeeDAL.GetAllEmployee();
            if(EmpList.Count == 0)
            {
                TempData["Massage"] = "Currently no Employee Found";
            }
            return View(EmpList);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            bool IsInserted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    IsInserted = _EmployeeDAL.InsertEmployee(employee);

                    if (IsInserted)
                    {
                        TempData["SussessMsg"] = "Save Successfully";
                    }
                    else
                    {
                        TempData["Error"] = "Already have the same name";
                    }
                }
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }

        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
