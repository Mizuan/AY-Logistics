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
    public class JobDocumentController : Controller
    {

       /* public ActionResult Index(int? ShipmentId)
        {
            if (ShipmentId.HasValue)
            {
                ViewBag.ShipmentId = ShipmentId.Value;
            }
            return View(new JobDocumentVM());
        }*/

        public ActionResult Index(int? ShipmentId, int? HBLId)
        {
            if (ShipmentId.HasValue)
            {
                ViewBag.ShipmentId = ShipmentId.Value;
            }
            if (HBLId.HasValue)
            {
                ViewBag.specificBLid = HBLId.Value;
            }
            return View(new JobDocumentVM());
        }

        public ActionResult RawIndex(int? ShipmentId, int? HBLId)
        {
            if (ShipmentId.HasValue)
            {
                ViewBag.ShipmentId = ShipmentId.Value;
            }
            if (HBLId.HasValue)
            {
                ViewBag.specificBLid = HBLId.Value;
            }
            return View(new JobDocumentVM() {Name ="No Type" });
        }

        public ActionResult GetJobDocumentTypes()
        {
            return Json(JobDocumentVM.GetJobDocumentTypes(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLList(int shipmentId, int? HBLId)
        {
            return Json(JobDocumentVM.GetBLList(shipmentId, HBLId), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobDocumentsKO(int shipmentId, int? HBLId)
        {
            List<String> list = new List<string>();
            List<JobDocumentVM> cdvms = JobDocumentVM.GetJobDocuments(shipmentId, HBLId);
            foreach (JobDocumentVM cdvm in cdvms)
            {
                String s = "new JobDocument(" + cdvm.Id.ToString() + "," + cdvm.ShipmentId + ",\"" + cdvm.Name + "\"," + cdvm.DocumentType.ToString() + "," + cdvm.HouseBLId + ",\"" + cdvm.Description + "\")";
                list.Add(s);
            }
            return Content("[" + string.Join(",", list) + "]", "text/html");
        }

        [HttpPost]
        public ActionResult GetJobDocuments(int shipmentId, int? HBLId)
        {
            List<JobDocumentVM> cdvms = JobDocumentVM.GetJobDocuments(shipmentId, HBLId);
            return Json(cdvms);
        }

        public ActionResult GetRemovedDocumentsKO(int shipmentId, int? HBLId)
        {
            List<String> list = new List<string>();
            List<JobDocumentVM> cdvms = JobDocumentVM.GetRemovedDocuments(shipmentId, HBLId);
            foreach (JobDocumentVM cdvm in cdvms)
            {
                String s = "new JobDocument(" + cdvm.Id.ToString() + "," + cdvm.ShipmentId + ",\"" + cdvm.Name + "\"," + cdvm.DocumentType.ToString() + ",\"" + cdvm.Description + "\")";
                list.Add(s);
            }
            return Content("[" + string.Join(",", list) + "]", "text/html");
        }

        [HttpPost]
        public ActionResult AddDocument(JobDocumentVM model, HttpPostedFileBase file)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            if ((ModelState.IsValid) && (file != null))
            {
                String originalFilename = Path.GetFileName(file.FileName);
                String ext = originalFilename.Substring(originalFilename.LastIndexOf('.') + 1).ToLower();
                ValidFileType vft = new ValidFileType();
                    if (vft.IsSatisfiedBy(ext))
                    {
                        String filename = System.Guid.NewGuid().ToString() + "." + ext;
                        String savefilename = Path.Combine(Server.MapPath(MySettings.Root(model.ShipmentId, Server)), filename);
                        file.SaveAs(savefilename);
                        model.Filename = filename;
                        dictionary = model.AddDocument();
                        switch (model.DocumentType)
                        {
                            case 18: // D/O - Shipping Agent
                                  ManifestModel MF = new ManifestModel();
                                 // MF.SaveShipmentStatus(model.ShipmentId, 5);
                                  BLStatus.SaveBLStatus(model.specificBLid, 8);
                                break;
                            case 13: //Debit Note Forein Agent
                                ManifestModel MF1 = new ManifestModel();
                                MF1.SaveShipmentReview(model.ShipmentId, 7);
                                break;
                            case 19:// DO Release Note
                                BLStatus.UpdateBLStatus(model.specificBLid, 3);
                                break;
                            default:
                                //code
                                break;
                        }

                    }
                    else
                    {
                        dictionary.Add("Message", "Restricted File Type.");
                        dictionary.Add("Status", "error");
                    }

            }
            else
            {
                dictionary.Add("Message", "Fill the required info.");
                dictionary.Add("Status", "error");
            }
            return Json(dictionary);
        }


        [HttpPost]
        public ActionResult AddMultipleDocument(JobDocumentVM model, HttpPostedFileBase[] files)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();



                //if (ModelState.IsValid)
                if(model.ShipmentId > 0)
                {
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            String originalFilename = Path.GetFileName(file.FileName);
                            String ext = originalFilename.Substring(originalFilename.LastIndexOf('.') + 1).ToLower();
                            ValidFileType vft = new ValidFileType();
                            if (vft.IsSatisfiedBy(ext))
                            {
                                String filename = System.Guid.NewGuid().ToString() + "." + ext;
                                String savefilename = Path.Combine(Server.MapPath(MySettings.Root(model.ShipmentId, Server)), filename);
                                file.SaveAs(savefilename);
                                model.Filename = filename;
                                //dictionary = model.AddDocument();
                                model.OriginalFilename = originalFilename;
                                dictionary = model.AddDocument();
                                switch (model.DocumentType)
                                {
                                    case 18: // D/O - Shipping Agent
                                        ManifestModel MF = new ManifestModel();
                                        // MF.SaveShipmentStatus(model.ShipmentId, 5);
                                        BLStatus.SaveBLStatus(model.specificBLid, 8);
                                        break;
                                    case 13: //Debit Note Forein Agent
                                        ManifestModel MF1 = new ManifestModel();
                                        MF1.SaveShipmentReview(model.ShipmentId, 7);
                                        break;
                                    case 19:// DO Release Note
                                        BLStatus.UpdateBLStatus(model.specificBLid, 3);
                                        break;
                                    default:
                                        //code
                                        break;
                                }

                            }
                        }
                    }
                }
                else
                {
                    dictionary.Add("Message", "Fill the required info.");
                    dictionary.Add("Status", "error");
                }
            return Json(dictionary);
        }

       // public ActionResult SaveJobDocument(JobDocumentVM model)
        public ActionResult SaveJobDocument(JobDocument model)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            if (ModelState.IsValid)
            {
                dictionary = model.SaveDocument();
            }
            else
            {
                dictionary.Add("Message", "Fill the required info");
                dictionary.Add("Status", "error");
            }
            return Json(dictionary);
        }

        [HttpPost]
       // public ActionResult RemoveDocument(JobDocumentVM model)
        public ActionResult RemoveDocument(JobDocument model)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            if (ModelState.IsValid)
            {
                 dictionary = model.RemoveDocument();
            }
            else
            {
                dictionary.Add("Message", "Fill the required info");
                dictionary.Add("Status", "error");
            }
            return Json(dictionary);
        }

        [HttpPost]
       // public ActionResult RestoreDocument(JobDocumentVM model)
        public ActionResult RestoreDocument(JobDocument model)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            if (ModelState.IsValid)
            {
                dictionary = model.RestoreDocument();
            }
            else
            {
                dictionary.Add("Message", "Fill the required info");
                dictionary.Add("Status", "error");
            }
            return Json(dictionary);
        }
        [HttpPost]
        public ActionResult DownloadDocument(int shipmentId, int docId)
        {
            if (ModelState.IsValid)
            {
                String filename, name;
                if (JobDocumentVM.GetDocumentFilename(shipmentId, docId, out filename, out name))
                {
                    if ((String.IsNullOrWhiteSpace(filename)) || (String.IsNullOrWhiteSpace(name)))
                    {
                        return null;
                    }
                    //String fullPath = Path.Combine(Server.MapPath(Settings.GetDocumentsRoot(caseId)), filename);
                    String fullPath = Path.Combine(Server.MapPath(MySettings.Root(shipmentId, Server)), filename);
                    String ext = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();

                    return File(fullPath, MySettings.getContentType(ext), name + "." + ext);
                };
            }
            else
            {
            }
            return null;
        }

    }
}
