using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AYLogistics.Services;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class AuthController : Controller
    {
        public void SyncMyBandharu()
        {
            List<Dictionary<object, object>> PendingBLlist = ManifestModel.GetPendingBLs();
            List<string> CustIdList = new List<string>();
            foreach (Dictionary<object, object> dictionary in PendingBLlist)
            {
                CustIdList.Add(dictionary["CustomerId"].ToString().Trim()); // get All Customer Id to remove dublicates
            }
            List<string> CustIdList_unique = CustIdList.Distinct().ToList(); // Remove Duplicates
            foreach (var CustId in CustIdList_unique)
            {
/**FOR TESTING**/if (CustId == "5220") // (CustomerId of Centara lagoon Project--- DevDB - 5220 / LiveDB - 7240
                {
                    string MyBandharuUser = "";
                    string MyBandharuPwd = "";
                    string LoginStatus = "";
                    foreach (Dictionary<object, object> dictionary in PendingBLlist)
                    {
                        foreach (string key in dictionary.Keys)
                        {
                            if (key.Equals("CustomerId"))
                            {
                                string value = dictionary[key].ToString();
                                if (value == CustId)
                                {
                                    if (LoginStatus == "") // track a log of customer login status
                                    {
                                        MyBandharuUser = dictionary["MyBandharuUser"].ToString();
                                        MyBandharuPwd = dictionary["MyBandharuPwd"].ToString();
                                        LoginStatus = MPLAuthService.Authenticate(MyBandharuUser, MyBandharuPwd); // Auth login
                                    }
                                    if (LoginStatus == "OK") // track each function status under customer
                                    {
                                        int ShipmentId = Convert.ToInt32(dictionary["ShipmentId"]);
                                        int BLId = Convert.ToInt32(dictionary["Id"]);
                                        string BLnumber = dictionary["HouseBL"].ToString();
                                        string CustomerName = dictionary["Name"].ToString();
                                        // Fetch Data from MYBandharu and save to DB
                                    }
                                }
                            }
                            break; // break once meet and update for next dictionary
                        }
                    }
                } /****FOR TESTING -------- REMOVE this Function after testing***/
            }
        }


                             
    }
}
