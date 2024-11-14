using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Designation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDesignation(Designation_Settings ASModel)
        {
            if (ASModel.AddDesignation())
            {
                ViewBag.type = "alert-success";
                ModelState.Clear();
                return View("Designation").Success("Designation has been added successfully!");
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Designation").Error("Designation has not been added, please try again!");
            }
        }

        [HttpPost]
        public ActionResult EditDesignation(Designation_Settings ASModel)
        {
            if (ASModel.EditDesignation())
            {
                ViewBag.type = "alert-success";
                ModelState.Clear();
                return View("Designation").Success("Designation has been Edited successfully!");
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Designation").Error("Designation has not been Edited, please try again!");
            }
        }

    }
}
