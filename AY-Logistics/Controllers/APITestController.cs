using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AYLogistics.Services;

namespace AYLogistics.Controllers
{
    public class APITestController : Controller
    {
        //
        // GET: /APITest/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Test()
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            //var Data = MPLAuthService.GetActiveSessions();
            // var Data = MPLAuthService.GetActiveSessionsByName("2024-09-18");
            /* var DId = MPLAuthService.GetActiveSessionIdByName("2024-09-16");
                 int SessionId = DId[0];
                 var Data = MPLAuthService.GetRequestedContainerByDate(SessionId);*/
            // var Data = MPLAuthService.GetUserDetail();
            // var Data = MPLAuthService.GetBillOfLadingDocument();
            // var Data = MPLAuthService.GetBillOfLadingDocumentByNumber("949700061762B");
            /* var BLId = MPLAuthService.GetBillOfLadingDocumentIdByNumber("ESFSEC24060241");
             var Data = MPLAuthService.GetBillOfLadingDocumentDetail(BLId.Value);*/
            // var Data = MPLAuthService.GetContainerDetail();
            // var Data = MPLAuthService.GetContainerDetail("FSCU3760665");
            var Data = MPLAuthService.GetInvoice("AFTMLE240800031");
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserDetail()
        {
            string status = MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetUserDetail();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActiveSessions()
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetActiveSessions();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActiveSessionsByName(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetActiveSessionsByName(myVal);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestedContainerByDate(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var DId = MPLAuthService.GetActiveSessionIdByName(myVal);
                 int SessionId = DId[0];
                 var Data = MPLAuthService.GetRequestedContainerByDate(SessionId);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBillOfLadingDocument()
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
             var Data = MPLAuthService.GetBillOfLadingDocument();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBillOfLadingDocumentByNumber(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetBillOfLadingDocumentByNumber(myVal);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBillOfLadingDocumentDetail(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
             var BLId = MPLAuthService.GetBillOfLadingDocumentIdByNumber(myVal);
             var Data = MPLAuthService.GetBillOfLadingDocumentDetail(BLId.Value);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContainerDetail(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetContainerDetail(myVal);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoice(string myVal)
        {
            MPLAuthService.Authenticate("CENTARA766", "CENTARA766");
            var Data = MPLAuthService.GetInvoice(myVal);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

    }
}
