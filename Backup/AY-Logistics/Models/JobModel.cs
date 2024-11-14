using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MyDBAccess;
using System.Data;

namespace AYLogistics.Models
{
    public class JobModel
    {
        public int JobId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? DemurrageDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public int CommodityTypeId { get; set; }
        public int JobPriorityId { get; set; }
      //  public string CNumber { get; set; }
        public string RNumber { get; set; }
        public DateTime? RNumberReqDate { get; set; }
        public string ANumber { get; set; }
        public DateTime? ANumberReqDate { get; set; }
        public string CustomerReference { get; set; }

        public Commodity Commodity { get; set; }
        public Project Project { get; set; }
        public Vehicle Vehicle { get; set; }
        public JobPriority JobPriority { get; set; }
       // public List<JobStatusModel> JobStatusList { get; set; }
        public JobStatusModel JobStatusModel { get; set; }

        public JobModel()
        {
            Commodity = new Commodity();
            Project = new Project();
            Vehicle = new Vehicle();
            JobPriority = new JobPriority();
          //  JobStatusList = JobStatusModel.GetJobStatusDetail();
            JobStatusModel = new JobStatusModel();
            AutoComplete VesselId = new AutoComplete();

        }
        public JobModel(int JbId)
        {
            Commodity = new Commodity();
            Project = new Project();
            Vehicle = new Vehicle();
            JobPriority = new JobPriority();
            JobStatusModel = new JobStatusModel(JbId);
            AutoComplete VesselId = new AutoComplete();

        }


    }

    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentName { get; set; }
        public string DocumentNo { get; set; }
        public int PaymentStatus { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int FreightIndicator { get; set; }
        public int HBLid { get; set; }
        public List<PaymentModel> PaymentStatusList { get; set; }

        public PaymentModel()
        {
        }

        public PaymentModel(int PaymentId)
        {
            PaymentStatusList = GetPaymentStatusDetail(PaymentId);
        }

        public static List<PaymentModel> GetPaymentStatusDetail(int PaymentId)
        {
            List<Dictionary<object, object>> PaymentStatusList = JobStatusModel.GetPaymentStatusList(PaymentId);
            List<Dictionary<object, object>> PaymentStatusH = new List<Dictionary<object, object>>();
            List<PaymentModel> list = new List<PaymentModel>();
            foreach (Dictionary<object, object> PaymentStatus in PaymentStatusList)
            {
                PaymentStatusH = JobStatusModel.GetPaymentStatusHistory(PaymentId, Convert.ToInt32(PaymentStatus["Id"]));
                string a = "";
                string b = "";
                string c = "";
                int d = 0;
                string e = "";
                string f = "";
                foreach (var StatusH in PaymentStatusH)
                {
                    a = StatusH["DocumentNo"] == DBNull.Value ? "" : Convert.ToString(StatusH["DocumentNo"]);
                    b = StatusH["UpdatedDate"] == DBNull.Value ? "" : Convert.ToString(StatusH["UpdatedDate"]);
                    c = StatusH["Employee"] == DBNull.Value ? "" : Convert.ToString(StatusH["Employee"]);
                    d = StatusH["PaymentStat"] == DBNull.Value ? 0 : Convert.ToInt32(StatusH["PaymentStat"]);
                    e = StatusH["PaymentTypesId"] == DBNull.Value ? "" : Convert.ToString(StatusH["PaymentTypesId"]);
                    f = StatusH["FreightIndicatorId"] == DBNull.Value ? "" : Convert.ToString(StatusH["FreightIndicatorId"]);
                }
                if (e=="1" && f=="1")
                {
                }else{
                    PaymentModel Payment = new PaymentModel
                    {
                        PaymentTypeId = Convert.ToInt32(PaymentStatus["Id"]),
                        PaymentName = PaymentStatus["Name"].ToString(),
                        DocumentNo = a,
                        UpdatedDate = b,
                        UpdatedBy = c,
                        PaymentStatus = d,
                    };
                    list.Add(Payment);
                }
            }
            return list;
        }

        public static Dictionary<object, object> GetUnpaidList()
        {
            List<Dictionary<object, object>> unpaidList = new List<Dictionary<object, object>>();

            string sql = @"SELECT Pmt.Id AS PaymentId,
		                            SP.Id AS ShipmentId,
                                    HBL.Id AS HBLId,
									Pmt.DocumentNo,
	                               CONCAT(MF.Number,CHAR(13) + CHAR(10), '<br/>',JB.JobNo) AS Number ,
	                               CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
	                               ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
	                               VS.Name AS VesselName,
	                               MS.Name AS ShipmentMode,
	                               FI.Name AS FreightIndicatorName,
	                               PT.Name AS PaymentType
                            FROM payments AS Pmt
                            INNER JOIN HouseBL AS HBL ON HBL.Id = Pmt.HouseBLId
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
                            INNER JOIN PaymentTypes AS PT ON PT.Id = Pmt.PaymentTypesId
                            LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                            LEFT JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
                            LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                            WHERE pmt.PaymentStat <1";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> unpaid = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    unpaid.Add(reader.GetName(i), reader.GetValue(i));
                }
                unpaidList.Add(unpaid);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", unpaidList);
            return dataTable;
        }
    }

    public class JobStatusModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int JobStatusValue { get; set; }
        public string JobStatusValueName { get; set; }
        public string DateCreated { get; set; }
        public string Employee { get; set; }
        public string Remarks { get; set; }
        public int JobId { get; set; }
        public List<JobStatusModel> JobStatusList { get; set; }
        public List<PaymentModel> PaymentStatusList { get; set; }

        public JobStatusModel()
        {

        }
        public JobStatusModel(int JbId)
        {
            JobStatusList = GetJobStatusDetail(JbId);
            PaymentStatusList = GetPaymentStatusDetail(JbId);
        }

        public static List<Dictionary<object, object>> GetJobStatusHistory(int JbId, int JobStatusId)
        {
            string sql = @"SELECT TOP 1 JSH.JobStatusId,
                                        JSH.JobStatusValue,
                                        JSH.DateCreated,
                                        JSH.Remarks,
                                        CONCAT(EMP.FirstName,' ',EMP.MiddleName,' ',EMP.LastName) AS Employee
                                        FROM JobStatusHistory AS JSH
                                        INNER JOIN Employee AS EMP ON EMP.Id = JSH.EmployeeId
                                        WHERE JobId = @JobId and JobStatusId = @JobStatusId
                                        ORDER BY DateCreated DESC";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = JbId;
            spList.Add(param);
            param = new SqlParameter("@JobStatusId", DbType.Int32);
            param.Value = JobStatusId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> GetJobStatusList = new List<Dictionary<object, object>>();

            while (reader.Read())
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                GetJobStatusList.Add(dictionary);
            }
            reader.Close();
            return GetJobStatusList;

        }

        public static List<Dictionary<object, object>> GetPaymentStatusHistory(int JbId, int PaymentStatusId)
        {
           /* string sql = @"SELECT TOP 1 JP.PaymentTypesId,
                                        JP.UpdatedDate,
                                        CONCAT(EMP.FirstName,' ',EMP.MiddleName,' ',EMP.LastName) AS Employee,
                                        JP.DocumentNo
                                        FROM JobPayments AS JP
                                        INNER JOIN Employee AS EMP ON EMP.Id = JP.PaidUpdatedBy
                                        WHERE JP.JobId = @JobId and PaymentTypesId = @PaymentStatusId
                                        ORDER BY UpdatedDate DESC";*/
            string sql = @"SELECT TOP 1 JP.PaymentTypesId,
                                        JP.UpdatedDate,
                                        CONCAT(EMP.FirstName,' ',EMP.MiddleName,' ',EMP.LastName) AS Employee,
                                        JP.DocumentNo,
										HBL.FreightIndicatorId,
										JP.PaymentStat
                                        FROM Payments AS JP
										INNER JOIN Job AS Jb ON Jb.HouseBLId = JP.HouseBLId
										INNER JOIN HouseBL AS HBL ON HBL.Id = Jb.HouseBLId
                                        INNER JOIN Employee AS EMP ON EMP.Id = JP.PaidUpdatedBy
                                        WHERE Jb.Id = @JobId AND JP.PaymentTypesId = @PaymentStatusId
                                        ORDER BY UpdatedDate DESC";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = JbId;
            spList.Add(param);
            param = new SqlParameter("@PaymentStatusId", DbType.Int32);
            param.Value = PaymentStatusId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> GetJobStatusList = new List<Dictionary<object, object>>();

            while (reader.Read())
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                GetJobStatusList.Add(dictionary);
            }
            reader.Close();
            return GetJobStatusList;

        }

        public bool EditJobStat(int job_StatId, string job_Remarks, int job_statValue, int JobId)
        {
            String sql = @"INSERT INTO [dbo].[JobStatusHistory]
                                       ([JobStatusId]
                                       ,[JobId]
                                       ,[DateCreated]
                                       ,[Remarks]
                                       ,[EmployeeId]
                                       ,[JobStatusValue])
                                 VALUES
                                       (@job_StatId
                                       ,@JobId
                                       ,GETDATE()
                                       ,@job_Remarks
                                       ,@EmployeeId
                                       ,@job_statValue)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@job_StatId", DbType.Int32);
            param.Value = job_StatId;
            List.Add(param);
            param = new SqlParameter("@job_Remarks", DbType.String);
            param.Value = job_Remarks;
            List.Add(param);
            param = new SqlParameter("@job_statValue", DbType.Int32);
            param.Value = job_statValue;
            List.Add(param);
            param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = JobId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateBLStatus(int BLStatus, int BLId)
        {
            String sql = @"INSERT INTO [dbo].[BLStatusHistory]
                                       ([BLStatusId]
                                       ,[HouseBLId]
                                       ,[DateCreated]
                                       ,[EmployeeId])
                                 VALUES
                                       (@BLStatus
                                       ,@HBLLastId
                                       ,GETDATE()
                                       ,@EmployeeId)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BLStatus", DbType.Int32);
            param.Value = BLStatus;
            List.Add(param);
            param = new SqlParameter("@HBLLastId", DbType.Int32);
            param.Value = BLId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateDeliveryDate(DateTime DDate, int NCP, int NDP,int NDm, int JobId, int DeliveredBy, string ReceivedBy)
        {
            String sql = @"UPDATE [dbo].[Job]
                               SET [DeliveryDate] =  @DDate
                                  ,[NoOfClearedPackage] = @NCP
                                  ,[NoOfDeliveredPackage] = @NDP
,[NoOfDamagePackage] = @NDm
                                  ,[DeliveredBy] = @DeliveredBy
                                  ,[ReceivedBy] = @ReceivedBy
                             WHERE Id = @JobId";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = JobId;
            List.Add(param);
            param = new SqlParameter("@DDate", DbType.DateTime);
            param.Value = DDate;
            List.Add(param);
            param = new SqlParameter("@NCP", DbType.Int32);
            param.Value = NCP;
            List.Add(param);
            param = new SqlParameter("@NDP", DbType.Int32);
            param.Value = NDP;
            List.Add(param);
            param = new SqlParameter("@NDm", DbType.Int32);
            param.Value = NDm;
            List.Add(param);
            param = new SqlParameter("@DeliveredBy", DbType.Int32);
            param.Value = DeliveredBy;
            List.Add(param);
            param = new SqlParameter("@ReceivedBy", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ReceivedBy) ? (object)DBNull.Value : ReceivedBy;
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool EditPaymentStat(int PTypeId, string PDocumentNo, int HouseBLId, int PaymentStat)
        {
            String sql = @"INSERT INTO [dbo].[Payments]
                                       ([HouseBLId]
                                       ,[PaymentTypesId]
                                       ,[UpdatedDate]
                                       ,[PaidUpdatedBy]
                                       ,[DocumentNo]
                                       ,[PaymentStat])
                                 VALUES
                                       (@HouseBLId
                                       ,@PaymentTypesId
                                       ,GETDATE()
                                       ,@EmployeeId
                                       ,@DocumentNo
                                       ,@PaymentStat)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HouseBLId;
            List.Add(param);
            param = new SqlParameter("@PaymentTypesId", DbType.Int32);
            param.Value = PTypeId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@DocumentNo", DbType.String);
            param.Value = PDocumentNo;
            List.Add(param);
            param = new SqlParameter("@PaymentStat", DbType.Int32);
            param.Value = PaymentStat;
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }
        public bool UpdateFreightInvoice(int HBLId)
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [FreightInvoice] = GETDATE()
                         WHERE HouseBLId = @HBLId";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }
        public bool NullFreightInvoice(int HBLId)
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [FreightInvoice] = NULL
                         WHERE HouseBLId = @HBLId";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateClearanceInvoice(int HBLId)
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [ClearanceInvoice] = GETDATE()
                         WHERE HouseBLId = @HBLId";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public bool NullClearanceInvoice(int HBLId)
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [ClearanceInvoice] = NULL
                         WHERE HouseBLId = @HBLId";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static List<Dictionary<object, object>> GetJobStatusList(int JbId)
        {
            string sql = @"select Id,Name from JobStatus Order by Id ASC";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> GetJobStatusList = new List<Dictionary<object, object>>();

            while (reader.Read())
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                GetJobStatusList.Add(dictionary);
            }
            reader.Close();
            return GetJobStatusList;

        }

        public static List<Dictionary<object, object>> GetPaymentStatusList(int JbId)
        {
            string sql = @"select Id,Name from PaymentTypes Order by Id ASC";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> GetPaymentStatusList = new List<Dictionary<object, object>>();

            while (reader.Read())
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                GetPaymentStatusList.Add(dictionary);
            }
            reader.Close();
            return GetPaymentStatusList;

        }

        public static int GetFreigtIndicator(int JbId)
        {
            string sql = @"SELECT HBL.FreightIndicatorId 
                                FROM Job AS JB
                                INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId
                                Where JB.Id = @JobId";
            int val = 0;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@JobId", System.Data.SqlDbType.Int);
            param.Value = JbId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                val = (Convert.ToInt32(reader["FreightIndicatorId"]));
            }
            reader.Close();
            return val;
        }

        public static IQueryable<JobStatusModel> GetJoStatusValues()
        {
            List<JobStatusModel> list = new List<JobStatusModel>();
            JobStatusModel JobStatusModel = new JobStatusModel { JobStatusValue = 1, JobStatusValueName = "YES" };
            list.Add(JobStatusModel);
            return list.AsQueryable<JobStatusModel>();
        }


        public static List<JobStatusModel> GetJobStatusDetail(int JbId)
        {
            List<Dictionary<object, object>> JobStatusList = GetJobStatusList(JbId);
            List<Dictionary<object, object>> JobStatusH = new List<Dictionary<object,object>>();
            List<JobStatusModel> list = new List<JobStatusModel>();
            foreach(Dictionary<object, object> JobStatus in JobStatusList)
            {
                JobStatusH = GetJobStatusHistory(JbId, Convert.ToInt32(JobStatus["Id"]));
                int a = 0;
                string b ="";
                string c ="";
                string d ="";
                foreach (var StatusH in JobStatusH)
                {
                    a = StatusH["JobStatusValue"] == DBNull.Value ? 0 : Convert.ToInt32(StatusH["JobStatusValue"]);
                    b = StatusH["DateCreated"] == DBNull.Value ? "" : Convert.ToString(StatusH["DateCreated"]);
                    c = StatusH["Employee"] == DBNull.Value ? "" : Convert.ToString(StatusH["Employee"]);
                    d = StatusH["Remarks"] == DBNull.Value ? "" : Convert.ToString(StatusH["Remarks"]);
                }
                    JobStatusModel JoBStatusList = new JobStatusModel
                    {
                        StatusId = Convert.ToInt32(JobStatus["Id"]),
                        StatusName = JobStatus["Name"].ToString(),
                        JobStatusValue = a,
                        DateCreated = b,
                        Employee = c,
                        Remarks = d,
                    };
                list.Add(JoBStatusList);
            }
            return list;
        }

        public static List<PaymentModel> GetPaymentStatusDetail(int JbId)
        {
            int FI = GetFreigtIndicator(JbId);
            List<Dictionary<object, object>> PaymentStatusList = GetPaymentStatusList(JbId);
            List<Dictionary<object, object>> PaymentStatusH = new List<Dictionary<object, object>>();
            List<PaymentModel> list = new List<PaymentModel>();
            foreach (Dictionary<object, object> PaymentStatus in PaymentStatusList)
            {
                PaymentStatusH = GetPaymentStatusHistory(JbId, Convert.ToInt32(PaymentStatus["Id"]));
                string a = "";
                string b = "";
                string c = "";
                int d = 0;
                string e = "";
                string f = "";
                foreach (var StatusH in PaymentStatusH)
                {
                    a = StatusH["DocumentNo"] == DBNull.Value ? "" : Convert.ToString(StatusH["DocumentNo"]);
                    b = StatusH["UpdatedDate"] == DBNull.Value ? "" : Convert.ToString(StatusH["UpdatedDate"]);
                    c = StatusH["Employee"] == DBNull.Value ? "" : Convert.ToString(StatusH["Employee"]);
                    d = StatusH["PaymentStat"] == DBNull.Value ? 0 : Convert.ToInt32(StatusH["PaymentStat"]);
                    e = StatusH["PaymentTypesId"] == DBNull.Value ? "" : Convert.ToString(StatusH["PaymentTypesId"]);
                    f = StatusH["FreightIndicatorId"] == DBNull.Value ? "" : Convert.ToString(StatusH["FreightIndicatorId"]);
                }
                if (e=="1" && f=="1")
                {
                }else{
                    PaymentModel Payment = new PaymentModel
                    {
                        PaymentTypeId = Convert.ToInt32(PaymentStatus["Id"]),
                        PaymentName = PaymentStatus["Name"].ToString(),
                        DocumentNo = a,
                        UpdatedDate = b,
                        UpdatedBy = c,
                        PaymentStatus = d,
                        FreightIndicator = FI,
                    };
                    list.Add(Payment);
                }
            }
            return list;
        }
    }

    public class Commodity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetCommodityType()
        {
            String sql = @"SELECT Id,Name FROM TypeOfCommodity;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }
    }

    public class JobPriority
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetJobPriority()
        {
            String sql = @"SELECT Id,Name FROM JobPriority;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }
    }

    public class ClearanceModel
    {
        public DateTime? ShiftingRequestedDate { get; set; }
        public DateTime? ClearanceDate { get; set; }
        public AutoComplete ClearancePartyId { get; set; }
        public AutoComplete DeliveredBy { get; set; }
        public int ClearanceModeId { get; set; }
        public int ClearanceShiftId { get; set; }
        public int ClearancePortId { get; set; }
        public string ClearanceRemarks { get; set; }
        public string DeliveryPlace { get; set; }
        public string AssignStaffMCS { get; set; }
        public string AssignStaffMPL { get; set; }
        public string AssignStaffOffice { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int NoOfClearedPackage { get; set; }
        public int NoOfDeliveredPackage { get; set; }
        public int NoOfDamagePackage { get; set; }
       // public int VehicleId { get; set; }
        public AutoComplete VehicleId { get; set; }

        public ClearanceModel()
        {
            AutoComplete ClearancePartyId = new AutoComplete();
            AutoComplete DeliveredBy = new AutoComplete();
            AutoComplete VehicleId = new AutoComplete();
        }

        public static Dictionary<String, String> GetClearanceMode()
        {
            String sql = @"SELECT Id,Name FROM ClearanceMode;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<String, String> GetClearanceShift()
        {
            String sql = @"SELECT Id,Name FROM ClearanceShift;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }
        public static Dictionary<String, String> GetClearancePort()
        {
            String sql = @"SELECT Id,Name FROM ClearancePort;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<object, object> GetDOInfo(int JobId)
        {
            /*string sql = @"SELECT   SP.Name AS Shipper
		                         ,CT.Name AS Customer
		                        ,JB.JobNo
		                        ,(Select Top 1 DocumentNo from Payments where HouseBLId = JB.HouseBLId and PaymentTypesId = 2 order by UpdatedDate DESC) AS InvoiceNo
	                            ,HBL.HouseBL
,HBL.Id
		                        ,HBL.NoOfPackage
		                        ,CONCAT(JB.NoOfClearedPackage,'/',HBL.NoOfPackage) AS ClearedPKG
		                        ,CONCAT(JB.NoOfDeliveredPackage,'/',HBL.NoOfPackage) AS DeliveredPKG
		                        ,Convert(nvarchar(10),JB.DeliveryDate,103) AS DeliveryDate
		                        ,JB.DeliveryPlace
		                        --,CV.Name AS Vehicle
,CONCAT(CV.Name,'<br>',CV.RegNo,'<br>',CV.ContactPersonal,' ',CV.ContactNo) AS Vehicle
		                        ,CP.Name AS ClearedParty
		                        ,DP.Name AS DeliveredBy
		                        ,JB.ReceivedBy
,HBL.[Description]
                        FROM Job AS JB
                        INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId
                        LEFT JOIN ClearanceVehicle AS CV ON CV.Id = JB.ClearanceVehicleId
                        LEFT JOIN Party AS SP ON SP.Id = HBL.ShipperId
                        LEFT JOIN Party AS CT ON CT.Id = HBL.CustomerId
                        LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
                        LEFT JOIN Party AS DP ON DP.Id = JB.DeliveredBy
                        WHERE JB.Id = @JobId";*/
            string sql = @"SELECT   --SP.Name AS Shipper
		                         --,CT.Name AS Customer
CONCAT(SP.Name,'<br>', SP.RegNo,'<br>',ShipperAdd.Address1,' ',ShipperNT.Name,'<br>','Tel: ', ShipperCT.Tel,' Fax: ',ShipperCT.Fax,' Email: ', ShipperCT.Email) AS Shipper
,CONCAT(CT.Name,'<br>', CT.RegNo,'<br>',ConsigneeAdd.Address1,' ',ConsigneeNT.Name,'<br>','Tel: ', ConsigneeCT.Tel,' Fax: ',ConsigneeCT.Fax,' Email: ', ConsigneeCT.Email) AS Customer
		                        ,JB.JobNo
		                        ,(Select Top 1 DocumentNo from Payments where HouseBLId = JB.HouseBLId and PaymentTypesId = 2 order by UpdatedDate DESC) AS InvoiceNo
	                            ,HBL.HouseBL
,HBL.Id
		                        ,HBL.NoOfPackage
		                        ,CONCAT(JB.NoOfClearedPackage,'/',HBL.NoOfPackage) AS ClearedPKG
		                        ,CONCAT(JB.NoOfDeliveredPackage,'/',HBL.NoOfPackage) AS DeliveredPKG
,CONCAT(JB.NoOfDamagePackage,'/',HBL.NoOfPackage) AS DamagePKG
		                        ,Convert(nvarchar(10),JB.DeliveryDate,103) AS DeliveryDate
		                        ,JB.DeliveryPlace
		                        ,CONCAT(CV.Name,'<br>',CV.RegNo,'<br>',CV.ContactPersonal,' ',CV.ContactNo) AS Vehicle
		                       -- ,CP.Name AS ClearedParty
		                       -- ,DP.Name AS DeliveredBy
,CONCAT(CP.Name,'<br>', CP.RegNo,'<br>',CPAdd.Address1,' ',CPNT.Name,'<br>','Tel: ', CPCT.Tel,' Fax: ',CPCT.Fax,' Email: ', CPCT.Email) AS ClearedParty
,CONCAT(DP.Name,'<br>', DP.RegNo,'<br>',DPAdd.Address1,' ',DPNT.Name,'<br>','Tel: ', DPCT.Tel,' Fax: ',DPCT.Fax,' Email: ', DPCT.Email) AS DeliveredBy
		                        ,JB.ReceivedBy
,HBL.[Description]
                        FROM Job AS JB
                        INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId
                        LEFT JOIN ClearanceVehicle AS CV ON CV.Id = JB.ClearanceVehicleId
                        LEFT JOIN Party AS SP ON SP.Id = HBL.ShipperId
							    LEFT JOIN [Address] AS ShipperAdd ON ShipperAdd.Id = SP.AddressId
	                            LEFT JOIN Nationality AS ShipperNT ON ShipperNT.Id = ShipperAdd.NationalityId
	                            LEFT JOIN Contact AS ShipperCT ON ShipperCT.Id = SP.ContactId

                        LEFT JOIN Party AS CT ON CT.Id = HBL.CustomerId
							    LEFT JOIN [Address] AS ConsigneeAdd ON ConsigneeAdd.Id = CT.AddressId
	                            LEFT JOIN Nationality AS ConsigneeNT ON ConsigneeNT.Id = ConsigneeAdd.NationalityId
	                            LEFT JOIN Contact AS ConsigneeCT ON ConsigneeCT.Id = CT.ContactId


                        LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
								LEFT JOIN [Address] AS CPAdd ON CPAdd.Id = CP.AddressId
	                            LEFT JOIN Nationality AS CPNT ON CPNT.Id = CPAdd.NationalityId
	                            LEFT JOIN Contact AS CPCT ON CPCT.Id = CP.ContactId

                        LEFT JOIN Party AS DP ON DP.Id = JB.DeliveredBy
								LEFT JOIN [Address] AS DPAdd ON DPAdd.Id = DP.AddressId
	                            LEFT JOIN Nationality AS DPNT ON DPNT.Id = DPAdd.NationalityId
	                            LEFT JOIN Contact AS DPCT ON DPCT.Id = DP.ContactId

                        WHERE JB.Id = @JobId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = JobId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> Dictionary = new Dictionary<object, object>();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return Dictionary;
        }
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public AutoComplete VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int VCustomerId { get; set; }
        public string VContactPersonal { get; set; }
        public string VContactNo { get; set; }
        public string VRegNo { get; set; }
        public int VCountryId { get; set; }

        public Vehicle()
        {
            AutoComplete VehicleId = new AutoComplete();
        }

        public static Dictionary<String, String> GetVehicle()
        {
            String sql = @"SELECT Id,Name FROM ClearanceVehicle;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null)
        {
            string sql = @"SELECT Id, Concat(Name, ' - ',RegNo) AS Name FROM 
                                    ClearanceVehicle
                                    WHERE 
                                    (
                                    RegNo like concat('%',@query,'%')
                                    OR
                                    Name like concat('%',@query,'%')
                                    ) ";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> Vehiclelist = new List<Dictionary<object, object>>();

            SqlParameter param;

            param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                dictionary.Add("Id", Convert.ToInt32(reader["Id"].ToString()));
                dictionary.Add("Name", (reader["Name"].ToString()));
                Vehiclelist.Add(dictionary);
            }
            reader.Close();
            return Vehiclelist;
        }

        public bool SaveVehicle()
        {
            string sql = @"INSERT INTO [dbo].[ClearanceVehicle]
                                   ([Name]
                                   ,[CustomerId]
                                   ,[ContactPersonal]
                                   ,[ContactNo]
                                   ,[CountryId]
                                   ,[RegNo])
                             VALUES
                                   (@Name
                                   ,@CustomerId
                                   ,@ContactPersonal
                                   ,@ContactNo
                                   ,@CountryId
                                   ,@RegNo)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VehicleName) ? (object)DBNull.Value : VehicleName;
            list.Add(param);
            /*param = new SqlParameter("@CustomerId", DbType.Int32);
            param.Value = VCustomerId;
            list.Add(param);*/
            param = new SqlParameter("@CustomerId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VCustomerId > 0)
            {
                param.Value = VCustomerId;
            }
            list.Add(param);
            param = new SqlParameter("@ContactPersonal", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VContactPersonal) ? (object)DBNull.Value : VContactPersonal;
            list.Add(param);
            param = new SqlParameter("@ContactNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VContactNo) ? (object)DBNull.Value : VContactNo;
            list.Add(param);
            /*param = new SqlParameter("@CountryId", DbType.Int32);
            param.Value = VCountryId;
            list.Add(param);*/
            param = new SqlParameter("@CountryId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VCountryId > 0)
            {
                param.Value = VCountryId;
            }
            list.Add(param);
            param = new SqlParameter("@RegNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VRegNo) ? (object)DBNull.Value : VRegNo;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }
        public bool UpdateVehicle()
        {
            string sql = @"UPDATE [dbo].[ClearanceVehicle]
                           SET [Name] = @Name
                              ,[CustomerId] = @CustomerId
                              ,[ContactPersonal] = @ContactPersonal
                              ,[ContactNo] = @ContactNo
                              ,[CountryId] = @CountryId
                              ,[RegNo] = @RegNo
                         WHERE Id =@Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = Id;
            list.Add(param);
            param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VehicleName) ? (object)DBNull.Value : VehicleName;
            list.Add(param);
            param = new SqlParameter("@CustomerId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VCustomerId > 0)
            {
                param.Value = VCustomerId;
            }
            list.Add(param);
            param = new SqlParameter("@ContactPersonal", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VContactPersonal) ? (object)DBNull.Value : VContactPersonal;
            list.Add(param);
            param = new SqlParameter("@ContactNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VContactNo) ? (object)DBNull.Value : VContactNo;
            list.Add(param);
            param = new SqlParameter("@CountryId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VCountryId > 0)
            {
                param.Value = VCountryId;
            }
            list.Add(param);
            param = new SqlParameter("@RegNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(VRegNo) ? (object)DBNull.Value : VRegNo;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }
        public Dictionary<object, object> GetSelectedVehicle(int VehicleId)
        {
            string sql = @"SELECT Name,
	                            CustomerId,
	                            CountryId,
	                            ContactPersonal,
	                            ContactNo,
	                            RegNo
                        FROM ClearanceVehicle WHERE Id=@VehicleId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@VehicleId", DbType.Int32);
            param.Value = VehicleId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> Dictionary = new Dictionary<object, object>();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                reader.Close();
                return Dictionary;
            }
            reader.Close();
            return null;
        }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public string ContactPersonal { get; set; }
        public string ContactNo { get; set; }
        public int CountryId { get; set; }
        public string Location { get; set; }

        public static Dictionary<String, String> GetProject()
        {
            String sql = @"SELECT Id,Name FROM Project;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null)
        {
            string sql = @"SELECT Id,Name FROM 
                                    Project
                                    WHERE 
                                    (
                                    Location like concat('%',@query,'%')
                                    OR
                                    Name like concat('%',@query,'%')
                                    ) ";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> Projectlist = new List<Dictionary<object, object>>();

            SqlParameter param;

            param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                dictionary.Add("Id", Convert.ToInt32(reader["Id"].ToString()));
                dictionary.Add("Name", (reader["Name"].ToString()));
                Projectlist.Add(dictionary);
            }
            reader.Close();
            return Projectlist;
        }

        public bool SaveProject()
        {
            string sql = @"INSERT INTO [dbo].[Project]
                                   ([Name]
                                   ,[CustomerId]
                                   ,[ContactPersonal]
                                   ,[ContactNo]
                                   ,[CountryId]
                                   ,[Location])
                             VALUES
                                   (@Name
                                   ,@CustomerId
                                   ,@ContactPersonal
                                   ,@ContactNo
                                   ,@CountryId
                                   ,@Location)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Name) ? (object)DBNull.Value : Name;
            list.Add(param);
            param = new SqlParameter("@CustomerId", DbType.Int32);
            param.Value = CustomerId;
            list.Add(param);
            param = new SqlParameter("@ContactPersonal", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ContactPersonal) ? (object)DBNull.Value : ContactPersonal;
            list.Add(param);
            param = new SqlParameter("@ContactNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ContactNo) ? (object)DBNull.Value : ContactNo;
            list.Add(param);
            param = new SqlParameter("@CountryId", DbType.Int32);
            param.Value = CountryId;
            list.Add(param);
            param = new SqlParameter("@Location", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Location) ? (object)DBNull.Value : Location;
            list.Add(param);      
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool UpdateProject()
        {
            string sql = @"UPDATE [dbo].[Project]
                           SET [Name] = @Name
                              ,[CustomerId] = @CustomerId
                              ,[ContactPersonal] = @ContactPersonal
                              ,[ContactNo] = @ContactNo
                              ,[CountryId] = @CountryId
                              ,[Location] = @Location
                         WHERE Id =@Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = Id;
            list.Add(param);
            param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Name) ? (object)DBNull.Value : Name;
            list.Add(param);
            param = new SqlParameter("@CustomerId", DbType.Int32);
            param.Value = CustomerId;
            list.Add(param);
            param = new SqlParameter("@ContactPersonal", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ContactPersonal) ? (object)DBNull.Value : ContactPersonal;
            list.Add(param);
            param = new SqlParameter("@ContactNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ContactNo) ? (object)DBNull.Value : ContactNo;
            list.Add(param);
            param = new SqlParameter("@CountryId", DbType.Int32);
            param.Value = CountryId;
            list.Add(param);
            param = new SqlParameter("@Location", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Location) ? (object)DBNull.Value : Location;
            list.Add(param);
            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }

        public Dictionary<object, object> GetSelectedProject(int ProjectId)
        {
            string sql = @"SELECT Pro.Name,
		                    Pro.CustomerId,
		                    Pro.CountryId,
		                    Pro.ContactPersonal,
		                    Pro.ContactNo,
		                    Pro.Location,
		                    Par.Name AS OwnerName
	                    FROM Project AS Pro 
	                    INNER JOIN Party AS Par On Par.Id = Pro.CustomerId
	                    WHERE Pro.Id=@ProjectId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ProjectId", DbType.Int32);
            param.Value = ProjectId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> Dictionary = new Dictionary<object, object>();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                reader.Close();
                return Dictionary;
            }
            reader.Close();
            return null;
        }

    }
}