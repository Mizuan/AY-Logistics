using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AYLogistics.Models;
using MyReports;
using AYLogistics.Attributes;

namespace AYLogistics.Controllers
{
    public class SalesController : Controller
    {
        //
        // GET: /Sales/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
       // public ActionResult AddSalesCategory(SalesCategory model)
        public ActionResult AddSalesCategory(string SalesCatName)
        {
            SalesCategory model = new SalesCategory();
            model.SalesCatName = SalesCatName;
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            if (ModelState.IsValid)
            {
                if (model.AddCategory())
                {
                    ModelState.Clear();
                   // ViewBag.type = "alert-success";
                    //return View("Index").Success("Category has been added successfully");
                    return Json(dictionary, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ViewBag.type = "alert-error";
                   // return View("Index").Success("Category not added, please try again!");
                    return Json(dictionary, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
               // ViewBag.type = "alert-error";
               // return View("Index").Success("Category Field cannot be NULL!");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCategories()
        {
            return Json(SalesCategory.GetCategories(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public string UpdateCategoryStatus(int CATid, int StatusId)
        {
            int action = 0;
            if (StatusId == 1)
            {
                action = 0;
            }
            if (StatusId == 0)
            {
                action = 1;
            }
            SalesCategory.UptadeCatStatus(CATid, action);
            return "true";
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public string UpdateItemStatus(int ITEMid, int StatusId)
        {
            int action = 0;
            if (StatusId == 1)
            {
                action = 0;
            }
            if (StatusId == 0)
            {
                action = 1;
            }
            SalesCategory.UptadeItemStatus(ITEMid, action);
            return "true";
        }

        [HttpGet]
        public ActionResult SearchCategories(string query, int? type, int? subtype)
        {
            return Json(AYLogistics.Models.SalesCategory.Search(query, type.Value, subtype.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriesKO()
        {
            return Json(SalesCategory.GetCategoriesKO(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriesKO_ALL()
        {
            return Json(SalesCategory.GetCategoriesKO_ALL(), "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSalesUnit(SalesUnit model)
        {
            if (ModelState.IsValid)
            {
                if (model.AddUnit())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Unit has been added successfully");
                }
                else
                {
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Unit not added, please try again!");
                }
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Index").Success("Unit Field cannot be NULL!");
            }
        }

        public JsonResult GetUnits()
        {
            return Json(SalesUnit.GetUnits(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSalesDiscount(SalesDiscount model)
        {
            if (ModelState.IsValid)
            {
                if (model.AddDiscount())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Discount has been added successfully");
                }
                else
                {
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Discount not added, please try again!");
                }
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Index").Success("Discount Field cannot be NULL!");
            }
        }

        public JsonResult GetDiscounts()
        {
            return Json(SalesDiscount.GetDiscounts(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSalesCurrency(SalesCurrency model)
        {
            if (ModelState.IsValid)
            {
                if (model.AddCurrency())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Currency has been added successfully");
                }
                else
                {
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Currency not added, please try again!");
                }
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Index").Success("Currency Field cannot be NULL!");
            }
        }

        public JsonResult GetCurrencies()
        {
            return Json(SalesCurrency.GetCurrencies(), JsonRequestBehavior.AllowGet);
        }



        public ActionResult UpdateCurrency(int Id)
        {
            SalesCurrency edit = new SalesCurrency(Id);
            return View(edit);
        }

        [HttpPost]
        public ActionResult UpdateCurrency(SalesCurrency model)
        {
            if (ModelState.IsValid)
            {
                if (model.ModifyCurrency())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Currency has been Updated successfully");
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Currency not Updated, please try again!");
                }
            }
            else
            {
                ModelState.Clear();
                ViewBag.type = "alert-error";
                return View("Index").Success("Currency Field cannot be NULL!");
            }
        }





        [HttpPost]
        public ActionResult AddSalesItem(SalesItems model)
        {
            if (ModelState.IsValid)
            {
                if (model.AddItems())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Item has been added successfully");
                }
                else
                {
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Item not added, please try again!");
                }
            }
            else
            {
                ViewBag.type = "alert-error";
                return View("Index").Success("Item Field cannot be NULL!");
            }
        }

        public JsonResult GetSalesItems()
        {
            return Json(SalesItems.GetSalesItems(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSalesItem(int Id)
        {
            SalesItems edit = new SalesItems(Id);
            return View(edit);
        }

        [HttpPost]
        public ActionResult UpdateSalesItem(SalesItems model)
        {
            if (ModelState.IsValid)
            {
                if (model.ModifyItems())
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-success";
                    return View("Index").Success("Item has been Updated successfully");
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.type = "alert-error";
                    return View("Index").Success("Item not Updated, please try again!");
                }
            }
            else
            {
                ModelState.Clear();
                ViewBag.type = "alert-error";
                return View("Index").Success("Item Field cannot be NULL!");
            }
        }

        public ActionResult NewQuotation()
        {
            return View();
        }

        public ActionResult SearchQuotation()
        {
            return View();
        } 

        [HttpGet]
        public ActionResult SearchSalesItem(string query, int? type, int? subtype, int?SCATid)
        {
            return Json(AYLogistics.Models.SalesItems.Search(query, type.Value, subtype.Value, SCATid.Value), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalesItemKO(int? currentCatID)
        {
            return Json(SalesItems.GetSalesItemKO(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesItemKO_ALL(int? currentCatID)
        {
            return Json(SalesItems.GetSalesItemKO_ALL(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemRates(int ItemId)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = SalesItems.GetItemsRates(ItemId);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult SaveQuotaion(SalesModel SM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (SM.PartyId.id != 0 && SM.SalesCategoryItems.Count > 0 && SM.Type !="")
            {
                /**Quotation**/
                if (SM.SaveQuotation())
                {
                   int LastQuotationId = SalesModel.GetLastQuotationId();
                    /**Category and Items*/
                   SM.SaveSalesSelectedItems(SM.SalesCategoryItems, LastQuotationId);
                    //SM.UpdateSequence();
                   MySettings.UpdateSequenceById(5);
                    SM.SaveQuotationLog(LastQuotationId);
                    dictionary.Add("Status", "success");
                    dictionary.Add("Message", "Quotation has been created successfully!");
                }
                else
                {
                    dictionary.Add("Status", "error");
                    dictionary.Add("Message", "Quotation Fail!");
                }
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Quotation has NOT been created, please fill the required info!" +
                                "<br>* Party,<br>* Category info,<br>* Item info");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getLastQuotationInfo()
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            int Id = SalesModel.GetLastQuotationId();
            dictionary = SalesModel.getLastQuotationInfo(Id);
            return Json(dictionary);
        }

        public JsonResult QuotationFilterByDate(DateTime StartDate, DateTime EndDate)
        {
            return Json(SalesModel.QuotationFilterByDate(StartDate, EndDate), JsonRequestBehavior.AllowGet);
        }
        public JsonResult QuotationFilterByQuery(string query)
        {
            return Json(SalesModel.QuotationFilterByQuery(query), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateQuotation(int Id)
        {
            SalesModel edit = new SalesModel(Id);
            return View(edit);
        }

        [HttpPost]
        public ActionResult UpdateQuotaion(SalesModel SM)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (SM.PartyId.id != 0 && SM.SalesCategoryItems.Count > 0 && SM.Type != "")
            {
                if (SM.UpdateQuotation())
                {
                    SM.UpdateSalesSelectedItems(SM.SalesCategoryItems, SM.QuotationId);
                    SM.SaveQuotationLog(SM.QuotationId);
                    dictionary.Add("Status", "success");
                    dictionary.Add("Message", "Quotation has been created successfully!");
                }
                else
                {
                    dictionary.Add("Status", "error");
                    dictionary.Add("Message", "Quotation update Fail!");
                }
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dictionary.Add("Status", "error");
                dictionary.Add("Message", "Quotation has NOT been created, please fill the required info!" +
                                "<br>* Party,<br>* Category info,<br>* Item info");
                return Json(dictionary, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetQT(int QuotId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = SalesModel.GetQT(QuotId);
            return Json(dictionary);
        }
        public ActionResult GetCAT(int QuotId)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = SalesModel.GetCAT(QuotId);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult GetSITEMS(int SCATid)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = SalesModel.GetSITEMS(SCATid);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult GetKOCurrencies()
        {
            return Json(SalesCurrency.GetKOCurrencies(), "text/html", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getExchangeRate()
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            dictionary = SalesCurrency.getExchangeRate();
            return Json(dictionary);
        }

        public ActionResult PrintQuotation(int QuotationId)
        {
            Dictionary<object, object> QuotBasicInfo = SalesModel.GetQuotationBasicInfo(QuotationId);
            Dictionary<object, object> calculations = SalesModel.GetTotalCalculation(QuotationId);
            List<Dictionary<object, object>> CATList = SalesModel.GetCAT(QuotationId);
            List<Dictionary<object, object>> PrintList = new List<Dictionary<object, object>>();
           // QuotBasicInfo.Add("CurrentDate", DateTime.Now.ToString("dd MMM yyyy"));
            QuotBasicInfo.Add("CurrentDate", DateTime.Now.ToString("dd MMM yyyy"));
            QuotBasicInfo.Add("SubTotal", Convert.ToString(calculations["SubTotal"]));
            QuotBasicInfo.Add("GST", Convert.ToString(calculations["GST"]));
            QuotBasicInfo.Add("NetTotal", Convert.ToString(calculations["NetTotal"]));
           /* decimal NetTotal = Convert.ToDecimal(calculations["NetTotal"]);
            decimal DollarRate = MySettings.GetMVRExchangeRate();
            decimal MVR = Decimal.Round(NetTotal * DollarRate,2);
            QuotBasicInfo.Add("MVR","MVR " + MVR);*/
            int CurrencyId = Convert.ToInt32(QuotBasicInfo["CurrencyId"]);
            if (CurrencyId == 1) // USD
            {
                QuotBasicInfo.Add("CN","USD" );
            }
            if (CurrencyId == 2) // MVR
            {
                QuotBasicInfo.Add("CN", "MVR");
            }
            decimal DollarRate = MySettings.GetMVRExchangeRate();

            foreach (Dictionary<object, object> dictionary in CATList)
            {
                Dictionary<object, object> CAT = new Dictionary<object, object>();
                int selectedCATid = Convert.ToInt32(dictionary["Id"]);
                List<Dictionary<object, object>> ITEMS = SalesModel.GetSITEMS(selectedCATid);
                CAT.Add("SalesCATName", Convert.ToString(dictionary["SalesCATName"]));
                CAT.Add("UnitName", "");
                CAT.Add("Quantity", "");
                CAT.Add("UnitPrice", "");
                CAT.Add("ItemTotal", "");
                string SalesCATName = "";
                string UnitName = "";
                string Quantity = "";
                string UnitPrice = "";
                string ItemTotal = "";
                Dictionary<object, object> ARRANGE = new Dictionary<object, object>();

                foreach (string key in CAT.Keys)
                {
                    if (key.Equals("SalesCATName"))
                    {
                        SalesCATName += "<strong>"+dictionary[key].ToString() + "</strong><br>";
                    }
                    if (key.Equals("UnitName"))
                    {
                        UnitName += CAT[key].ToString() + "<br>";
                    }
                    if (key.Equals("Quantity"))
                    {
                        Quantity += CAT[key].ToString() + "<br>";
                    }
                    if (key.Equals("UnitPrice"))
                    {
                        UnitPrice += CAT[key].ToString() + "<br>";
                    }
                    if (key.Equals("ItemTotal"))
                    {
                        ItemTotal += CAT[key].ToString() + "<br>";
                    }
                }
                foreach (Dictionary<object, object> item in ITEMS)
                {
                        int QTY = 0;
                        decimal UP = new decimal();
                        foreach (string key1 in item.Keys)
                        {
                            if (key1.Equals("SalesItemName"))
                            {
                                SalesCATName += item[key1].ToString()+"<br>";
                            }
                            if (key1.Equals("UnitName"))
                            {
                                UnitName += item[key1].ToString() + "<br>";
                            }
                            if (key1.Equals("Quantity"))
                            {
                                Quantity += item[key1].ToString() + "<br>";
                                QTY = Convert.ToInt32(item[key1]);
                            }
                            if (key1.Equals("UnitPrice"))
                            {
                                if (CurrencyId == 2) //USD to MVR
                                {
                                    decimal TempUP = Convert.ToDecimal(item[key1].ToString());
                                   // TempUP = TempUP * DollarRate;
                                    TempUP = Decimal.Round(TempUP * DollarRate, 2);
                                    UnitPrice += TempUP + "<br>";
                                    UP = TempUP;
                                }
                                else
                                {
                                    UnitPrice += item[key1].ToString() + "<br>";
                                    UP = Convert.ToDecimal(item[key1]);
                                }
                            }
                            if (key1.Equals("CurrencyName")) // ItemTotal
                            {
                                ItemTotal += QTY * UP + "<br>";
                            }
                        }
                }
                ARRANGE.Add("SalesCATName", SalesCATName);
                ARRANGE.Add("UnitName", UnitName);
                ARRANGE.Add("Quantity", Quantity);
                ARRANGE.Add("UnitPrice", UnitPrice);
                ARRANGE.Add("ItemTotal", ItemTotal);
                PrintList.Add(ARRANGE);
            }

            int EmployeeId = Profile.GetProfile(HttpContext.User.Identity.Name).ACSUserId;
            if (EmployeeId > 0)
            {
                Dictionary<object, object> EmpDT = EmployeeModel.getEmployeeInfo(EmployeeId);
                QuotBasicInfo["EmployeeName"] = Server.HtmlDecode(EmpDT["EmployeeName"].ToString());
                QuotBasicInfo["EmpDesignation"] = Server.HtmlDecode(EmpDT["Designation"].ToString());
            }
            return File(new Reports().Generate_Quotation(PrintList, QuotBasicInfo, Server.MapPath(MySettings.GetReportPath() + "QuotationDOC.doc")), "application/pdf", "Quotation.pdf");
        }

        public ActionResult DeleteCATblock(int SelectedCATid)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();

             if (SalesCategory.DeleteCAT(SelectedCATid))
             {
                 SalesItems.DeleteITEM(SelectedCATid);
             }
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Category and its Items has been removed successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteITEMonly(int ItemPriId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            SalesItems.DeleteSpecificITEM(ItemPriId);
            dictionary.Add("Status", "success");
            dictionary.Add("Message", "Item has been removed successfully!");
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUnitPrice( decimal UnitPrice, int ItemId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<object, object>();
            bool result = SalesModel.UpdateUnitPrice(UnitPrice, ItemId);
            if (result == true)
            {
                dictionary.Add("title", "Item Price");
                dictionary.Add("type", "success");
                dictionary.Add("text", "Item Price has been updated successfully!");
            }
            else
            {
                dictionary.Add("title", "Item Price");
                dictionary.Add("type", "error");
                dictionary.Add("text", "Item price update FAILED!");
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }

    }
}
