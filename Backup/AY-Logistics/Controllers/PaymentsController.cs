using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class PaymentsController : Controller
    {
        //
        // GET: /Payments/
        public ActionResult RequestedPayments()
        {
            return View();
        }

        public ActionResult UnpaidPayments()
        {
            return View();
        }


        public void RequestDOPayement(DOPayment Model)
        {
            if (Model.SaveDOPayement())
            {
                ViewBag.message = "Insert Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public void RequestDOPayementAIR(DOPayment Model)
        {
            if (Model.SaveDOPayement())
            {
                ViewBag.message = "Insert Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public void ApproveDOPayement(DOPayment Model)
        {
            if (Model.ApproveDOPayement())
            {
                List<Dictionary<object, object>> HBLIdList = ManifestModel.GetHBLIdList(Model.shipmentId);
                foreach (Dictionary<object, object> dictionary in HBLIdList)
                {
                    Model.SavePaymentStat(Convert.ToInt32(dictionary["HBLid"]));
                }
                ViewBag.message = "Insert Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }

        }

        public JsonResult GetUnpaid()
        {
            return Json(PaymentModel.GetUnpaidList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCollection(int PaymentId, int HBLId)
        {
            PaymentModel edit = new PaymentModel(PaymentId);
            edit.HBLid = HBLId;
            return View(edit);
        }

        /* START Dash Board*/
        public JsonResult GetDashboardItem()
        {
            PaymentDashBoard dashboard = new PaymentDashBoard();

            return Json(dashboard.Getdashboarditem(), JsonRequestBehavior.AllowGet);
        }
        /* END Dash Board*/

    }
}
