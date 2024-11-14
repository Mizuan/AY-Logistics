using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllEmployee()
        {
            return Json(EmployeeModel.GetAllEmployee(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeDetails(int id)
        {
            EmployeeModel Employee = EmployeeModel.GetEmployeeDetail(id);
            return View(Employee);
        }

        public ActionResult AddNewEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewEmployee(EmployeeModel EmpModel)
        {
            if (ModelState.IsValid)
            {
                string NICcheck = EmployeeModel.GetEmployeeNIC(EmpModel.NIC);
                if (NICcheck != null)
                {
                    ViewBag.type = "alert-warning";// this ViewBagType is using to display notification message for My Custom CSS only
                    return View(EmpModel).Warning("This Employee already exist, you could not be doublicate the data!");
                }
                if (EmpModel.AddNewEmployee())
                {
                    int LastEmployeeId = EmployeeModel.GetLastEmployeeId();
                    EmpModel.AddEmployeeToACSUser(LastEmployeeId);
                    ModelState.Clear(); //to clear model
                    ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
                    return View("AddNewEmployee").Success("Employee has been added successfully") ;
                }
                else
                {
                    ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                    return View("AddNewEmployee").Error("Employee not Added, please try again!");
                }
            }
            else
            {
                ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                return View("AddNewEmployee").Error("Employee not Added, please try again!");
            }
        }

        public ActionResult EditEmployee(int id)
        {
            EmployeeModel Employee = EmployeeModel.GetEmployeeDetailforEdit(id);
            return View(Employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeModel editEmpModel, int id)
        {
            //if (ModelState.IsValid)
            if (editEmpModel.UpdateEmployee())
            {
              //  editEmpModel.UpdateEmployee();

                EmployeeModel Employee = EmployeeModel.GetEmployeeDetailforEdit(id);

                ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
                return View(Employee).Success("Employee has been updated successfully!");
            }

            ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
            return View().Error("Employee has not been updated, please try again!");
        }

    }
}
