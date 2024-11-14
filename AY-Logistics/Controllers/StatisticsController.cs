using AYLogistics.Attributes;
using AYLogistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AYLogistics.Controllers
{
    public class StatisticsController : Controller
    {
        //
        // GET: /Statistics/
       // [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AllData(DateTime StartDate, DateTime EndDate)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = StatisticsModel.GetAllData(StartDate, EndDate);
            var Output = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return Output;
        }

    }
}
