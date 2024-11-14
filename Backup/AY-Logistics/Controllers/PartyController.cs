using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class PartyController : Controller
    {
        //
        // GET: /Party/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void SaveParty(Party PModel)
        {
            if (PModel.SaveParty())
            {
                ViewBag.message = "Add Success";
            }
            else {
                Response.Write("error");
                Response.End();
            }
        }

        public ActionResult GetSelectedParty(int PartyId)
        {
            Party PModel = new Party();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(PModel.GetSelectedParty(PartyId), JsonRequestBehavior.AllowGet);

            return Json(PModel.GetSelectedParty(PartyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateParty(Party PModel)
        {
            if (PModel.UpdateParty ())
            {
                ViewBag.message = "Update Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        [HttpGet]
        public ActionResult SearchParty(string query, int? type, int? subtype)
        {
            return Json(AYLogistics.Models.Party.Search(query, type.Value, subtype.Value), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchVessel(string query, int? type, int? subtype)
        {
            return Json(AYLogistics.Models.Vessel.Search(query, type.Value, subtype.Value), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void SaveVessel(Vessel VModel)
        {
            if (VModel.SaveVessel())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public ActionResult GetSelectedVessel(int VesselId)
        {
            Vessel VModel = new Vessel();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(VModel.GetSelectedVessel(VesselId), JsonRequestBehavior.AllowGet);

            return Json(VModel.GetSelectedVessel(VesselId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateVessel(Vessel VModel)
        {
            if (VModel.UpdateVessel())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public JsonResult GetContacts()
        {
            return Json(Party.GetContactList(), JsonRequestBehavior.AllowGet);
        }

    }
}
