using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;
using MyReports;

namespace AYLogistics.Controllers
{
    public class JobController : Controller
    {
        //
        // GET: /Job/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AdvanceSearch()
        {
            return View();
        }

        public ActionResult GetCommodityType()
        {
            return Json(Commodity.GetCommodityType(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProject()
        {
            return Json(Project.GetProject(), "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchProject(string query, int? type, int? subtype)
        {
            return Json(AYLogistics.Models.Project.Search(query, type.Value, subtype.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVehicle()
        {
            return Json(Vehicle.GetVehicle(), "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchVehicle(string query, int? type, int? subtype)
        {
            return Json(AYLogistics.Models.Vehicle.Search(query, type.Value, subtype.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobPriority()
        {
            return Json(JobPriority.GetJobPriority(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClearanceMode()
        {
            return Json(ClearanceModel.GetClearanceMode(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClearanceShift()
        {
            return Json(ClearanceModel.GetClearanceShift(), "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClearancePort()
        {
            return Json(ClearanceModel.GetClearancePort(), "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveProject(Project PModel)
        {
            if (PModel.SaveProject())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        [HttpPost]
        public void SaveVehicle(Vehicle VModel)
        {
            if (VModel.SaveVehicle())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public ActionResult GetSelectedProject(int ProjectId)
        {
            Project PModel = new Project();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(PModel.GetSelectedProject(ProjectId), JsonRequestBehavior.AllowGet);

            return Json(PModel.GetSelectedProject(ProjectId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSelectedVehicle(int VehicleId)
        {
            Vehicle VModel = new Vehicle();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(VModel.GetSelectedVehicle(VehicleId), JsonRequestBehavior.AllowGet);

            return Json(VModel.GetSelectedVehicle(VehicleId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateProject(Project PModel)
        {
            if (PModel.UpdateProject())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }
        [HttpPost]
        public void UpdateVehicle(Vehicle VModel)
        {
            if (VModel.UpdateVehicle())
            {
                ViewBag.message = "Add Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public ActionResult SaveJob(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.HouseBLItems.Count != 0)
            {
                /**add Manifest**/
                MM.SaveShipment();
                int LastShipmentId = ManifestModel.GetLastShipmentId();
                /**add BL Item and BL status*/
                MM.SaveHouseBLItems(MM.HouseBLItems, LastShipmentId, 4);
                int LastBLId = ManifestModel.GetLastBLId();
                MM.SaveJob(LastBLId, MM.ModeofShipment);
                MM.UpdateJobSequence();
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Job has been created successfully!");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Job has NOT been created, please fill the required info!" +
                                "<br>* VoyageNo,<br>* Vessel,<br>* Mode of Shipment,<br>* Shipper" +
                                ",<br>* Customer,<br>* Container Type");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveJobUnderMBL(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.HouseBLItems.Count != 0)
            {
                /**add Manifest**/
                MM.UpdateShipment();
                /**add BL Item and BL status*/
                MM.SaveHouseBLItems(MM.HouseBLItems, MM.ShipmentId, 4);
                int LastBLId = ManifestModel.GetLastBLId();
                MM.SaveJob(LastBLId, MM.ModeofShipment);
                MM.UpdateJobSequence();
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Job has been created successfully!");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Job has NOT been created, please fill the required info!" +
                                "<br>* VoyageNo,<br>* Vessel,<br>* Mode of Shipment,<br>* Shipper" +
                                ",<br>* Customer,<br>* Container Type");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConvertManifestToJob(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.VesselId.id != 0 && MM.HouseBLItems.Count != 0)
            {
                /**Update Manifest**/
                MM.UpdateManifest();
                /**Update BL Item and status*/
                MM.UpdateHouseBLItemsConvertJob(MM.HouseBLItems, MM.ShipmentId, 4);
                MM.SaveJob(MM.HouseBLItems[0].HouseBLId, MM.ModeofShipment);
                MM.UpdateJobSequence();
                //Skipping somestatus for Payment
                int ShipmentLastStatus = MySettings.GetShipmentLastStatus(MM.ShipmentId);
                if (ShipmentLastStatus >= 3)
                {
                    MM.SkipStatus(MM.HouseBLItems[0].HouseBLId);
                }
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Job has been created successfully!");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Manifest has NOT been Updated, please fill the required info!" +
                                "<br>* VoyageNo,<br>* Vessel,<br>* Mode of Shipment,<br>* Shipper" +
                                ",<br>* Customer,<br>* Container Type");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateJob(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.HouseBLItems.Count != 0)
            {
                /**Update Manifest**/
                if (MM.UpdateShipment())
                {
                    dictionary.Add("ShipmentStatus", "success");
                    dictionary.Add("ShipmentMessage", "Vessel info has been updated successfully!");
                }
                else {
                    dictionary.Add("ShipmentStatus", "error");
                    dictionary.Add("ShipmentMessage", "Vessel info update FAIL!");
                }
                /**update BL Item and BL status*/
                if (MM.UpdateHouseBLItems(MM.HouseBLItems, MM.ShipmentId))
                {
                    dictionary.Add("BOLStatus", "success");
                    dictionary.Add("BOLMessage", "BOL info has been updated successfully!");
                }
                else {
                    dictionary.Add("BOLStatus", "error");
                    dictionary.Add("BOLMessage", "BOL info update FAIL!");
                }
                if (MM.UpdateJob())
                {
                    dictionary.Add("JOBStatus", "success");
                    dictionary.Add("JOBMessage", "JOB info has been updated successfully!");
                }
                else { 
                    dictionary.Add("JOBStatus", "error");
                    dictionary.Add("JOBMessage", "JOB info updated FAIL!");
                }
                if (MM.UpdateClearance())
                {
                    if (MM.Clearance.ClearanceDate !=null)
                    {
                         JobStatusModel JSM = new JobStatusModel();
                         JSM.UpdateBLStatus(6, MM.HouseBLItems[0].HouseBLId);
                    }
                }
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Job has NOT been updated, please fill the required info!" +
                                "<br>* VoyageNo,<br>* Vessel,<br>* Mode of Shipment,<br>* Shipper" +
                                ",<br>* Customer,<br>* Container Type");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getLastJobInfo()
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.getLastJobInfo(ManifestModel.GetLastJobId());
            return Json(dictionary);
        }

        public JsonResult GetJobbyStatus(int StatusId)
        {
          //  if (StatusId == 5)
           // {
           //     return Json(ManifestModel.GetUnpaidJobs(), JsonRequestBehavior.AllowGet);
           // }
           // else
            //{
                return Json(ManifestModel.GetJobbyStatus(StatusId), JsonRequestBehavior.AllowGet);
           // }
        }
        public JsonResult GetNewJobs(int StatusId)
        {
            return Json(ManifestModel.GetNewJobs(StatusId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterJob(DateTime StartDate, DateTime EndDate)
        {
            return Json(ManifestModel.FilterJob(StartDate, EndDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterJobBy(string query)
        {
            return Json(ManifestModel.FilterJobBy(query), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDOpayRequestedJobs()
        {
            return Json(ManifestModel.GetDOpayRequestedJobs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDOPendingJobs()
        {
            return Json(ManifestModel.GetDOPendingJobs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDOCollectedJobs()
        {
            return Json(ManifestModel.GetDOCollectedJobs(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditJob(int JobId, int HBLId)
        {
            ManifestModel edit = new ManifestModel(JobId);
            edit.Job.JobId = JobId;
            edit.HBLId = HBLId;
            edit.Job.JobStatusModel.JobId = JobId;
            return View(edit);
        }

        public ActionResult AddNewJobUnderMBL(int ShipmentId)
        {
            ManifestModel MM = new ManifestModel();
            MM.ShipmentId = ShipmentId;
            return View(MM);
        }

        public JsonResult GetJob(int JBId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetJob(JBId);
            return Json(dictionary);
        }

        public ActionResult ConvertManifestJob(int ManifestId, int HBLId)
        {
            ManifestModel edit = new ManifestModel();
            edit.ManifestId = ManifestId;
            edit.HBLId = HBLId;
            return View(edit);
        }

        public ActionResult GetBLUnderMBL(int ShipmentId)
        {
            ManifestModel edit = new ManifestModel();
            edit.ShipmentId = ShipmentId;
            return View(edit);
        }

        public ActionResult EditJobStat(int job_StatId, string job_Remarks, int job_statValue, int JobId)
        {
            JobStatusModel JSM = new JobStatusModel();
            JSM.EditJobStat(job_StatId, job_Remarks, job_statValue, JobId);
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Updated successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPaymentStat(int PTypeId, string PDocumentNo, int HBLId, int PaymentStat)
        {
            JobStatusModel JSM = new JobStatusModel();
            JSM.EditPaymentStat(PTypeId, PDocumentNo, HBLId, PaymentStat);
            if (PaymentStat > 0)
            {
                if (PTypeId == 1)
                {
                    JSM.UpdateFreightInvoice(HBLId);
                }
                if (PTypeId == 2)
                {
                    JSM.UpdateClearanceInvoice(HBLId);
                }
            }
            else
            {
                if (PTypeId == 1)
                {
                    JSM.NullFreightInvoice(HBLId);
                }
                if (PTypeId == 2)
                {
                    JSM.NullClearanceInvoice(HBLId);
                }
            }
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Updated successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDelivery(string DeliveryDate, int NoOfCleared, int NoOfDelivered, int NoOfDamage, int JobId, int HBLId, int DeliveredBy, string ReceivedBy)
        {
            DateTime DDate = Convert.ToDateTime(DeliveryDate);

            JobStatusModel JSM = new JobStatusModel();
            if (JSM.UpdateDeliveryDate(DDate, NoOfCleared, NoOfDelivered,NoOfDamage, JobId, DeliveredBy, ReceivedBy))
            {
                JSM.UpdateBLStatus(7, HBLId);
            }
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Delivery Date Updated successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

       /* public ActionResult PrintDO(int JobId) //TO DO:
        {
            Dictionary<Object, Object> DOBinfo = ClearanceModel.GetDOInfo(JobId);
            int HBLid =Convert.ToInt32(DOBinfo["Id"]);
            Dictionary<object, object> ContainerList = HouseBLModel.GetContainerData4(HBLid);
            DateTime today = DateTime.Now;
            DOBinfo.Add("IssueDate", today);
            return File(Reports.Generate_Report(DOBinfo, Server.MapPath("~/Templates/DO_A5.doc")), "application/pdf", "Delivery Order (DO).pdf");
        }*/

        public ActionResult PrintDO(int JobId)
        {
            Dictionary<Object, Object> DOBinfo = ClearanceModel.GetDOInfo(JobId);
            int HBLid = Convert.ToInt32(DOBinfo["Id"]);
            List<Dictionary<object, object>> ContainerList = HouseBLModel.GetContainerData3(HBLid);
            DateTime today = DateTime.Now;
            DOBinfo.Add("IssueDate", today);
            return File(new Reports().Generate_GDN(ContainerList, DOBinfo, Server.MapPath(MySettings.GetReportPath() + "GDN_A4.doc")), "application/pdf", "Goods Delivery Note (GDN).pdf");
        }

        public ActionResult PrintDailyClearance(DateTime CLdate, int ModeofShipment)
        {
            MySettings.UpdateSequenceById(7);
            string Number = MySettings.FormatNumbering(7);
            string Dateonly = CLdate.ToString("yyyy-MM-dd");
            List<Dictionary<object, object>> DCUSList = ManifestModel.GetDailyClearance(Dateonly, ModeofShipment);
            Dictionary<Object, Object> DCBinfo = new Dictionary<object, object>();
            DCBinfo.Add("Printed", DateTime.Now.ToString("dd MMM yyyy"));
            DCBinfo.Add("ClearDate", Dateonly);
            DCBinfo.Add("Number", Number);
            int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
            if (EmployeeId > 0)
            {
                Dictionary<object, object> EmpDT = EmployeeModel.getEmployeeInfo(EmployeeId);
                DCBinfo["EmpName"] = Server.HtmlDecode(EmpDT["EmployeeName"].ToString());
                DCBinfo["EmpDesig"] = Server.HtmlDecode(EmpDT["Designation"].ToString());
            }
            string typeName = "";
            if (ModeofShipment == 1)
            {
                typeName = "SEA";
            }
            else
            {
                typeName = "AIR";
            }
            DCBinfo.Add("typeName", typeName);
            return File(new Reports().Generate_DailyClearanceSheet(DCUSList, DCBinfo, Server.MapPath(MySettings.GetReportPath() + "DCUS.doc")), "application/pdf", typeName + " Daily Clearance Update Sheet.pdf");
        }

        /* START Job Dash Board*/
        public JsonResult GetDashboardItem()
        {
            JobDashBoard dashboard = new JobDashBoard();
            return Json(dashboard.Getdashboarditem(), JsonRequestBehavior.AllowGet);
        }
        /* END Job Dash Board*/

    }
}
