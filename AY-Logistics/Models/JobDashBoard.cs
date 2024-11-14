using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class JobDashBoard
    {
                public List<Dictionary<string, string>> dashboarditem { get; set; }
        public List<StatusVM> StatusList { get; set; }


        public JobDashBoard()
        {

        }

        public List<Dictionary<string, string>> Getdashboarditem()
        {
            string sql = @"SELECT COUNT(LBLS.BLId) AS  Total ,LBLS.BLStatusId, BLS.Name 
                            FROM BLLastStatus  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
                            WHERE LBLS.BLStatusId  in (%DBStatusList%) group by LBLS.BLStatusId , BLS.Name ORDER BY LBLS.BLStatusId";

          //  this.StatusList = StatusVM.GetStatusList(new List<int> { 6, 7 }).ToList();
            List<int> DBStatusList = new List<int> {2};

            sql = sql.Replace("%DBStatusList%", String.Join(",", DBStatusList.Select(i => i.ToString()).ToArray()));

            this.dashboarditem = new List<Dictionary<string, string>>();

            SqlDataReader reader = DBAccess.FetchResult(sql, new List<SqlParameter>(), ConnectionString.DEFAULT);
            Dictionary<string, string> item = new Dictionary<string, string>();
            item = new Dictionary<string, string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    item.Add("Total", reader["Total"].ToString());
                    item.Add("BLStatusId", "2");
                    item.Add("Name", "Received From Documentaion");
                    item.Add("Title", "To be converted to a New Jobs");
                    item.Add("backgroundcolor", "breadcrump-red");

                    /*for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (Convert.ToInt32(reader["BLStatusId"]) == 2 && reader.GetName(i) == "Name")
                        {
                            item.Add(reader.GetName(i), "Sent by Documentaion");
                            // item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                            item.Add("backgroundcolor", "breadcrump-red");
                        }
                         //else if (Convert.ToInt32(reader["BLStatusId"]) == 6 && reader.GetName(i) == "Name")
                         //{
                         //    item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));
                         //    item.Add("backgroundcolor", "breadcrump-green");
                         //}
                        else
                        {
                            item.Add(reader.GetName(i), Convert.ToString(reader.GetValue(i)));

                        }
                    }*/
                    
                }
            }
            else
            {
                item.Add("Total", "0");
                item.Add("BLStatusId", "2");
                item.Add("Name", "Received From Documentaion");
                item.Add("Title", "To be converted to New Jobs");
                item.Add("backgroundcolor", "breadcrump-red");
            }
            this.dashboarditem.Add(item);

            Dictionary<string, string> BLStatus = new Dictionary<string, string>();
            string NewJobCount = GetNewJobCount();
           /*string DOPaymentRequestedCount = GetDOPaymentRequestedCount();
            string DOCollectPendingCount = GetDOCollectPendingCount();
            string DOCollectedCount = GetDOCollectedCount();*/
            string ClearanceReadyCount = GetClearanceReadyCount();
            if (NewJobCount != null)
            {
                BLStatus.Add("Total", NewJobCount);
                BLStatus.Add("BLStatusId", "4");
                //BLStatus.Add("Name", "New Jobs");
                BLStatus.Add("Name", "Document Processing");
                BLStatus.Add("Title", "New Entry & Document Processing");
                BLStatus.Add("backgroundcolor", "breadcrump-maroon");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("BLStatusId", "4");
                //BLStatus.Add("Name", "New Jobs");
                BLStatus.Add("Name", "Document Processing");
                BLStatus.Add("Title", "New Entry & Document Processing");
                BLStatus.Add("backgroundcolor", "breadcrump-maroon");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }

           /* if (DOPaymentRequestedCount != null)
            {
                BLStatus.Add("Total", DOPaymentRequestedCount);
                BLStatus.Add("BLStatusId", "100");
               // BLStatus.Add("Name", "D/O Pay Requested");
                BLStatus.Add("Name", "Document Processing");
                BLStatus.Add("Title", "Waiting for DO Payment Approval");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("BLStatusId", "100");
                // BLStatus.Add("Name", "D/O Pay Requested");
                BLStatus.Add("Name", "Document Processing");
                BLStatus.Add("Title", "Waiting for DO Payment Approval");
                BLStatus.Add("backgroundcolor", "breadcrump-red");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }

            if (DOCollectPendingCount != null)
            {
                BLStatus.Add("Total", DOCollectPendingCount);
                BLStatus.Add("BLStatusId", "101");
               // BLStatus.Add("Name", "DO Collect Pending");
                BLStatus.Add("Name", "Payment & Receipt");
                BLStatus.Add("Title", "To be attach DO scan copy");
                BLStatus.Add("backgroundcolor", "breadcrump-blue");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("BLStatusId", "101");
                // BLStatus.Add("Name", "DO Collect Pending");
                BLStatus.Add("Name", "Payment & Receipt");
                BLStatus.Add("Title", "To be attach DO scan copy");
                BLStatus.Add("backgroundcolor", "breadcrump-blue");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }


            if (DOCollectedCount != null)
            {
                BLStatus.Add("Total", DOCollectedCount);
                BLStatus.Add("BLStatusId", "102");
                //BLStatus.Add("Name", "DO Collected");
                BLStatus.Add("Name", "Clearance");
                BLStatus.Add("Title", "To be enter Clearance Date");
                BLStatus.Add("backgroundcolor", "breadcrump-gray");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("BLStatusId", "102");
                //BLStatus.Add("Name", "DO Collected");
                BLStatus.Add("Name", "Clearance");
                BLStatus.Add("Title", "To be enter Clearance Date");
                BLStatus.Add("backgroundcolor", "breadcrump-gray");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }*/


            if (ClearanceReadyCount != null)
            {
                BLStatus.Add("Total", ClearanceReadyCount);
                BLStatus.Add("BLStatusId", "6");
                BLStatus.Add("Name", "Clearance");
                BLStatus.Add("Title", "Clearance & Delivery");
                BLStatus.Add("backgroundcolor", "breadcrump-green");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }else{
                BLStatus.Add("Total", "0");
                BLStatus.Add("BLStatusId", "6");
                BLStatus.Add("Name", "Clearance");
                BLStatus.Add("Title", "Clearance & Delivery");
                BLStatus.Add("backgroundcolor", "breadcrump-green");
                this.dashboarditem.Add(BLStatus);
                BLStatus = new Dictionary<string, string>();
            }
            return this.dashboarditem;
        }

        public string GetNewJobCount()
        {
            /*string sql = @"SELECT COUNT(JB.Id) AS  Total ,BLLS.BLStatusId, BLS.Name
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId = 4 AND BLLS.BLId = HBL.Id
								INNER JOIN BLStatus AS BLS ON BLS.Id = BLLS.BLStatusId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN ShipmentLastStatus AS SPLS ON SPLS.ShipmentId = SP.Id
								where SPLS.ShipmentStatusId < 3 OR SPLS.ShipmentStatusId IS NULL
								group by BLLS.BLStatusId , BLS.Name  ORDER BY BLLS.BLStatusId";*/
            string sql = @"SELECT COUNT(JB.Id) AS  Total --,BLLS.BLStatusId, BLS.Name
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId IN(4,5,8,9) AND BLLS.BLId = HBL.Id
								INNER JOIN BLStatus AS BLS ON BLS.Id = BLLS.BLStatusId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN ShipmentLastStatus AS SPLS ON SPLS.ShipmentId = SP.Id
								where SPLS.ShipmentStatusId IN(1,2,3,4,5,6,7) AND JB.ClearanceDate IS NULL OR SPLS.ShipmentStatusId IS NULL AND JB.ClearanceDate IS NULL
								--group by BLLS.BLStatusId , BLS.Name  ORDER BY BLLS.BLStatusId";
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

        public string GetDOPaymentRequestedCount()
        {
            string sql = @"SELECT COUNT(SPLS.ShipmentId) AS  Total ,SPLS.ShipmentStatusId, SPS.Name 
                            FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId
							INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SPLS.ShipmentId
							INNER JOIN Job AS JB ON JB.HouseBLId = HBL.Id
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
                            FROM BLLastStatusSkipped  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
							INNER JOIN Job AS JB ON JB.HouseBLId = LBLS.BLId
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

        public string GetDOCollectedCount()
        {
            string sql = @"SELECT COUNT(HBL.Id) AS Total, SPLS.ShipmentStatusId,SPS.Name
                            FROM ShipmentLastStatus AS SPLS
                            INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId AND SP.DateRemoved IS NULL
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                            INNER JOIN BLLastStatusSkipped AS BLLSS ON BLLSS.BLId = HBL.Id AND BLLSS.BLStatusId in(8)
                            INNER JOIN ShipmentStatus AS SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                            where SPLS.ShipmentStatusId in (4) GROUP BY SPLS.ShipmentStatusId,SPS.Name ORDER BY SPLS.ShipmentStatusId";
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

        public string GetClearanceReadyCount()
        {
            /*string sql = @"SELECT COUNT(LBLS.BLId) AS  Total ,LBLS.BLStatusId, BLS.Name 
                            FROM BLLastStatus  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
                            WHERE LBLS.BLStatusId  in (6) group by LBLS.BLStatusId , BLS.Name ORDER BY LBLS.BLStatusId";*/
            string sql = @"SELECT COUNT(JB.Id) AS  Total --,BLLS.BLStatusId, BLS.Name
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId IN(6,5,8,9) AND BLLS.BLId = HBL.Id
								INNER JOIN BLStatus AS BLS ON BLS.Id = BLLS.BLStatusId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN ShipmentLastStatus AS SPLS ON SPLS.ShipmentId = SP.Id
								where SPLS.ShipmentStatusId IN(1,2,3,4,5,6,7) AND JB.ClearanceDate IS NOT NULL OR SPLS.ShipmentStatusId IS NULL AND JB.ClearanceDate IS NOT NULL
								--group by BLLS.BLStatusId , BLS.Name  ORDER BY BLLS.BLStatusId";
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