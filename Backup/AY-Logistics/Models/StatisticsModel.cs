using MyDBAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AYLogistics.Models
{
    public class StatisticsModel
    {
        public StatisticsModel()
        {
        }

        public static Dictionary<object, object> GetAllData(DateTime StartDate, DateTime EndDate)
        {

            List<Dictionary<object, object>> AllDataList = GetAllJobs(StartDate, EndDate);

            string sql = @"select --SP.Id,
	                               --HBL.Id,
	                               --MF.Id,
	                               MF.Number,
	                               JB.JobNo,
	                               /*(select  Jb.JobNo  + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN Job AS Jb ON Jb.HouseBLId = HouseBL.Id
		                            where HouseBL.ShipmentId =SP.Id AND HouseBL.DateRemoved IS NULL
		                            FOR XML PATH('')) AS JobNo,*/

	                               V.Name AS VesselName,
	                               SP.VoyageNo,
	                               Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
	                               Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
	                               Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,

	                               (select TOP 1 Convert(nvarchar(10),SHPS.DateCreated,103) from ShipmentStatus
		                            INNER JOIN ShipmentAllStatus AS SHPS ON SHPS.ShipmentStatusId = ShipmentStatus.Id
		                            where ShipmentStatus.Id = 4 AND SHPS.ShipmentId = SP.Id
		                            order by SHPS.DateCreated DESC) AS DOPayApproved,

	                                Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
		                            Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
	                               MS.Name AS ModeOfShipment,

	                               (select ISNULL(p.Code,'NIL') + ', ' AS 'data()'
	                                from HouseBL AS HB
		                            INNER JOIN [Port] AS p ON p.Id = HB.PortOfLoading
		                            WHERE HB.ShipmentId = SP.Id AND HB.DateRemoved IS NULL
		                            order by HB.Id asc
		                            FOR XML PATH('')) AS PortOfLoading,

	                               (select ISNULL(p.Code,'NIL') + ', ' AS 'data()'
	                                from HouseBL AS HB
		                            INNER JOIN [Port] AS p ON p.Id = HB.PortOfUnloading
		                            WHERE HB.ShipmentId = SP.Id AND HB.DateRemoved IS NULL
		                            order by HB.Id asc
		                            FOR XML PATH('')) AS PortOfUnloading, --PORT OF DISCHARGE

	                               JB.DeliveryPlace,
	                               SP.MasterBL,

	                               (select ISNULL(HouseBL.HouseBL,'NIL')  + ', ' AS 'data()'
		                            from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            where HouseBL.ShipmentId =SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS HouseBL,

	                               (select ISNULL(Party.[Name],'NIL')  + ', ' AS 'data()'
		                            from HouseBL AS HB
		                            INNER JOIN Party ON Party.Id = HB.ShipperId
		                            WHERE HB.ShipmentId = SP.Id AND HB.DateRemoved IS NULL
		                            order by HB.Id asc
		                            FOR XML PATH('')) AS Shipper,

	                               (select ISNULL(Party.[Name],'NIL')  + ', ' AS 'data()'
		                            from HouseBL AS HB
		                            INNER JOIN Party ON Party.Id = HB.CustomerId
		                            WHERE HB.ShipmentId = SP.Id AND HB.DateRemoved IS NULL
		                            order by HB.Id asc
		                            FOR XML PATH('')) AS Customer,

	                               (select ISNULL(Party.[Name],'')  + ', ' AS 'data()'
		                            from HouseBL AS HB
		                            INNER JOIN Party ON Party.Id = HB.NotifyPartyId
		                            WHERE HB.ShipmentId = SP.Id AND HB.DateRemoved IS NULL
		                            order by HB.Id asc
		                            FOR XML PATH('')) AS NotifyParty,

	                               (select ISNULL(Party.[Name],'')  + ', ' AS 'data()'
		                            from Party
		                            WHERE Party.Id = SP.ShippingAgentId
		                            FOR XML PATH('')) AS ShippingAgent,

	                               (select ISNULL(bn.[Name],'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN BLNature AS bn ON bn.Id = HouseBL.BLNatureId
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS BLNature,

	                               (select ISNULL(fi.CODE,'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN FreightIndicator AS fi ON fi.Id = HouseBL.FreightIndicatorId
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS FreightIndicator,

	                               --(STUFF((select containerNo AS c from ContainerInfo where HouseBLId = SP.Id FOR XML PATH('')),1,0,'')) AS ContainerNo,
	                               (select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

	                               (select ISNULL(CT.[Name],'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            LEFT JOIN ContainerType AS CT ON CT.Id = CI.ContainerTypeId 
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS ContainerType,

	                               (select ISNULL(CONVERT(VARCHAR(12), CI.Size),'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
	                                FOR XML PATH('')) AS ContainerSize,

	                               (select ISNULL(Cin.[Description],'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            INNER JOIN ContainerIndicator AS Cin ON Cin.Id = CI.ContainerIndicatorId
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS ContainerIndicator,

	                               (select ISNULL(CONVERT(VARCHAR(12), CI.CNoOfPackage),'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
	                                FOR XML PATH('')) AS NoOfPackage,

	                              (select ISNULL(Tpk.CODE,'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            INNER JOIN ContainerInfo AS CI ON CI.HouseBLId = HouseBL.Id
		                            INNER JOIN TypeofPackage AS Tpk ON Tpk.Id = CI.TypeofPackageId
		                            WHERE HouseBL.ShipmentId = SP.Id AND HouseBL.DateRemoved IS NULL
		                            order by HouseBL.Id asc
		                            FOR XML PATH('')) AS TypeofPackage,

	                               (select ISNULL([Description],'NIL') + ', ' AS 'data()'
	                                from HouseBL
		                            WHERE ShipmentId = SP.Id AND DateRemoved IS NULL
		                            order by Id asc
	                                FOR XML PATH('')) AS [Description]
	                               --INVOICE NO
	                               --PAYMENT STATUS
	                               --REMARKS 
                            FROM Job AS JB
                            LEFT JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
                            RIGHT JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            LEFT JOIN Vessel AS V ON SP.VesselId = V.Id
                            LEFT JOIN ModeOfShipment AS MS ON SP.ModeofShipmentId = MS.Id
                            WHERE SP.ManifestId > 0 AND JB.Id IS NULL AND MF.CreatedDate Between @StartDate AND @EndDate
                            order by MF.Id";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@StartDate", DbType.DateTime);
            param.Value = StartDate;
            spList.Add(param);
            param = new SqlParameter("@EndDate", DbType.DateTime);
            param.Value = EndDate;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                AllDataList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", AllDataList);
            return dataTable;
        }
        public static List<Dictionary<object, object>> GetAllJobs(DateTime StartDate, DateTime EndDate)
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

            string sql = @"SELECT --SP.Id,
	                               --HBL.Id,
	                               --MF.Id,
	                               MF.Number,
	                               JB.JobNo,
	                               VS.Name AS VesselName,
	                               SP.VoyageNo,
	                               Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
	                               Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
	                               Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,

	                               (select TOP 1 Convert(nvarchar(10),SHPS.DateCreated,103) from ShipmentStatus
		                            INNER JOIN ShipmentAllStatus AS SHPS ON SHPS.ShipmentStatusId = ShipmentStatus.Id
		                            where ShipmentStatus.Id = 4 AND SHPS.ShipmentId = SP.Id
		                            order by SHPS.DateCreated DESC) AS DOPayApproved,

	                                Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
		                            Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
	                               MS.Name AS ModeOfShipment,

	                               (select CONCAT([Name],'(',Code,')') from [Port] where Id = HBL.PortOfLoading) AS PortOfLoading,
	                               (select CONCAT([Name],'(',Code,')') from [Port] where Id = HBL.PortOfUnloading) AS PortOfUnloading,--PORT OF DISCHARGE

	                               JB.DeliveryPlace,
	                               SP.MasterBL,
	                               HBL.HouseBL,

	                               (select [Name] from [Party] where Id = HBL.ShipperId) AS Shipper,
	                               CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
	                               (select [Name] from [Party] where Id = HBL.NotifyPartyId) AS NotifyParty,
	                               (select [Name] from [Party] where Id = SP.ShippingAgentId) AS ShippingAgent,
	                               (select [Name] from BLNature WHERE Id = HBL.BLNatureId) AS BLNature,
	                               (select [Name] from FreightIndicator WHERE Id = HBL.FreightIndicatorId) AS FreightIndicator,

	                               --(STUFF((select containerNo AS c from ContainerInfo where HouseBLId = SP.Id FOR XML PATH('')),1,0,'')) AS ContainerNo,
	                               (select containerNo + ', ' AS 'data()'
		                            from ContainerInfo where HouseBLId = HBL.Id
		                            FOR XML PATH('')) AS ContainerNo,

	                               (select CT.[Name] + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            INNER JOIN ContainerType AS CT ON CT.Id = CI.ContainerTypeId WHERE CI.HouseBLId = HBL.Id 
		                            FOR XML PATH('')) AS ContainerType,

	                               (select CONVERT(VARCHAR(12), Size) + ', ' AS 'data()'
	                                from ContainerInfo where HouseBLId = HBL.Id 
	                                FOR XML PATH('')) AS ContainerSize,

	                               (select Cin.[Description] + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            INNER JOIN ContainerIndicator AS Cin ON Cin.Id = CI.ContainerIndicatorId
		                            WHERE CI.HouseBLId = HBL.Id 
		                            FOR XML PATH('')) AS ContainerIndicator,
	                               HBL.NoOfPackage,
	                              (select Tpk.CODE + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            INNER JOIN TypeofPackage AS Tpk ON Tpk.Id = CI.TypeofPackageId
		                            WHERE CI.HouseBLId = HBL.Id 
		                            FOR XML PATH('')) AS TypeofPackage,
	                               HBL.[Description]
	                              -- CT.Name AS Commodity,
	                              -- Pro.Name AS Project
	                               --INVOICE NO
	                               --PAYMENT STATUS
	                               --REMARKS 

                            FROM Job AS JB
                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
                            LEFT JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            where MF.Number IS NOT NULL AND JB.RegistrationDate Between @StartDate AND @EndDate";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@StartDate", DbType.DateTime);
            param.Value = StartDate;
            spList.Add(param);
            param = new SqlParameter("@EndDate", DbType.DateTime);
            param.Value = EndDate;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Manifest);
            }
            reader.Close();

            //Dictionary<object, object> dataTable = new Dictionary<object, object>();
            //dataTable.Add("aaData", JobList);
            //return dataTable;
            return JobList;
        }
    }
}