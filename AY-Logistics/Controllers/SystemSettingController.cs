using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;
using System.IO;
using AYLogistics.BusinessRules;

namespace AYLogistics.Controllers
{
    public class SystemSettingController : Controller
    {
        //
        // GET: /SystemSetting/

        public ActionResult Index()
        {
            return View(new SystemSetting());
        }

       [HttpPost]
        public ActionResult UpdateManifestFormat(SystemSetting SS)
        {
            if (SS.UpdateManifestFormating())
            {
                ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
                return View("Index", new SystemSetting()).Success("Manifest Formating has been updated successfully");
            }
            else
            {
                ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                return View("Index", new SystemSetting()).Error("Not Updated, please try again!");
            }
        }

       [HttpPost]
       public ActionResult UpdateJobFormat(SystemSetting SS)
       {
           if (SS.UpdateJobFormating())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("Job Formating has been updated successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Not Updated, please try again!");
           }
       }

       [HttpPost]
       public ActionResult UpdateQuotFormat(SystemSetting SS)
       {
           if (SS.UpdateQuotFormating())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("Quotation Formating has been updated successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Not Updated, please try again!");
           }
       }

       [HttpPost]
       public ActionResult UpdateDCFormat(SystemSetting SS)
       {
           if (SS.UpdateDCFormating())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("Daily Clearing Formating has been updated successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Not Updated, please try again!");
           }
       }

       public ActionResult UpdateBLFormat(SystemSetting SS)
       {
           if (SS.UpdateBLFormating())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("BL Formating has been updated successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Not Updated, please try again!");
           }
       }

       [HttpPost]
       public ActionResult AddCommodity(SystemSetting SS)
       {
           if (SS.AddCommodity())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("Commodity has been added successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Failed, please try again!");
           }
       }

       [HttpPost]
       public ActionResult AddDocType(SystemSetting SS)
       {
           if (SS.AddDocType())
           {
               ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Success("Document Type has been added successfully");
           }
           else
           {
               ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
               return View("Index", new SystemSetting()).Error("Failed, please try again!");
           }
       }

    }
}
