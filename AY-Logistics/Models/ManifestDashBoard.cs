using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class ManifestDashBoard
    {
        public List<Dictionary<string, string>> dashboarditem { get; set; }
        public List<StatusVM> StatusList { get; set; }


        public ManifestDashBoard()
        {

        }

        public List<Dictionary<string, string>> Getdashboarditem()
        {
            string sql = @"SELECT COUNT(SPLS.ShipmentId) AS  Total ,SPLS.ShipmentStatusId, SPS.Name 
                            FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId
							INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            WHERE SPLS.ShipmentStatusId  in (%DBStatusList%) group by SPLS.ShipmentStatusId , SPS.Name ORDER BY SPLS.ShipmentStatusId";

          //  this.StatusList = StatusVM.GetStatusList(new List<int> { 6, 7 }).ToList();
           // List<int> DBStatusList = new List<int> { 1, 2, 3};
            List<int> DBStatusList = new List<int> { 1 };

            sql = sql.Replace("%DBStatusList%", String.Join(",", DBStatusList.Select(i => i.ToString()).ToArray()));

            this.dashboarditem = new List<Dictionary<string, string>>();

            SqlDataReader reader = DBAccess.FetchResult(sql, new List<SqlParameter>(), ConnectionString.DEFAULT);
            Dictionary<string, string> item = new Dictionary<string, string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    item.Add("Total", reader["Total"].ToString());
                    item.Add("ShipmentStatusId", "1");
                    item.Add("Name", "NEW Entry");
                    item.Add("Title", "To be Generate XML");
                    item.Add("backgroundcolor", "breadcrump-maroon");
                    /*   for (int i = 0; i < reader.FieldCount; i++)
                     {
                         if (Convert.ToInt32(reader["ShipmentStatusId"]) == 1 && reader.GetName(i) == "Name")
                         {
                             //item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                             item.Add(reader.GetName(i), "BL Entry");
                             item.Add("Title", "To be Generate XML");
                             item.Add("backgroundcolor", "breadcrump-maroon");
                         }
                       else if (Convert.ToInt32(reader["ShipmentStatusId"]) == 2 && reader.GetName(i) == "Name")
                         {
                             //item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                             item.Add(reader.GetName(i), "Manifest Submitted");
                             item.Add("Title", "To be Request DO Payment and attach Debit Note");
                             item.Add("backgroundcolor", "breadcrump-green");
                         }
                         else if (Convert.ToInt32(reader["ShipmentStatusId"]) == 3 && reader.GetName(i) == "Name")
                         {
                             //item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                             item.Add(reader.GetName(i), "Arrival Updates");
                             item.Add("Title", "Waiting for DO Payment Approval");
                             item.Add("backgroundcolor", "breadcrump-red");
                         }
                         else
                         {
                             item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));

                         }
                     }
                    this.dashboarditem.Add(item);*/
                }
            }
            else
            {
                item = new Dictionary<string, string>();
                item.Add("Total", "0");
                item.Add("ShipmentStatusId", "1");
                item.Add("Name", "NEW Entry");
                item.Add("Title", "To be Generate XML");
                item.Add("backgroundcolor", "breadcrump-maroon");
            }
            this.dashboarditem.Add(item);

            Dictionary<string, string> BLStatus = new Dictionary<string, string>();
            string DOManifestedCount = GetMenifested();
            string DOPayRequestCount = GetDOPayRequest();
            string DOCollectPendingCount = GetDOCollectPendingCount();
            string NewBolCount = GetNewBolCount();
            string DOReleaseCount = GetDOReleaseCount();

            if (DOManifestedCount != null)
            {
                BLStatus.Add("Total", DOManifestedCount);
                BLStatus.Add("ShipmentStatusId", "2");
                BLStatus.Add("Name", "Manifest Submitted");
                BLStatus.Add("Title", "To be Request DO Payment and attach Debit Note");
                BLStatus.Add("backgroundcolor", "breadcrump-green");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", DOManifestedCount);
                BLStatus.Add("ShipmentStatusId", "2");
                BLStatus.Add("Name", "Manifest Submitted");
                BLStatus.Add("Title", "To be Request DO Payment and attach Debit Note");
                BLStatus.Add("backgroundcolor", "breadcrump-green");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }

            if (DOPayRequestCount != null)
            {
                BLStatus.Add("Total", DOPayRequestCount);
                BLStatus.Add("ShipmentStatusId", "3");
                BLStatus.Add("Name", "DO Payment Requested");
                BLStatus.Add("Title", "Waiting for DO Payment Approval");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("ShipmentStatusId", "3");
                BLStatus.Add("Name", "DO Payment Requested");
                BLStatus.Add("Title", "Waiting for DO Payment Approval");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }


            if (DOCollectPendingCount != null)
            {
                BLStatus.Add("Total", DOCollectPendingCount);
                BLStatus.Add("ShipmentStatusId", "4");
               // BLStatus.Add("Name", "DO Collect Pending");
                BLStatus.Add("Name", "DO Payment Approved");
                BLStatus.Add("Title", "To be attach DO Scan Copy");
                BLStatus.Add("backgroundcolor", "breadcrump-blue");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("ShipmentStatusId", "4");
               // BLStatus.Add("Name", "DO Collect Pending");
                BLStatus.Add("Name", "DO Payment Approved");
                BLStatus.Add("Title", "To be attach DO Scan Copy");
                BLStatus.Add("backgroundcolor", "breadcrump-blue");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }


            if (NewBolCount != null)
            {
                BLStatus.Add("Total", NewBolCount);
                BLStatus.Add("ShipmentStatusId", "5");
               // BLStatus.Add("Name", "DO Collected");
                BLStatus.Add("Name", "DO Releasing / OPERATION");
                BLStatus.Add("Title", "To be Release DO / Send to Operation");
                BLStatus.Add("backgroundcolor", "breadcrump-gray");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("ShipmentStatusId", "5");
               // BLStatus.Add("Name", "DO Collected");
                BLStatus.Add("Name", "DO Releasing / JOB");
                BLStatus.Add("Title", "To be Release DO / Send to Operation");
                BLStatus.Add("backgroundcolor", "breadcrump-gray");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }



            if (DOReleaseCount != null)
            {
                BLStatus.Add("Total", DOReleaseCount);
                BLStatus.Add("ShipmentStatusId", "-1");
                BLStatus.Add("Name", "DO Released & POD Uploading");
                BLStatus.Add("Title", "To be attach Proof Of Delivery");
                BLStatus.Add("backgroundcolor", "breadcrump-black");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("ShipmentStatusId", "-1");
                BLStatus.Add("Name", "DO Released & POD Uploading");
                BLStatus.Add("Title", "To be attach Proof Of Delivery");
                BLStatus.Add("backgroundcolor", "breadcrump-black");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }

         return this.dashboarditem;

        }
        public string GetMenifested()
        {
            string sql = @"SELECT COUNT(SPLS.ShipmentId) AS  Total ,SPLS.ShipmentStatusId, SPS.Name 
                            FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId
							INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            WHERE SPLS.ShipmentStatusId  in (2) group by SPLS.ShipmentStatusId , SPS.Name ORDER BY SPLS.ShipmentStatusId";
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
        public string GetDOPayRequest()
        {
            string sql = @"SELECT COUNT(SPLS.ShipmentId) AS  Total ,SPLS.ShipmentStatusId, SPS.Name 
                            FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId
							INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
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
        public string GetDOCollectPendingCount()
        {
            string sql = @"SELECT COUNT(LBLS.BLId) AS  Total ,LBLS.BLStatusId AS ShipmentStatusId, BLS.Name 
                            FROM BLLastStatus  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
							INNER JOIN Shipment AS SP ON SP.Id = LBLS.ShipmentId
							INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            WHERE LBLS.BLStatusId  in (5) group by LBLS.BLStatusId , BLS.Name ORDER BY LBLS.BLStatusId";
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
        public string GetNewBolCount()
        {
            string sql = @"SELECT COUNT(HBL.Id) AS Total, SPLS.ShipmentStatusId,SPS.Name
                            FROM ShipmentLastStatus AS SPLS
                            INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId AND SP.DateRemoved IS NULL
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLId = HBL.Id AND BLLS.BLStatusId in(8)
                            INNER JOIN ShipmentStatus AS SPS ON SPS.Id = SPLS.ShipmentStatusId
							LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                            where SPLS.ShipmentStatusId in (4) AND JB.JobNo IS NULL GROUP BY SPLS.ShipmentStatusId,SPS.Name ORDER BY SPLS.ShipmentStatusId";
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
        public string GetDOReleaseCount()
        {
            string sql = @"SELECT COUNT(LBLS.BLId) AS  Total ,LBLS.BLStatusId AS ShipmentStatusId, BLS.Name 
                            FROM BLLastStatus  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
                            WHERE LBLS.BLStatusId  in (3) AND LBLS.DOAttached IS NULL group by LBLS.BLStatusId , BLS.Name ORDER BY LBLS.BLStatusId";
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