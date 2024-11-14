using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Models;
using MyReports;
using AYLogistics.Attributes;
using System.Web.Script.Serialization;
using System.Xml.Serialization; /*XML serialization*/
using System.Xml;
using System.Text;
using System.IO;
using System.Net.Mail; /*XML serialization*/

namespace AYLogistics.Controllers
{
    public class ManifestController : Controller
    {
        //
        // GET: /Manifest/

        public ActionResult Index()
        {
            
            return View();
        }

        //
        // GET: /Manifest/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manifest/Create

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult ManifestEntry()
        {
            return View();
        }

        public ActionResult PreManifest( int modofShipment =3)
        {
            ManifestModel MM = new ManifestModel();
            string ManifestNo = MM.PreSaveManifest();
            MM.ManifestId = ManifestModel.GetManifestId(ManifestNo);
            //MM.UpdateSequence();
            MySettings.UpdateSequenceById(1);
            ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
            return RedirectToAction("EditManifest", new { ManifestId = MM.ManifestId }).Success("Manifest " + ManifestNo + " created successfully, Please update the information");
        }

        public ActionResult AdvanceSearch()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Manifest/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manifest/Edit/5

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

        //
        // GET: /Manifest/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manifest/Delete/5

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

        public ActionResult GetVessel()
        {
            return Json(Vessel.GetVessel(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModeofShipment()
        {
            return Json(ModeofShipment.GetModeofShipment(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomeOffice()
        {
            return Json(CustomOffice.GetCustomOffice(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNationality()
        {
            return Json(Nationality.GetNationality(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPort()
        {
            return Json(Port.GetPort(), "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchPort(string query, int? type, int? subtype, int mode = 0)
        {
            return Json(AYLogistics.Models.Port.Search(query, type.Value, subtype.Value,mode), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLAllStatusList()
        {
            return Json(BLStatus.GetBLAllStatusList(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContainerType()
        {
            return Json(ContainerType.GetContainerType(), "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetContainerIndicatorType()
        {
            return Json(ContainerType.GetContainerIndicatorType(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTypeofPackage()
        {
            return Json(TypeofPackage.GetTypeofPackage(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPacking()
        {
            return Json(TypeofPacking.GetTypeofPacking(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFreightIndicator()
        {
            return Json(FreightIndicator.GetFreightIndicator(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLNature()
        {
            return Json(BLNature.GetBLNature(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLType()
        {
            return Json(BLType.GetBLTypes(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLState()
        {
            return Json(BLType.GetBLStates(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParty()
        {
            return Json(Party.GetParty(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPartyRuntime()
        {
            return Json(new SelectList(Party.GetPartyIQ(), "PartyId", "PartyName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateHouseBLnumber(int DOAId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.FormatHBLNumber(DOAId);
            //ManifestModel.UpdateHBLSequence();
            MySettings.UpdateSequenceById(6);
            return Json(dictionary);
        }

        [HttpPost]
        public ActionResult SaveManifest(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.VesselId.id != 0 && MM.HouseBLItems.Count != 0)
            {
                /**add Manifest**/
                MM.SaveManifest();
                int LastShipmentId = ManifestModel.GetLastShipmentId();
                /**add BL Item and BL status*/
                MM.SaveHouseBLItems(MM.HouseBLItems, LastShipmentId);
                //MM.UpdateSequence();
                MySettings.UpdateSequenceById(1);
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Manifest has been created successfully!");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Manifest has NOT been created, please fill the required info!" +
                                "<br>* VoyageNo,<br>* Vessel,<br>* Mode of Shipment,<br>* Shipper" +
                                ",<br>* Customer,<br>* Container Type");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateManifest(ManifestModel MM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (MM.VesselId.id != 0 && MM.HouseBLItems.Count != 0)
            {
                /**Update Manifest**/
                if (MM.UpdateManifest())
                {
                    dictionary.Add("ShipmentStatus", "success");
                    dictionary.Add("ShipmentMessage", "Shipment info has been updated successfully!");
                }
                else
                {
                    dictionary.Add("ShipmentStatus", "error");
                    dictionary.Add("ShipmentMessage", "Shipment info update FAIL!");
                }
                /**Update BL Item and status*/
                if (MM.UpdateHouseBLItems(MM.HouseBLItems, MM.ShipmentId))
                {
                    dictionary.Add("BOLStatus", "success");
                    dictionary.Add("BOLMessage", "BOL has been updated successfully!");
                }
                else
                {
                    dictionary.Add("BOLStatus", "error");
                    dictionary.Add("BOLMessage", "BOL info update FAIL!");
                }
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

        public JsonResult getLastManifestInfo()
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.getLastManifestInfo(ManifestModel.GetLastShipmentId());
            return Json(dictionary);
        }

        public JsonResult GetBLbyStatus(int StatusId)
        {
            return Json(ManifestModel.GetBLbyStatus(StatusId), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult FilterManifest(DateTime StartDate, DateTime EndDate)
        {
            return Json(ManifestModel.FilterManifest(StartDate, EndDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterManifestBy(string query)
        {
            return Json(ManifestModel.FilterManifestBy(query), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShipmentByStatus(int StatusId)
        {
            return Json(ManifestModel.GetShipmentByStatus(StatusId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShipmentBy(int SPstatus, int BLstatus)
        {
            return Json(ManifestModel.GetShipmentByStatus(SPstatus, BLstatus), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditManifest(int ManifestId)
        {
            ManifestModel edit = new ManifestModel();
            edit.ManifestId = ManifestId;
            return View(edit);
        }


        public ActionResult ViewManifestDetail(int ManifestId)
        {
            ManifestModel detail = new ManifestModel();
            detail.ManifestId = ManifestId;
            return View(detail);
        }

        public JsonResult GetManifest(int MfId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetManifest(MfId);
            return Json(dictionary);
        }

        public JsonResult GetShipment(int SPId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetShipment(SPId);
            return Json(dictionary);
        }

        public JsonResult GetMasterBL(string MasterBLnumber)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetMasterBL(MasterBLnumber);
            return Json(dictionary);
        }

        public JsonResult GetHouseBL(string HouseBLnumber)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetHouseBL(HouseBLnumber);
            return Json(dictionary);
        }

        public JsonResult GetHouseBLinfo(int HBLId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = ManifestModel.GetHouseBLinfo(HBLId);
            return Json(dictionary);
        }

        public ActionResult ConvertJobToManifest(int ShipmentId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            if (ManifestModel.ConvertJobToManifest(ShipmentId))
            {
                ManifestModel MM = new ManifestModel();
                //  MM.UpdateShipmentStatusNEW(ShipmentId, 1);
                //MM.UpdateSequence();
                MySettings.UpdateSequenceById(1);
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Manifest has been created successfully!");
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Manifest created FAIL!");
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MoveBL(int BLId, int CurrShipmentId, int NewShipmentId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            if (ManifestModel.MoveTheBL(BLId, CurrShipmentId, NewShipmentId))
            {
                ManifestModel MM = new ManifestModel();
                dictionary.Add("Status", "success");
                dictionary.Add("Message", "Manifest has been created successfully!");
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Manifest created FAIL!");
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetHouseBLItems(int SPId, int HBLId = 0)
        {
            var serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = HouseBLModel.GetHouseBLItems(SPId, HBLId);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult GetHouseBLItem(int SPId, string HBLno)
        {
            var serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = HouseBLModel.GetHouseBLItem(SPId, HBLno);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult GetContainerItems(int HBLId)
        {
            var serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = HouseBLModel.GetContainerItems(HBLId);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult DeleteHBLitem(int HBLId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            HouseBLModel.DeleteHBLitem(HBLId);
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "BL has been deleted successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteContaineritem(int ContainerId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            HouseBLModel.DeleteContainerItem(ContainerId);
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Container has been deleted successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

       /* public ActionResult UpdateShipmentStatus(int ShipmentId, int ShipmentStatusId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            ManifestModel MM = new ManifestModel();
            MM.SaveShipmentStatus(ShipmentId, ShipmentStatusId);
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Manifest Status has been updated successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }*/

        public ActionResult PrintManifest(int ManifestId)
        {
            Dictionary<object, object> ManifestDT = ManifestModel.GetManifest(ManifestId);
            int ShipmentId = Convert.ToInt32(ManifestDT["ShipmentId"]);
           // string MasterBLNo = Convert.ToString(ManifestDT["MasterBL"]);
            List<Dictionary<object, object>> BLList = HouseBLModel.GetHouseBLItems(ShipmentId);
            ManifestDT.Add("CurrentDate", DateTime.Now.ToString("dd MMM yyyy"));

            ManifestDT["TotalNoOfBL"] = BLList.Count;
            int TotalNoOfPackages = 0;
            foreach (Dictionary<object, object> dictionary in BLList)
            {
                int NoOfPackage = dictionary["NoOfPackage"] == DBNull.Value ? 0 : Convert.ToInt32(dictionary["NoOfPackage"]);
                TotalNoOfPackages = TotalNoOfPackages + NoOfPackage;
                ManifestDT["TotalNoOfPackages"] = TotalNoOfPackages;
                
            }

            int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
            if (EmployeeId > 0)
            {
                Dictionary<object, object> EmpDT = EmployeeModel.getEmployeeInfo(EmployeeId);
                ManifestDT["EmployeeName"] = Server.HtmlDecode(EmpDT["EmployeeName"].ToString());
                ManifestDT["EmpDesignation"] = Server.HtmlDecode(EmpDT["Designation"].ToString());
            }
            return File(new Reports().Generate_Manifest(BLList, ManifestDT, Server.MapPath(MySettings.GetReportPath() + "Manifest_Template.docx")), "application/pdf", "Manifest.pdf");
        }

        public ActionResult PrintDispatchOrderSheet(int ManifestId)
        {
            Dictionary<object, object> ManifestDT = ManifestModel.GetManifest(ManifestId);
            int ShipmentId = Convert.ToInt32(ManifestDT["ShipmentId"]);
            // string MasterBLNo = Convert.ToString(ManifestDT["MasterBL"]);
            List<Dictionary<object, object>> BLList = HouseBLModel.GetHouseBLItems(ShipmentId);
            ManifestDT.Add("CurrentDate", DateTime.Now.ToString("dd MMM yyyy"));

            string HBLCount = BLList.Count.ToString();
            ManifestDT["TotalNoOfBL"] = HBLCount;
            int TotalNoOfPackages = 0;
            foreach (Dictionary<object, object> dictionary in BLList)
            {
                int NoOfPackage = dictionary["NoOfPackage"] == DBNull.Value ? 0 : Convert.ToInt32(dictionary["NoOfPackage"]);
                TotalNoOfPackages = TotalNoOfPackages + NoOfPackage;
                ManifestDT["TotalNoOfPackages"] = TotalNoOfPackages;

            }

            int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
            if (EmployeeId > 0)
            {
                Dictionary<object, object> EmpDT = EmployeeModel.getEmployeeInfo(EmployeeId);
                ManifestDT["EmployeeName"] = Server.HtmlDecode(EmpDT["EmployeeName"].ToString());
                ManifestDT["EmpDesignation"] = Server.HtmlDecode(EmpDT["Designation"].ToString());
            }
            return File(new Reports().Generate_Manifest_Dispatch(BLList, ManifestDT, HBLCount, Server.MapPath(MySettings.GetReportPath() + "Dispatch_Order_Sheet.docx")), "application/pdf", "Dispatch Order Sheet.pdf");
        }

        public ActionResult PrintBL(int HouseBLId, int ModeofShipment, int DeliveryAgent = 0)
        {
            if (ModeofShipment == 2)
            {
                Dictionary<object, object> BOL = ManifestModel.GETAWBL(HouseBLId);
                Dictionary<object, object> ContainerList = HouseBLModel.GetContainerData4(HouseBLId);
                BOL.Add("CNoOfPackage", ContainerList["CNoOfPackage"]);
                BOL.Add("TypeofPackage", ContainerList["TypeofPackage"]);
                return File(Reports.Generate_Report(BOL, Server.MapPath(MySettings.GetReportPath() + "AWBL.doc"),false,3), "application/pdf", "Bill of Lading (AWBL).pdf");
            }
            else
            {
                Dictionary<object, object> BOL = ManifestModel.GETHBL(HouseBLId);
                List<Dictionary<object, object>> ContainerList = HouseBLModel.GetContainerData3(HouseBLId);
                BOL.Add("NumberOfContainer", Convert.ToString(ContainerList.Count));
                //Maldives Shipping and Logistics
                if (DeliveryAgent == 7413)
                {
                    return File(new Reports().Generate_SEABL(ContainerList, BOL, Server.MapPath(MySettings.GetReportPath() + "SEABL_FooterRepeat_MSL.doc")), "application/pdf", "Bill of Lading (SHBL).pdf");
                }
                //Asia Shipping and Logistics
                else if (DeliveryAgent == 5749)
                {
                    return File(new Reports().Generate_SEABL(ContainerList, BOL, Server.MapPath(MySettings.GetReportPath() + "SEABL_FooterRepeat_ASL.doc")), "application/pdf", "Bill of Lading (SHBL).pdf");
                }
                //Asia Forwarding and Logistics
                else if (DeliveryAgent == 1017)
                {
                    return File(new Reports().Generate_SEABL(ContainerList, BOL, Server.MapPath(MySettings.GetReportPath() + "SEABL_FooterRepeat_AFL.doc")), "application/pdf", "Bill of Lading (SHBL).pdf");
                }
                else
                {
                    return File(new Reports().Generate_SEABL(ContainerList, BOL, Server.MapPath(MySettings.GetReportPath() + "SEABL_FooterRepeat.doc")), "application/pdf", "Bill of Lading (SHBL).pdf");
                }
            }
        }

        public ActionResult PrintMultipleBL(int ShipmentId, int ModeofShipment)
        {
            List<Dictionary<object, object>> HBLIdList = ManifestModel.GetHBLIdList(ShipmentId);
            List<Dictionary<object, object>> BOLlist = new List<Dictionary<object, object>>();
            if (ModeofShipment == 2)
            {
                foreach (Dictionary<object, object> dictionary in HBLIdList)
                {
                    Dictionary<object, object> BOL = ManifestModel.GETAWBL(Convert.ToInt32(dictionary["HBLid"])); //TO DO:
                    BOLlist.Add(BOL);
                }
                ManifestModel MF = new ManifestModel();
                MF.SaveShipmentStatus(ShipmentId, 2);
                return File(Reports.GenerateMultipleReport(BOLlist, Server.MapPath(MySettings.GetReportPath() + "AWBL.doc")), "application/pdf", "Bill of Lading (AWBL).pdf");
            }
            else
            {
                foreach (Dictionary<object, object> dictionary in HBLIdList)
                {
                    Dictionary<object, object> BOL = ManifestModel.GETHBL(Convert.ToInt32(dictionary["HBLid"])); //TO DO:
                    BOLlist.Add(BOL);
                }
                return File(Reports.GenerateMultipleReport(BOLlist, Server.MapPath(MySettings.GetReportPath() + "SEABL_FooterRepeat.doc")), "application/pdf", "Bill of Lading (SHBL).pdf");
            }
        }

        public void SendToJob(int HBLid, int StatusId)
        {
            if (BLStatus.SaveBLStatus(HBLid,StatusId))
            {
                ViewBag.message = "Insert Success";
            }
            else
            {
                Response.Write("error");
                Response.End();
            }
        }

        public void DORelease(int HBLid, int StatusId, string DOcollectedBy, string DOcollectedContact, DateTime? DOcollectedDate, int sendEmail)
        {
            if (sendEmail == 1)
            {
                //Get Customer Email
                int CustomerId = MySettings.GetCustomerId(HBLid);
                string CustomerEmail = MySettings.GetPartyEmail(CustomerId);

                if (CustomerEmail == "")
                {
                    Response.Write("error");
                    Response.End();
                }
                else
                {
                    int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
                    /******Get Attachement Start*******/
                    String filename, name;
                    Attachment attachment = null;
                    MailMessage mailMessage = new MailMessage();
                    if (JobDocumentVM.GetDocumentFilePath(HBLid, 1, out filename, out name)) // Attach E-OD
                    {
                        int ShipmentId = MySettings.GetShipmentId(HBLid);
                        attachment = new Attachment(Path.Combine(Server.MapPath(MySettings.Root(ShipmentId, Server)), filename));
                        String ext = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
                        attachment.ContentDisposition.FileName = name + "." + ext;
                        mailMessage.Attachments.Add(attachment);
                    }
                    string result = "";
                    if (mailMessage.Attachments.Count > 0)
                    {
                        result = EmailFormModel.SendEmailwithAttachment(MySettings.PODTemplate(HBLid, EmployeeId), CustomerEmail, "DOCUMENT RELEASE", mailMessage);
                    }
                    else
                    {
                        result = "Please attach E-OD";
                    }
                    /******Get Attachement End******/
                   // string result = EmailFormModel.SendEmail(MySettings.PODTemplate(HBLid, EmployeeId), CustomerEmail, "DOCUMENT RELEASE");
                    if (result == "True")
                    {
                        BLStatus.SaveBLStatus(HBLid, StatusId, DOcollectedBy, DOcollectedContact, DOcollectedDate);
                        ViewBag.message = "Insert Success";
                    }
                    else
                    {
                        Response.Write("error");
                        Response.End();
                    }
                }
            }
            else
            {
                 if (BLStatus.SaveBLStatus(HBLid, StatusId, DOcollectedBy, DOcollectedContact, DOcollectedDate))
                 {
                     ViewBag.message = "Insert Success";
                 }
                 else
                 {
                     Response.Write("error");
                     Response.End();
                 }
            }
        }

        public ActionResult PrintDOReleaseNote(int HouseBLId)
        {
            Dictionary<object, object> PODinfo = ManifestModel.GetPODinfo(HouseBLId);
                return File(Reports.Generate_Report(PODinfo, Server.MapPath("~/Templates/POD.doc")), "application/pdf", "POD-Proof Of Delivery.pdf");
        }

        public ActionResult SendArrivalNotice(int HBLId, int ModeofShipment, int BLNature)
        {
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            string BLN = "";
            if (BLNature == 1)
            {
                BLN = "Export";
            }
            if (BLNature == 2)
            {
                BLN = "Import";
            }
            if (BLNature == 3)
            {
                BLN = "In-Transit";
            }
            if (BLNature == 4)
            {
                BLN = "Transshipment";
            }
            string ShipmentType = "";
            if (ModeofShipment == 1)
            {
                ShipmentType = " - SEA " + BLN;
            }
            else
            {
                ShipmentType = " - AIR " + BLN;
            }
            //Get Customer Email
            int CustomerId = MySettings.GetCustomerId(HBLId);
            string CustomerEmail = MySettings.GetPartyEmail(CustomerId);

            if (CustomerEmail == "")
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Please Enter the Customer Email Address!");
            }
            else
            {
                int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
                /******Get Attachement Start*******/
                String filename, name;
                Attachment attachment = null;
                MailMessage mailMessage = new MailMessage();
                int ShipmentId = MySettings.GetShipmentId(HBLId);
                if (JobDocumentVM.GetDocumentFilePath(HBLId, 15, out filename, out name)) // BL
                {
                    attachment = new Attachment(Path.Combine(Server.MapPath(MySettings.Root(ShipmentId, Server)), filename));
                    String ext = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
                    attachment.ContentDisposition.FileName = name + "." + ext;
                    mailMessage.Attachments.Add(attachment);
                }
                if (JobDocumentVM.GetDocumentFilePath(HBLId, 23, out filename, out name)) // Freight Invoice
                {
                    attachment = new Attachment(Path.Combine(Server.MapPath(MySettings.Root(ShipmentId, Server)), filename));
                    String ext = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
                    attachment.ContentDisposition.FileName = name + "." + ext;
                    mailMessage.Attachments.Add(attachment);
                }
                string result = "";
                if (mailMessage.Attachments.Count > 1) // if 2 documents are there
                {
                    result = EmailFormModel.SendEmailwithAttachment(MySettings.ArrivalNoticeTemplate(HBLId, ShipmentType, EmployeeId), CustomerEmail, "ARRIVAL NOTICE" + ShipmentType, mailMessage);
                }
                else
                {
                    result = "Please attach HBL and ASF-Freight Invoice";
                }
                /******Get Attachement End******/
              //  string result = EmailFormModel.SendEmail(MySettings.ArrivalNoticeTemplate(HBLId, ShipmentType, EmployeeId), CustomerEmail, "ARRIVAL NOTICE" + ShipmentType);
                if (result == "True")
                {
                    ManifestModel MFM = new ManifestModel();
                    MFM.SaveBLstatusReview(HBLId, 9);
                    dictionary.Add("Status", "success");
                    dictionary.Add("Message", "Arrival Notice sent successfully!");
                }
                else
                {
                    dictionary.Add("Status", "error");
                    dictionary.Add("Message", result);
                }
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestDebitNote(int SPId, int ModeofShipment)
        {
            Dictionary<object, object> dictionary = new Dictionary<object, object>();

            //Get Customer Email
            int ShippingAgentId = MySettings.GetShippingAgentId(SPId);
            string ShippingAgentEmail = MySettings.GetPartyEmail(ShippingAgentId);

            if (ShippingAgentEmail == "")
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Please Enter the Shipping Agent Email Address!");
            }
            else
            {
                int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
                string result = EmailFormModel.SendEmail(MySettings.DebitNoteTemplate(SPId, EmployeeId), ShippingAgentEmail, "REQUEST FOR DEBIT NOTE / FREIGHT INVOICE");
                if (result == "True")
                {
                    ManifestModel MFM = new ManifestModel();
                    MFM.SaveShipmentReview(SPId, 6);
                    dictionary.Add("Status", "success");
                    dictionary.Add("Message", "Debit Note Request email has been sent successfully!");
                }
                else
                {
                    dictionary.Add("Status", "error");
                    dictionary.Add("Message", result);
                }
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }
        
        /* START Manifest Dash Board*/
        public JsonResult GetDashboardItem()
        {
            ManifestDashBoard dashboard = new ManifestDashBoard();

            return Json(dashboard.Getdashboarditem(), JsonRequestBehavior.AllowGet);
        }
        /* END Manifest Dash Board*/

        public ActionResult shipmentStatus(int? ShipmentId, int? HBLId)
        {
            ShipmentStatusVM model = new ShipmentStatusVM(ShipmentId, HBLId);
            if (ShipmentId.HasValue)
            {
                ViewBag.ShipmentId = ShipmentId.Value;
            }
            if (HBLId.HasValue)
            {
                ViewBag.HouseBLId = HBLId;
            }
            else
            {
                ViewBag.HouseBLId = 0;
            }
            return View();
        }
    }
}
