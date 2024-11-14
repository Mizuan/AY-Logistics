using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class PaymentDashBoard
    {
        public List<Dictionary<string, string>> dashboarditem { get; set; }
        public List<StatusVM> StatusList { get; set; }

        public PaymentDashBoard()
        {

        }

        public List<Dictionary<string, string>> Getdashboarditem()
        {
            string sql = @"SELECT COUNT(LBLS.BLId) AS  Total ,LBLS.BLStatusId AS PaymentStatId, BLS.Name 
                            FROM BLLastStatus  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
                            WHERE LBLS.BLStatusId  in (%DBStatusList%) group by LBLS.BLStatusId , BLS.Name ORDER BY LBLS.BLStatusId";

            //  this.StatusList = StatusVM.GetStatusList(new List<int> { 6, 7 }).ToList();
            List<int> DBStatusList = new List<int> {0};

            sql = sql.Replace("%DBStatusList%", String.Join(",", DBStatusList.Select(i => i.ToString()).ToArray()));

            this.dashboarditem = new List<Dictionary<string, string>>();

            SqlDataReader reader = DBAccess.FetchResult(sql, new List<SqlParameter>(), ConnectionString.DEFAULT);
            Dictionary<string, string> item = new Dictionary<string, string>();

            while (reader.Read())
            {
                item = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (Convert.ToInt32(reader["PaymentStatId"]) == 2 && reader.GetName(i) == "Name")
                    {
                         item.Add(reader.GetName(i), "Port HandlingX");
                        //item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                        item.Add("backgroundcolor", "breadcrump-green");
                    }
                    else if (Convert.ToInt32(reader["PaymentStatId"]) == 4 && reader.GetName(i) == "Name")
                    {
                        item.Add(reader.GetName(i), "Custom DutyX");
                       // item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                        item.Add("backgroundcolor", "breadcrump-blue");
                    }
                    else
                    {
                        item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));

                    }
                }
                this.dashboarditem.Add(item);
            }

            Dictionary<string, string> BLStatus = new Dictionary<string, string>();
            BLStatus.Add("Total", GetNewBolCount());
            if (BLStatus["Total"] == null)
            {
                BLStatus["Total"] = "0";
                BLStatus.Add("PaymentStatId", "3");
                BLStatus.Add("Title", "D/O Payment");
                BLStatus.Add("Name", "D/O Payment");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                return this.dashboarditem;
            }
            else
            {
                BLStatus.Add("PaymentStatId", "3");
                BLStatus.Add("Name", "D/O Payment");
                BLStatus.Add("Title", "D/O Payment");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                return this.dashboarditem;
            }

        }
        public string GetNewBolCount()
        {
            string sql = @"SELECT COUNT(SPLS.ShipmentId) AS  Total ,SPLS.ShipmentStatusId AS PaymentStatId, SPS.Name 
                            FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
                            WHERE SPLS.ShipmentStatusId  in (3) group by SPLS.ShipmentStatusId , SPS.Name ORDER BY SPLS.ShipmentStatusId";
            string count = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                count = reader[0].ToString();
            }
            reader.Close();
            return count;
        }

    }
}