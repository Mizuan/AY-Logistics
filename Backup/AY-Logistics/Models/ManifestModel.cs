using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MyDBAccess;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

namespace AYLogistics.Models
{
    public class ManifestModel
    {
        public int ManifestId { get; set; }
        public string VoyageNo { get; set; }
        public AutoComplete VesselId { get; set; }
        public AutoComplete DOAgent { get; set; }
        public AutoComplete DOAgentL { get; set; }
        public int ModeofShipment { get; set; }
        public int Nationality { get; set; }
        public AutoComplete PortOfDeparture { get; set; }
        public DateTime? DateOfDeparture { get; set; }
        public AutoComplete PortOfDestination { get; set; }
        public DateTime? DateOfArrival { get; set; }
        public string MasterBLno { get; set; }
        public string MasterName { get; set; }
        public int TotalNoOfContainer { get; set; }
        public int TotalNoOfHBL { get; set; }
        public int TotalNoOfPackages { get; set; }
        public double NetTonnage { get; set; }
        public double GrossTonnage { get; set; }
        public int shipperId { get; set; }
        public int ShipmentStatusId { get; set; }
        public int ShipmentId { get; set; }
        public string MFNumber { get; set; }
        public int HBLId { get; set; }
        public int CustomOfficeId { get; set; }
        public int EntryType { get; set; }


        public Vessel VesselModel { get; set; }
        public ModeofShipment ModeofShipmentModel { get; set; }
        public Port PortOfDepartureModel { get; set; }
        public Port PortOfDestinationModel { get; set; }
        public Party PartyModel { get; set; }
        public List<HouseBLModel> HouseBLItems { get; set; }
        public JobModel Job { get; set; }
        public Project ProjectModel { get; set; }
        public Vehicle VehicleModel { get; set; }
        public ClearanceModel Clearance { get; set; }


        public ManifestModel()
        {
            VesselModel = new Vessel();
            ModeofShipmentModel = new ModeofShipment();
            PortOfDepartureModel = new Port();
            PortOfDestinationModel = new Port();
            PartyModel = new Party();
            HouseBLItems = new List<HouseBLModel>();
            Job = new JobModel();
            ProjectModel = new Project();
            VehicleModel = new Vehicle();
            Clearance = new ClearanceModel();
            AutoComplete PortOfDeparture = new AutoComplete();
            AutoComplete PortOfDestination = new AutoComplete();
            AutoComplete VesselId = new AutoComplete();
        }

        public ManifestModel(int JbId)
        {
            VesselModel = new Vessel();
            ModeofShipmentModel = new ModeofShipment();
            PortOfDepartureModel = new Port();
            PortOfDestinationModel = new Port();
            PartyModel = new Party();
            HouseBLItems = new List<HouseBLModel>();
            Job = new JobModel(JbId);
            ProjectModel = new Project();
            VehicleModel = new Vehicle();
            Clearance = new ClearanceModel();
            AutoComplete PortOfDeparture = new AutoComplete();
            AutoComplete PortOfDestination = new AutoComplete();
            AutoComplete VesselId = new AutoComplete();
        }

        public static string FormatManifestNumber()
        {
            string FormatManifestNumber = GetMenifestNumberFormat();
            return FormatManifestNumber.Replace("%S%", GetMenifestSequence().ToString()).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
        }

        public static string GetMenifestNumberFormat()
        {
            string sql = @"SELECT Format FROM  Numbering WHERE Id=1";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }

        public static int GetMenifestSequence()
        {
            string sql = @"SELECT Sequence FROM  Numbering WHERE Id=1";
            int sequence = 0;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                sequence = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            return sequence;
        }

        public static Dictionary<object, object> FormatHBLNumber(int DOAId)
        {
            string Format = GetHBLNumberFormat();
            string CompanyCode = "";
            string HBLNumber ="";
            if (DOAId == 1017)
            {
                CompanyCode = "ASF";
            }
            else if (DOAId == 7413)
            {
                CompanyCode = "MSL";
            }
            else if (DOAId == 5749)
            {
                CompanyCode = "ASL";
            }
            HBLNumber = Format.Replace("%S%", GetHBLSequence().ToString()).Replace("CODE", CompanyCode).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
            Dictionary<object, object> List = new Dictionary<object, object>();
            List.Add("HBLNumber", HBLNumber);
            return List;
        }
        public static string GetHBLNumberFormat()
        {
            string sql = @"SELECT Format FROM  Numbering WHERE Id=6";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }
        public static int GetHBLSequence()
        {
            string sql = @"SELECT Sequence FROM  Numbering WHERE Id=6";
            int sequence = 0;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                sequence = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            return sequence;
        }

        public static string FormatJobNumber(int MS)
        {
            string FormatManifestNumber = "";
            if (MS == 1)
            {
                FormatManifestNumber = GetJobSEANumberFormat();
            }
            if (MS == 2)
            {
                FormatManifestNumber = GetJobAIRNumberFormat();
            }
            if (MS == 3)
            {
                FormatManifestNumber = GetJobAIRNumberFormat();
            }
            return FormatManifestNumber.Replace("%S%", GetJobSequence().ToString()).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
        }
        public static string GetJobSEANumberFormat()
        {
            string sql = @"SELECT Format FROM  Numbering WHERE Id=2";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }

        public static string GetJobAIRNumberFormat()
        {
            string sql = @"SELECT Format FROM  Numbering WHERE Id=3";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }

        public static string GetJobPOSTNumberFormat()
        {
            string sql = @"SELECT Format FROM  Numbering WHERE Id=4";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }

        public static int GetJobSequence()
        {
            string sql = @"SELECT Sequence FROM  Numbering WHERE Id=2";
            int sequence = 0;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                sequence = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            return sequence;
        }

        public bool SaveManifest()
        {
            string ManifestNumber = FormatManifestNumber();
            String sql = @"DECLARE @ManifestLastId INT
                           DECLARE @ShipmentId INT

                            INSERT INTO [dbo].[Manifest]
                                       ([Number]
                                       ,[CreatedDate]
                                       ,[CreatedBy])
                                 VALUES
                                       (@Number
                                       ,GETDATE()
                                       ,@EmployeeId )

                            SELECT @ManifestLastId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[Shipment]
                                       ([ManifestId]
                                       ,[VoyageNo]
                                       ,[VesselId]
                                       ,[ModeofShipmentId]
                                       ,[MasterBL]
                                       ,[PortOfDeparture]
                                       ,[DateDeparture]
                                       ,[PortOfDestination]
                                       ,[DateArrival]
                                       ,[MasterName]
                                      -- ,[TotalNoOfContainer]
                                      -- ,[TotalNoOfBL]
                                      -- ,[TotalNoOfPackages]
                                       ,[NETtonnage]
                                       ,[GROSStonnage]
                                       ,[ShippingAgentId]
,[DeliveryAgentId]
                                       ,[DateEntered]
                                       ,[CustomOfficeId]
                                       ,[EntryTypeId])
                                 VALUES
                                       (@ManifestLastId
                                       ,@VoyageNo
                                       ,@VesselId
                                       ,@ModeofShipmentId
                                       ,@MasterBL
                                       ,@PortOfDeparture
                                       ,@DateDeparture
                                       ,@PortOfDestination
                                       ,@DateArrival
                                       ,@MasterName
                                      -- ,@TotalNoOfContainer
                                      -- ,@TotalNoOfBL
                                      -- ,@TotalNoOfPackages
                                       ,@NETtonnage
                                       ,@GROSStonnage
                                       ,@ShippingAgentId
,@DeliveryAgentId
                                       ,GETDATE()
                                       ,@CustomOfficeId
                                       ,@EntryType)
                            SELECT @ShipmentId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[ShipmentStatusHistory]
                                       ([ShipmentId]
                                       ,[ShipmentStatusId]
                                       ,[DateCreated]
                                       ,[EmployeeId])
                                 VALUES
                                       (@ShipmentId
                                       ,1
                                       ,GETDATE()
                                       ,@EmployeeId)";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Number", DbType.String);
            param.Value = ManifestNumber;
            List.Add(param);
            param = new SqlParameter("@EntryType", DbType.Int32);
            param.Value = EntryType;
            List.Add(param);
            param = new SqlParameter("@VoyageNo", DbType.String);
            param.Value = VoyageNo;
            List.Add(param);
            param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VesselId != null)
            {
                param.Value = VesselId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ModeofShipmentId", DbType.Int32);
            param.Value = ModeofShipment;
            List.Add(param);
            param = new SqlParameter("@MasterBL", DbType.String);
            param.Value = DBNull.Value;
            if (MasterBLno != "")
            {
                param.Value = MasterBLno.Replace(" ", String.Empty);
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDeparture", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDeparture != null)
            {
                param.Value = PortOfDeparture.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateDeparture", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfDeparture != null)
            {
                param.Value = DateOfDeparture;
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDestination", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDestination != null)
            {
                param.Value = PortOfDestination.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateArrival", DbType.DateTime);
            param.Value = DateOfArrival;
            List.Add(param);
            param = new SqlParameter("@ShippingAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgent != null)
            {
                param.Value = DOAgent.id;
            }
            List.Add(param);
            param = new SqlParameter("@DeliveryAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgentL != null)
            {
                param.Value = DOAgentL.id;
            }
            List.Add(param);
            param = new SqlParameter("@MasterName", DbType.String);
            param.Value = MasterName;
            List.Add(param);
            /*param = new SqlParameter("@TotalNoOfContainer", DbType.Int32);
            param.Value = TotalNoOfContainer;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfBL", DbType.Int32);
            param.Value = TotalNoOfHBL;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfPackages", DbType.Int32);
            param.Value = TotalNoOfPackages;
            List.Add(param);*/
            param = new SqlParameter("@NETtonnage", DbType.Decimal);
            param.Value = NetTonnage;
            List.Add(param);
            param = new SqlParameter("@GROSStonnage", DbType.Decimal);
            param.Value = GrossTonnage;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@CustomOfficeId", DbType.Int32);
            param.Value = CustomOfficeId;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveXMLasManifest()
        {
            string ManifestNumber = FormatManifestNumber();
            String sql = @"DECLARE @ManifestLastId INT
                           DECLARE @ShipmentId INT

                            INSERT INTO [dbo].[Manifest]
                                       ([Number]
                                       ,[CreatedDate]
                                       ,[CreatedBy])
                                 VALUES
                                       (@Number
                                       ,GETDATE()
                                       ,@EmployeeId )

                            SELECT @ManifestLastId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[Shipment]
                                       ([ManifestId]
                                       ,[VoyageNo]
                                       ,[ModeofShipmentId]
                                       ,[MasterBL]
                                       ,[DateDeparture]
                                       ,[DateArrival]
                                       ,[NETtonnage]
                                       ,[GROSStonnage]
                                       ,[DateEntered]
                                       ,[CustomOfficeId]
                                       ,[EntryTypeId])
                                 VALUES
                                       (@ManifestLastId
                                       ,@VoyageNo
                                       ,@ModeofShipmentId
                                       ,@MasterBL
                                       ,@DateDeparture
                                       ,@DateArrival
                                       ,@NETtonnage
                                       ,@GROSStonnage
                                       ,GETDATE()
                                       ,@CustomOfficeId
                                       ,1)

                            SELECT @ShipmentId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[ShipmentStatusHistory]
                                       ([ShipmentId]
                                       ,[ShipmentStatusId]
                                       ,[DateCreated]
                                       ,[EmployeeId])
                                 VALUES
                                       (@ShipmentId
                                       ,1
                                       ,GETDATE()
                                       ,@EmployeeId)";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Number", DbType.String);
            param.Value = ManifestNumber;
            List.Add(param);
            param = new SqlParameter("@VoyageNo", DbType.String);
            param.Value = VoyageNo;
            List.Add(param);
            param = new SqlParameter("@ModeofShipmentId", DbType.Int32);
            param.Value = ModeofShipment;
            List.Add(param);
            param = new SqlParameter("@MasterBL", DbType.String);
            param.Value = MasterBLno.Replace(" ", String.Empty);
            List.Add(param);
            param = new SqlParameter("@DateDeparture", DbType.DateTime);
            param.Value = DateOfDeparture;
            List.Add(param);
            param = new SqlParameter("@DateArrival", DbType.DateTime);
            param.Value = DateOfArrival;
            List.Add(param);
            param = new SqlParameter("@NETtonnage", DbType.Decimal);
            param.Value = NetTonnage;
            List.Add(param);
            param = new SqlParameter("@GROSStonnage", DbType.Decimal);
            param.Value = GrossTonnage;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@CustomOfficeId", DbType.Int32);
            param.Value = CustomOfficeId;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveShipment()
        {
            String sql = @"INSERT INTO [dbo].[Shipment]
                                       ([VoyageNo]
                                       ,[VesselId]
                                       ,[ModeofShipmentId]
                                       ,[MasterBL]
                                       ,[PortOfDeparture]
                                       ,[DateDeparture]
                                       ,[PortOfDestination]
                                       ,[DateArrival]
                                       ,[ShippingAgentId]
,[DeliveryAgentId]
                                       ,[MasterName]
                                     --  ,[TotalNoOfContainer]
                                     --  ,[TotalNoOfBL]
                                     --  ,[TotalNoOfPackages]
                                       ,[NETtonnage]
                                       ,[GROSStonnage]
                                       ,[DateEntered]
                                       ,[CustomOfficeId]
                                       ,[EntryTypeId])
                                 VALUES
                                       (@VoyageNo
                                       ,@VesselId
                                       ,@ModeofShipmentId
                                       ,@MasterBL
                                       ,@PortOfDeparture
                                       ,@DateDeparture
                                       ,@PortOfDestination
                                       ,@DateArrival
                                       ,@ShippingAgentId
,@DeliveryAgentId
                                       ,@MasterName
                                     --  ,@TotalNoOfContainer
                                     --  ,@TotalNoOfBL
                                     --  ,@TotalNoOfPackages
                                       ,@NETtonnage
                                       ,@GROSStonnage
                                       ,GETDATE()
                                       ,@CustomOfficeId
                                       ,2)";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@VoyageNo", DbType.String);
            param.Value = VoyageNo;
            List.Add(param);
            param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VesselId != null)
            {
                param.Value = VesselId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ModeofShipmentId", DbType.Int32);
            param.Value = ModeofShipment;
            List.Add(param);
            param = new SqlParameter("@MasterBL", DbType.String);
            param.Value = DBNull.Value;
            if (MasterBLno != "")
            {
                param.Value = MasterBLno.Replace(" ", String.Empty);
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDeparture", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDeparture != null)
            {
                param.Value = PortOfDeparture.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateDeparture", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfDeparture != null)
            {
                param.Value = DateOfDeparture;
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDestination", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDestination != null)
            {
                param.Value = PortOfDestination.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateArrival", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfArrival != null)
            {
                param.Value = DateOfArrival;
            }
            List.Add(param);
            param = new SqlParameter("@ShippingAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgent != null)
            {
                param.Value = DOAgent.id;
            }
            List.Add(param);
            param = new SqlParameter("@DeliveryAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgentL != null)
            {
                param.Value = DOAgentL.id;
            }
            List.Add(param);
            param = new SqlParameter("@MasterName", DbType.String);
            param.Value = MasterName;
            List.Add(param);
            /*param = new SqlParameter("@TotalNoOfContainer", DbType.Int32);
            param.Value = TotalNoOfContainer;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfBL", DbType.Int32);
            param.Value = TotalNoOfHBL;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfPackages", DbType.Int32);
            param.Value = TotalNoOfPackages;
            List.Add(param);*/
            param = new SqlParameter("@NETtonnage", DbType.Decimal);
            param.Value = NetTonnage;
            List.Add(param);
            param = new SqlParameter("@GROSStonnage", DbType.Decimal);
            param.Value = GrossTonnage;
            List.Add(param);
            param = new SqlParameter("@CustomOfficeId", DbType.Int32);
            param.Value = CustomOfficeId;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveJob(int LastBLId, int SPmode)
        {
            string JobNo = FormatJobNumber(SPmode);
            String sql = @"INSERT INTO [dbo].[Job]
                                       ([JobNo]
                                       ,[RegistrationDate]
                                       ,[JobPriorityId]
                                       ,[DateDischarge]
                                       ,[DateDemurrage]
                                       ,[TypeOfCommodityId]
                                       ,[HouseBLId]
,EmployeeId)
                                 VALUES
                                       (@JobNo
                                       ,@RegistrationDate
                                       ,@JobPriorityId
                                       ,@DateDischarge
                                       ,@DateDemurrage
                                       ,@TypeOfCommodityId
                                       ,@HouseBLId
,@EmployeeId)";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@JobNo", DbType.String);
            param.Value = JobNo;
            List.Add(param);
            param = new SqlParameter("@RegistrationDate", DbType.DateTime);
            param.Value = Job.RegistrationDate;
            List.Add(param);
            param = new SqlParameter("@JobPriorityId", DbType.Int32);
            param.Value = Job.JobPriorityId;
            List.Add(param);
            param = new SqlParameter("@DateDischarge", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.DischargeDate != null)
            {
                param.Value = Job.DischargeDate;
            }
            List.Add(param);
            param = new SqlParameter("@DateDemurrage", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.DemurrageDate != null)
            {
                param.Value = Job.DemurrageDate;
            }
            List.Add(param);
            param = new SqlParameter("@TypeOfCommodityId", DbType.Int32);
            param.Value = Job.CommodityTypeId;
            List.Add(param);
            param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = LastBLId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateJob()
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [RegistrationDate] = @RegistrationDate
                              ,[JobPriorityId] = @JobPriorityId
                              ,[DateDischarge] = @DateDischarge
                              ,[DateDemurrage] = @DateDemurrage
                              ,[TypeOfCommodityId] = @TypeOfCommodityId
                            --  ,[CNumber] = @CNumber
                              ,[RNumber] = @RNumber
,[RNumberReqDate] = @RNumberReqDate
,[ANumber] = @ANumber
,[ANumberReqDate] = @ANumberReqDate
                              ,[CustomerReference] = @CustomerReference
                         WHERE Id = @JobId";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = Job.JobId;
            List.Add(param);
            param = new SqlParameter("@RegistrationDate", DbType.DateTime);
            param.Value = Job.RegistrationDate;
            List.Add(param);
            param = new SqlParameter("@JobPriorityId", DbType.Int32);
            param.Value = Job.JobPriorityId;
            List.Add(param);
            param = new SqlParameter("@DateDischarge", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.DischargeDate != null)
            {
                param.Value = Job.DischargeDate;
            }
            List.Add(param);
            param = new SqlParameter("@DateDemurrage", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.DemurrageDate != null)
            {
                param.Value = Job.DemurrageDate;
            }
            List.Add(param);
            param = new SqlParameter("@TypeOfCommodityId", DbType.Int32);
            param.Value = Job.CommodityTypeId;
            List.Add(param);
            /*param = new SqlParameter("@CNumber", DbType.String);
            param.Value = Job.CNumber;
            List.Add(param);*/
            param = new SqlParameter("@RNumber", DbType.String);
            param.Value = Job.RNumber;
            List.Add(param);

            param = new SqlParameter("@RNumberReqDate", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.RNumberReqDate != null)
            {
                param.Value = Job.RNumberReqDate;
            }
            List.Add(param);

            param = new SqlParameter("@ANumber", DbType.String);
            param.Value = Job.ANumber;
            List.Add(param);

            param = new SqlParameter("@ANumberReqDate", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Job.ANumberReqDate != null)
            {
                param.Value = Job.ANumberReqDate;
            }
            List.Add(param);

            param = new SqlParameter("@CustomerReference", DbType.String);
            param.Value = Job.CustomerReference;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateClearance()
        {
            String sql = @"UPDATE [dbo].[Job]
                           SET [ClearanceDate] = @ClearanceDate
                              ,[ClearancePartyId] = @ClearancePartyId
                              ,[ClearanceModeId] = @ClearanceModeId
                              ,[ClearanceVehicleId] = @ClearanceVehicleId
                              ,[ClearanceShiftId] = @ClearanceShiftId
,[ClearancePortId] = @ClearancePortId
                              ,[DeliveryPlace] = @DeliveryPlace
,[AssignStaffMCS] = @AssignStaffMCS
,[AssignStaffMPL] = @AssignStaffMPL
,[AssignStaffOffice] = @AssignStaffOffice
                              ,[Remarks] = @Remarks
,[ShiftingRequestedDate] = @ShiftingRequestedDate
                         WHERE Id = @JobId";

            List<SqlParameter> List = new List<SqlParameter>();

            /*SqlParameter param = new SqlParameter("@ClearanceDate", DbType.DateTime);
            param.Value = Clearance.ClearanceDate;
            List.Add(param);*/

            SqlParameter param = new SqlParameter("@ClearanceDate", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Clearance.ClearanceDate != null)
            {
                param.Value = Clearance.ClearanceDate;
            }
            List.Add(param);


            param = new SqlParameter("@ClearancePartyId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Clearance.ClearancePartyId.id > 0)
            {
                param.Value = Clearance.ClearancePartyId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ClearanceModeId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Clearance.ClearanceModeId > 0)
            {
                param.Value = Clearance.ClearanceModeId;
            }
            List.Add(param);
            param = new SqlParameter("@ClearanceVehicleId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Clearance.VehicleId.id > 0)
            {
             param.Value = Clearance.VehicleId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ClearanceShiftId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Clearance.ClearanceShiftId > 0)
            {
                param.Value = Clearance.ClearanceShiftId;
            }
            List.Add(param);
            param = new SqlParameter("@ClearancePortId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Clearance.ClearancePortId > 0)
            {
                param.Value = Clearance.ClearancePortId;
            }
            List.Add(param);

            param = new SqlParameter("@DeliveryPlace", DbType.String);
            param.Value = Clearance.DeliveryPlace;
            List.Add(param);

            param = new SqlParameter("@AssignStaffMCS", DbType.String);
            param.Value = Clearance.AssignStaffMCS;
            List.Add(param);
            param = new SqlParameter("@AssignStaffMPL", DbType.String);
            param.Value = Clearance.AssignStaffMPL;
            List.Add(param);
            param = new SqlParameter("@AssignStaffOffice", DbType.String);
            param.Value = Clearance.AssignStaffOffice;
            List.Add(param);

            param = new SqlParameter("@Remarks", DbType.String);
            param.Value = Clearance.ClearanceRemarks;
            List.Add(param);
            param = new SqlParameter("@JobId", DbType.Int32);
            param.Value = Job.JobId;
            List.Add(param);
            param = new SqlParameter("@ShiftingRequestedDate", DbType.DateTime);
            param.Value = DBNull.Value;
            if (Clearance.ShiftingRequestedDate != null)
            {
                param.Value = Clearance.ShiftingRequestedDate;
            }
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateManifest()
        {
            String sql = @"UPDATE [dbo].[Shipment]
                               SET [VoyageNo] = @VoyageNo
                                  ,[VesselId] = @VesselId
                                  ,[ModeofShipmentId] = @ModeofShipmentId
                                  ,[MasterBL] = @MasterBL
                                  ,[PortOfDeparture] = @PortOfDeparture
                                  ,[DateDeparture] = @DateDeparture
                                  ,[PortOfDestination] = @PortOfDestination
                                  ,[DateArrival] = @DateArrival
                                  ,[ShippingAgentId] = @ShippingAgentId
                                  ,[DeliveryAgentId] = @DeliveryAgentId
                                  ,[MasterName] = @MasterName
                                 -- ,[TotalNoOfContainer] = @TotalNoOfContainer
                                 -- ,[TotalNoOfBL] = @TotalNoOfBL
                                 -- ,[TotalNoOfPackages] = @TotalNoOfPackages
                                  ,[NETtonnage] = @NETtonnage
                                  ,[GROSStonnage] = @GROSStonnage
                                  ,[CustomOfficeId] = @CustomOfficeId
                             WHERE ManifestId = @ManifestId";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ManifestId", DbType.Int32);
            param.Value = ManifestId;
            List.Add(param);
            param = new SqlParameter("@VoyageNo", DbType.String);
            param.Value = VoyageNo;
            List.Add(param);
            param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = VesselId.id;
            List.Add(param);
            param = new SqlParameter("@ModeofShipmentId", DbType.Int32);
            param.Value = ModeofShipment;
            List.Add(param);
            param = new SqlParameter("@MasterBL", DbType.String);
            param.Value = DBNull.Value;
            if (MasterBLno != "")
            {
                param.Value = MasterBLno.Replace(" ", String.Empty);
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDeparture", DbType.Int32);
            param.Value = PortOfDeparture.id;
            List.Add(param);
            param = new SqlParameter("@DateDeparture", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfDeparture != null)
            {
                param.Value = Convert.ToDateTime(DateOfDeparture);
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDestination", DbType.Int32);
            param.Value = PortOfDestination.id;
            List.Add(param);
            param = new SqlParameter("@DateArrival", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfArrival != null)
            {
                param.Value = Convert.ToDateTime(DateOfArrival);
            }
            List.Add(param);
            param = new SqlParameter("@ShippingAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgent == null)
            {
                param.Value = DBNull.Value;
            }
            else if (DOAgent.id > 0)
            {
                param.Value = DOAgent.id;
            }
            List.Add(param);
            param = new SqlParameter("@DeliveryAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgentL == null)
            {
                param.Value = DBNull.Value;
            }
            else if (DOAgentL.id > 0)
            {
                param.Value = DOAgentL.id;
            }
            List.Add(param);
            param = new SqlParameter("@MasterName", DbType.String);
            param.Value = MasterName;
            List.Add(param);
            /* param = new SqlParameter("@TotalNoOfContainer", DbType.Int32);
             param.Value = TotalNoOfContainer;
             List.Add(param);
             param = new SqlParameter("@TotalNoOfBL", DbType.Int32);
             param.Value = TotalNoOfHBL;
             List.Add(param);
             param = new SqlParameter("@TotalNoOfPackages", DbType.Int32);
             param.Value = TotalNoOfPackages;
             List.Add(param);*/
            param = new SqlParameter("@NETtonnage", DbType.Decimal);
            param.Value = NetTonnage;
            List.Add(param);
            param = new SqlParameter("@GROSStonnage", DbType.Decimal);
            param.Value = GrossTonnage;
            List.Add(param);
            param = new SqlParameter("@CustomOfficeId", DbType.Int32);
            param.Value = CustomOfficeId;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateShipment()
        {
            String sql = @"UPDATE [dbo].[Shipment]
                               SET [VoyageNo] = @VoyageNo
                                  ,[VesselId] = @VesselId
                                  ,[ModeofShipmentId] = @ModeofShipmentId
                                  ,[MasterBL] = @MasterBL
                                  ,[PortOfDeparture] = @PortOfDeparture
                                  ,[DateDeparture] = @DateDeparture
                                  ,[PortOfDestination] = @PortOfDestination
                                  ,[DateArrival] = @DateArrival
                                  ,[ShippingAgentId] = @ShippingAgentId
,[DeliveryAgentId] = @DeliveryAgentId
                                  ,[MasterName] = @MasterName
                                 -- ,[TotalNoOfContainer] = @TotalNoOfContainer
                                 -- ,[TotalNoOfBL] = @TotalNoOfBL
                                 -- ,[TotalNoOfPackages] = @TotalNoOfPackages
                                  ,[NETtonnage] = @NETtonnage
                                  ,[GROSStonnage] = @GROSStonnage
                                  ,[CustomOfficeId] = @CustomOfficeId
                             WHERE Id = @ShipmentId";

            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            List.Add(param);
            param = new SqlParameter("@VoyageNo", DbType.String);
            param.Value = VoyageNo;
            List.Add(param);
            param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = DBNull.Value;
            if (VesselId.id > 0)
            {
                param.Value = VesselId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ModeofShipmentId", DbType.Int32);
            param.Value = ModeofShipment;
            List.Add(param);
            param = new SqlParameter("@MasterBL", DbType.Int32);
            param.Value = DBNull.Value;
            if (MasterBLno != "")
            {
                param.Value = MasterBLno.Replace(" ", String.Empty);
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDeparture", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDeparture.id > 0)
            {
                param.Value = PortOfDeparture.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateDeparture", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfDeparture != null)
            {
                param.Value = DateOfDeparture;
            }
            List.Add(param);
            param = new SqlParameter("@PortOfDestination", DbType.Int32);
            param.Value = DBNull.Value;
            if (PortOfDestination.id > 0)
            {
                param.Value = PortOfDestination.id;
            }
            List.Add(param);
            param = new SqlParameter("@DateArrival", DbType.DateTime);
            param.Value = DBNull.Value;
            if (DateOfArrival != null)
            {
                param.Value = DateOfArrival;
            }
            List.Add(param);
            param = new SqlParameter("@ShippingAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgent == null)
            {
                param.Value = DBNull.Value;
            }
            else if (DOAgent.id > 0)
            {
                param.Value = DOAgent.id;
            }
            List.Add(param);
            param = new SqlParameter("@DeliveryAgentId", DbType.Int32);
            param.Value = DBNull.Value;
            if (DOAgentL == null)
            {
                param.Value = DBNull.Value;
            }
            else if (DOAgentL.id > 0)
            {
                param.Value = DOAgentL.id;
            }
            List.Add(param);
            param = new SqlParameter("@MasterName", DbType.String);
            param.Value = MasterName;
            List.Add(param);
            /*param = new SqlParameter("@TotalNoOfContainer", DbType.Int32);
            param.Value = TotalNoOfContainer;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfBL", DbType.Int32);
            param.Value = TotalNoOfHBL;
            List.Add(param);
            param = new SqlParameter("@TotalNoOfPackages", DbType.Int32);
            param.Value = TotalNoOfPackages;
            List.Add(param);*/
            param = new SqlParameter("@NETtonnage", DbType.Decimal);
            param.Value = NetTonnage;
            List.Add(param);
            param = new SqlParameter("@GROSStonnage", DbType.Decimal);
            param.Value = GrossTonnage;
            List.Add(param);
            param = new SqlParameter("@CustomOfficeId", DbType.Int32);
            param.Value = CustomOfficeId;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public static int GetLastShipmentId()
        {
            string sql = @"SELECT IDENT_CURRENT('Shipment')";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            int ShipmentId = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    ShipmentId = Convert.ToInt32(reader.GetValue(i));
                }
            }
            reader.Close();
            return ShipmentId;
        }

        public static int GetLastBLId()
        {
            string sql = @"SELECT IDENT_CURRENT('HouseBL')";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            int BLId = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    BLId = Convert.ToInt32(reader.GetValue(i));
                }
            }
            reader.Close();
            return BLId;
        }

        public static int GetLastJobId()
        {
            string sql = @"SELECT IDENT_CURRENT('Job')";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            int JobId = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    JobId = Convert.ToInt32(reader.GetValue(i));
                }
            }
            reader.Close();
            return JobId;
        }

        public bool SaveHouseBLItems(List<HouseBLModel> HouseBLItems, int LastShipmentId, int status = 1)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;

            for (int j = 0; j < HouseBLItems.Count; j++)
            {
                sql = @"DECLARE @HBLLastId INT
                            INSERT INTO [dbo].[HouseBL]
                                       ([ShipmentId]
                                       ,[HouseBL]
                                      -- ,[ContainerTypeId]
                                     --  ,[ContainerNo]
                                      -- ,[SealNo]
                                       ,[NoOfPackage]
                                       ,[FreightIndicatorId]
                                       ,[BLNatureId]
                                       ,[BLTypesId]
,[BLStateId]
                                       ,[ShippingMark]
                                       ,[ShipperId]
                                       ,[CustomerId]
                                       ,[NotifyPartyId]
                                      -- ,[DOAgentId]
                                       ,[Weight]
                                       ,[Measurement]
                                       ,[ProjectId]
                                       ,[PortOfLoading]
                                       ,[PortOfUnloading]
                                       ,[PortOfOrigin]
                                       ,[OriginalLoadingPort]
                                       ,[PortOfDelivery]
                                       ,[UltimateDestination]
                                       ,[Description])
                                 VALUES
                                       (@LastShipmentId
                                       ,@HouseBL
                                      -- ,@ContainerTypeId
                                      -- ,@ContainerNo
                                      -- ,@SealNo
                                       ,@NoOfPackage
                                       ,@FreightIndicatorId
                                       ,@BLNatureId
                                       ,@BLTypesId
,@BLStateId
                                       ,@ShippingMark
                                       ,@ShipperId
                                       ,@CustomerId
                                       ,@NotifyPartyId
                                    --   ,@DOAgentId
                                       ,@Weight
                                       ,@Measurement
                                       ,@ProjectId
                                       ,@PortOfLoading
                                       ,@PortOfUnloading
                                       ,@PortOfOrigin
                                       ,@OriginalLoadingPort
                                       ,@PortOfDelivery
                                       ,@UltimateDestination
                                       ,@Description)

                           SELECT @HBLLastId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[BLStatusHistory]
                                       ([BLStatusId]
                                       ,[HouseBLId]
                                       ,[DateCreated]
                                       ,[EmployeeId])
                                 VALUES
                                       (@BLStatusId
                                       ,@HBLLastId
                                       ,GETDATE()
                                       ,@EmployeeId)";

                List = new List<SqlParameter>();
                param = new SqlParameter("@LastShipmentId", DbType.Int32);
                param.Value = LastShipmentId;
                List.Add(param);

                param = new SqlParameter("@HouseBL", DbType.String);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].HouseBLno != "")
                {
                    param.Value = HouseBLItems[j].HouseBLno.Replace(" ", String.Empty);
                }
                List.Add(param);
                param = new SqlParameter("@NoOfPackage", DbType.Int32);
                param.Value = HouseBLItems[j].TotalBLPackages;
                List.Add(param);
                param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                param.Value = HouseBLItems[j].FreightIndicator;
                List.Add(param);
                param = new SqlParameter("@BLNatureId", DbType.Int32);
                param.Value = HouseBLItems[j].BLNature;
                List.Add(param);
                param = new SqlParameter("@BLTypesId", DbType.Int32);
                param.Value = HouseBLItems[j].BLTypeId;
                List.Add(param);
                param = new SqlParameter("@BLStateId", DbType.Int32);
                param.Value = HouseBLItems[j].BLStateId;
                List.Add(param);
                param = new SqlParameter("@ShippingMark", DbType.String);
                param.Value = HouseBLItems[j].ShippingMark;
                List.Add(param);
                param = new SqlParameter("@ShipperId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ShipperNew != null)
                {
                    param.Value = HouseBLItems[j].ShipperNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@CustomerId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].CustomerNew != null)
                {
                    param.Value = HouseBLItems[j].CustomerNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@NotifyPartyId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].NotifyPartyNew != null)
                {
                    param.Value = HouseBLItems[j].NotifyPartyNew.id;
                }
                List.Add(param);
                /* param = new SqlParameter("@DOAgentId", DbType.Int32);
                 param.Value = DBNull.Value;
                 if (HouseBLItems[j].DOAgent != null)
                 {
                     param.Value = HouseBLItems[j].DOAgent.id;
                 }
                 List.Add(param);*/
                param = new SqlParameter("@Weight", DbType.Decimal);
                param.Value = HouseBLItems[j].Weight;
                List.Add(param);
                param = new SqlParameter("@Measurement", DbType.Decimal);
                param.Value = HouseBLItems[j].Measurement;
                List.Add(param);
                param = new SqlParameter("@ProjectId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ProjectId != null)
                {
                    param.Value = HouseBLItems[j].ProjectId.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfLoading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfLoading != null)
                {
                    param.Value = HouseBLItems[j].PortOfLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfUnloading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfUnLoading != null)
                {
                    param.Value = HouseBLItems[j].PortOfUnLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfOrigin", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfOrigin != null)
                {
                    param.Value = HouseBLItems[j].PortOfOrigin.id;
                }
                List.Add(param);
                param = new SqlParameter("@OriginalLoadingPort", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].OriginalLoadingPort != null)
                {
                    param.Value = HouseBLItems[j].OriginalLoadingPort.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfDelivery", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfDelivery != null)
                {
                    param.Value = HouseBLItems[j].PortOfDelivery.id;
                }
                List.Add(param);
                param = new SqlParameter("@UltimateDestination", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].UltimateDestination != null)
                {
                    param.Value = HouseBLItems[j].UltimateDestination.id;
                }
                List.Add(param);
                param = new SqlParameter("@Description", DbType.String);
                param.Value = HouseBLItems[j].Description;
                List.Add(param);
                param = new SqlParameter("@BLStatusId", DbType.Int32);
                param.Value = status;
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);

                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // cotainer item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < HouseBLItems[j].ContainerItems.Count; k++)
                    {
                        if (HouseBLItems[j].ContainerItems[k].PackingType == 2)
                        {
                            sql = @"INSERT INTO [dbo].[ContainerInfo]
                                           ([HouseBLId]
                                           ,[PackingId]
                                           ,[TypeofPackageId]
                                           ,[CNoOfPackage])
                                     VALUES
                                           ((SELECT IDENT_CURRENT('HouseBL'))
                                           ,@PackingId
                                           ,@TypeofPackageId
                                           ,@CNoOfPackage)";
                        }
                        else
                        {
                            sql = @"INSERT INTO [dbo].[ContainerInfo]
                                           ([HouseBLId]
                                           ,[PackingId]
                                           ,[TypeofPackageId]
                                           ,[ContainerNo]
                                           ,[ContainerTypeId]
                                           ,[CNoOfPackage]
                                           ,[Size]
                                           ,[SealNo]
                                           ,[ContainerIndicatorId])
                                     VALUES
                                           ((SELECT IDENT_CURRENT('HouseBL'))
                                           ,@PackingId
                                           ,@TypeofPackageId
                                           ,@ContainerNo
                                           ,@ContainerTypeId
                                           ,@CNoOfPackage
                                           ,@Size
                                           ,@SealNo
                                           ,@ContainerIndicatorId)";
                        }
                        sqlList.Add(sql);
                    }

                    for (int i = 0; i < HouseBLItems[j].ContainerItems.Count; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@PackingId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackingType;
                        List.Add(param);
                        param = new SqlParameter("@ContainerNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerTypeId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerType > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        }
                        List.Add(param);
                        param = new SqlParameter("@TypeofPackageId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackageType;
                        List.Add(param);
                        param = new SqlParameter("@CNoOfPackage", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].CNoOfPackage;
                        List.Add(param);
                        param = new SqlParameter("@Size", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].ContainerSize;
                        List.Add(param);
                        param = new SqlParameter("@SealNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].SealNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].SealNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerIndicatorId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].Indicator > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        }
                        List.Add(param);

                        paramlist.Add(List);
                    }
                    DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool SaveXMLasHouseBLItems(CustomXML.AwmdsBol_segment[] HouseBLItems, int LastShipmentId, int status = 1)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;

            for (int j = 0; j < HouseBLItems.Length; j++)
            {
                sql = @"DECLARE @HBLLastId INT
                            INSERT INTO [dbo].[HouseBL]
                                       ([ShipmentId]
                                       ,[HouseBL]
                                       ,[NoOfPackage]
                                       ,[FreightIndicatorId]
                                       ,[BLNatureId]
                                       ,[BLTypesId]
                                       ,[ShippingMark]
                                       ,[Weight]
                                       ,[Measurement]
                                       ,[Description])
                                 VALUES
                                       (@LastShipmentId
                                       ,@HouseBL
                                       ,@NoOfPackage
                                       ,@FreightIndicatorId
                                       ,@BLNatureId
                                       ,@BLTypesId
                                       ,@ShippingMark
                                       ,@Weight
                                       ,@Measurement
                                       ,@Description)

                           SELECT @HBLLastId = SCOPE_IDENTITY();

                            INSERT INTO [dbo].[BLStatusHistory]
                                       ([BLStatusId]
                                       ,[HouseBLId]
                                       ,[DateCreated]
                                       ,[EmployeeId])
                                 VALUES
                                       (@BLStatusId
                                       ,@HBLLastId
                                       ,GETDATE()
                                       ,@EmployeeId)";

                List = new List<SqlParameter>();
                param = new SqlParameter("@LastShipmentId", DbType.Int32);
                param.Value = LastShipmentId;
                List.Add(param);

                param = new SqlParameter("@HouseBL", DbType.String);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].Bol_id.Bol_reference != "")
                {
                    param.Value = HouseBLItems[j].Bol_id.Bol_reference;
                }
                List.Add(param);
                param = new SqlParameter("@NoOfPackage", DbType.Int32);
                param.Value = HouseBLItems[j].Goods_segment.Number_of_packages;
                List.Add(param);
                param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].Value_segment.Freight_segment.PC_indicator != "")
                {
                    param.Value = MySettings.GetFreightIndicatorId(HouseBLItems[j].Value_segment.Freight_segment.PC_indicator);
                }
                List.Add(param);
                param = new SqlParameter("@BLNatureId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].Bol_id.Bol_nature == 22)
                {
                    param.Value = 1;
                }
                else if (HouseBLItems[j].Bol_id.Bol_nature == 23)
                {
                    param.Value = 2;
                }
                else if (HouseBLItems[j].Bol_id.Bol_nature == 24)
                {
                    param.Value = 3;
                }
                else if (HouseBLItems[j].Bol_id.Bol_nature == 28)
                {
                    param.Value = 4;
                }
                List.Add(param);
                param = new SqlParameter("@BLTypesId", DbType.Int32);
                param.Value = MySettings.GetBLTypeId(HouseBLItems[j].Bol_id.Bol_type_code);
                List.Add(param);
                param = new SqlParameter("@ShippingMark", DbType.String);
                param.Value = HouseBLItems[j].Goods_segment.Shipping_marks;
                List.Add(param);
                param = new SqlParameter("@Weight", DbType.Decimal);
                param.Value = HouseBLItems[j].Goods_segment.Gross_mass;
                List.Add(param);
                param = new SqlParameter("@Measurement", DbType.Decimal);
                param.Value = HouseBLItems[j].Goods_segment.Volume_in_cubic_meters;
                List.Add(param);
                param = new SqlParameter("@Description", DbType.String);
                param.Value = HouseBLItems[j].Goods_segment.Goods_description;
                List.Add(param);
                param = new SqlParameter("@BLStatusId", DbType.Int32);
                param.Value = status;
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);

                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // cotainer item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < HouseBLItems[j].ctn_segment.Length; k++)
                    {
                        sql = @"INSERT INTO [dbo].[ContainerInfo]
                                           ([HouseBLId]
                                           ,[PackingId]
                                           ,[TypeofPackageId]
                                           ,[ContainerNo]
                                           ,[ContainerTypeId]
                                           ,[CNoOfPackage]
                                           ,[Size]
                                           ,[SealNo]
                                           ,[ContainerIndicatorId])
                                     VALUES
                                           ((SELECT IDENT_CURRENT('HouseBL'))
                                           ,@PackingId
                                           ,@TypeofPackageId
                                           ,@ContainerNo
                                           ,@ContainerTypeId
                                           ,@CNoOfPackage
                                           ,@Size
                                           ,@SealNo
                                           ,@ContainerIndicatorId)";
                        sqlList.Add(sql);
                    }

                    for (int i = 0; i < HouseBLItems[j].ctn_segment.Length; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@PackingId", DbType.Int32);
                        param.Value = 1;
                        List.Add(param);
                        param = new SqlParameter("@ContainerNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ctn_segment[i].Ctn_reference != "")
                        {
                            param.Value = HouseBLItems[j].ctn_segment[i].Ctn_reference;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerTypeId", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ctn_segment[i].Type_of_container != "")
                        {
                            param.Value = MySettings.GetContainerTypeId(HouseBLItems[j].ctn_segment[i].Type_of_container.Substring(2, 2));
                        }
                        List.Add(param);
                        param = new SqlParameter("@TypeofPackageId", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].Goods_segment.Package_type_code != "")
                        {
                            param.Value = MySettings.GetPackageTypeId(HouseBLItems[j].Goods_segment.Package_type_code);
                        }
                        List.Add(param);
                        param = new SqlParameter("@CNoOfPackage", DbType.Int32);
                        param.Value = HouseBLItems[j].ctn_segment[i].Number_of_packages;
                        List.Add(param);
                        param = new SqlParameter("@Size", DbType.Int32);
                        param.Value = HouseBLItems[j].ctn_segment[i].Type_of_container.Substring(0, 2);
                        List.Add(param);
                        param = new SqlParameter("@SealNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ctn_segment[i].Marks1 != "")
                        {
                            param.Value = HouseBLItems[j].ctn_segment[i].Marks1;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerIndicatorId", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ctn_segment[i].Empty_Full != "")
                        {
                            param.Value = MySettings.GetContainerIndicatorId(HouseBLItems[j].ctn_segment[i].Empty_Full);
                        }
                        List.Add(param);

                        paramlist.Add(List);
                    }
                    DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public string GetXMLMasterBL(CustomXML.AwmdsBol_segment[] HouseBLItems)
        {
            string MasterBL = "";
            for (int j = 0; j < HouseBLItems.Length; j++)
            {
                MasterBL = HouseBLItems[j].Bol_id.Master_bol_ref_number;
                if (MasterBL != "")
                {
                    return MasterBL;
                }
            }
            return MasterBL;
        }

        public string CheckExistingHBL(CustomXML.AwmdsBol_segment[] HouseBLItems)
        {
            string HouseBL = "";
            for (int j = 0; j < HouseBLItems.Length; j++)
            {
                Dictionary<object, object> HBL = GetHouseBL(HouseBLItems[j].Bol_id.Bol_reference);
                if (HBL.Count > 0)
                {
                    return HouseBL = HouseBLItems[j].Bol_id.Bol_reference;
                }
            }
            return HouseBL;
        }

        /*    public bool SaveHouseBLItems(List<HouseBLModel> HouseBLItems, int LastShipmentId, int status = 1)
            {
                List<string> sqlList;
                List<SqlParameter> List;

                List<List<SqlParameter>> paramlist;
                SqlParameter param;

                sqlList = new List<string>();
                paramlist = new List<List<SqlParameter>>();

                string sql = @"DECLARE @HBLLastId INT
                                INSERT INTO [dbo].[HouseBL]
                                           ([ShipmentId]
                                           ,[HouseBL]
                                          -- ,[ContainerTypeId]
                                         --  ,[ContainerNo]
                                          -- ,[SealNo]
                                           ,[NoOfPackage]
                                           ,[FreightIndicatorId]
                                           ,[ShipperId]
                                           ,[CustomerId]
                                           ,[NotifyPartyId]
                                           ,[DOAgentId]
                                           ,[Weight]
                                           ,[Measurement]
                                           ,[PortOfLoading]
                                           ,[PortOfUnloading]
                                           ,[PortOfOrigin]
                                           ,[OriginalLoadingPort]
                                           ,[PortOfDelivery]
                                           ,[UltimateDestination]
                                           ,[Description]
                                           ,[ContainerInfo])
                                     VALUES
                                           (@LastShipmentId
                                           ,@HouseBL
                                          -- ,@ContainerTypeId
                                          -- ,@ContainerNo
                                          -- ,@SealNo
                                           ,@NoOfPackage
                                           ,@FreightIndicatorId
                                           ,@ShipperId
                                           ,@CustomerId
                                           ,@NotifyPartyId
                                           ,@DOAgentId
                                           ,@Weight
                                           ,@Measurement
                                           ,@PortOfLoading
                                           ,@PortOfUnloading
                                           ,@PortOfOrigin
                                           ,@OriginalLoadingPort
                                           ,@PortOfDelivery
                                           ,@UltimateDestination
                                           ,@Description
                                           ,@ContainerInfo)

                               SELECT @HBLLastId = SCOPE_IDENTITY();

                                INSERT INTO [dbo].[BLStatusHistory]
                                           ([BLStatusId]
                                           ,[HouseBLId]
                                           ,[DateCreated]
                                           ,[EmployeeId])
                                     VALUES
                                           (@BLStatusId
                                           ,@HBLLastId
                                           ,GETDATE()
                                           ,@EmployeeId)";

                for (int i = 0; i < HouseBLItems.Count; i++)
                {
                    sqlList.Add(sql);
                }
                for (int j = 0; j < HouseBLItems.Count; j++)
                {

                    List = new List<SqlParameter>();
                    param = new SqlParameter("@LastShipmentId", DbType.Int32);
                    param.Value = LastShipmentId;
                    List.Add(param);

                    param = new SqlParameter("@HouseBL", DbType.String);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].HouseBLno !="")
                    {
                        param.Value = HouseBLItems[j].HouseBLno;
                    }
                    List.Add(param);
                    param = new SqlParameter("@NoOfPackage", DbType.Int32);
                    param.Value = HouseBLItems[j].NoOfPackage;
                    List.Add(param);
                    param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                    param.Value = HouseBLItems[j].FreightIndicator;
                    List.Add(param);
                    param = new SqlParameter("@ShipperId", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].ShipperNew != null)
                    {
                        param.Value = HouseBLItems[j].ShipperNew.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@CustomerId", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].CustomerNew != null)
                    {
                        param.Value = HouseBLItems[j].CustomerNew.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@NotifyPartyId", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].NotifyPartyNew != null)
                    {
                        param.Value = HouseBLItems[j].NotifyPartyNew.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@DOAgentId", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].DOAgent != null)
                    {
                        param.Value = HouseBLItems[j].DOAgent.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@Weight", DbType.Decimal);
                    param.Value = HouseBLItems[j].Weight;
                    List.Add(param);
                    param = new SqlParameter("@Measurement", DbType.Decimal);
                    param.Value = HouseBLItems[j].Measurement;
                    List.Add(param);
                    param = new SqlParameter("@PortOfLoading", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].PortOfLoading != null)
                    {
                        param.Value = HouseBLItems[j].PortOfLoading.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@PortOfUnloading", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].PortOfUnLoading != null)
                    {
                        param.Value = HouseBLItems[j].PortOfUnLoading.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@PortOfOrigin", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].PortOfOrigin != null)
                    {
                        param.Value = HouseBLItems[j].PortOfOrigin.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@OriginalLoadingPort", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].OriginalLoadingPort != null)
                    {
                        param.Value = HouseBLItems[j].OriginalLoadingPort.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@PortOfDelivery", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].PortOfDelivery != null)
                    {
                        param.Value = HouseBLItems[j].PortOfDelivery.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@UltimateDestination", DbType.Int32);
                    param.Value = DBNull.Value;
                    if (HouseBLItems[j].UltimateDestination != null)
                    {
                        param.Value = HouseBLItems[j].UltimateDestination.id;
                    }
                    List.Add(param);
                    param = new SqlParameter("@Description", DbType.String);
                    param.Value = HouseBLItems[j].Description;
                    List.Add(param);
                    param = new SqlParameter("@ContainerInfo", DbType.String);
                    param.Value = HouseBLItems[j].ContainerInfo;
                    List.Add(param);
                    param = new SqlParameter("@BLStatusId", DbType.Int32);
                    param.Value = status;
                    List.Add(param);
                    param = new SqlParameter("@EmployeeId", DbType.Int32);
                    param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                    List.Add(param);

                    paramlist.Add(List);

                }

                return DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
            }*/

        /*   public bool UpdateHouseBLItems(List<HouseBLModel> HouseBLItems, int ShipmentId, int ConvertJob = 0)
           {
               List<string> sqlList;
               List<SqlParameter> List;

               List<List<SqlParameter>> paramlist;
               SqlParameter param;

               sqlList = new List<string>();
               paramlist = new List<List<SqlParameter>>();

               string sql = @"IF EXISTS(select Id from HouseBL where Id = @HouseBLId)
                               BEGIN
                                       UPDATE [dbo].[HouseBL]
                                           SET [HouseBL] = @HouseBL
                                               --,[ContainerTypeId] = @ContainerTypeId
                                               --,[ContainerNo] = @ContainerNo
                                               --,[SealNo] = @SealNo
                                               ,[NoOfPackage] = @NoOfPackage
                                               ,[FreightIndicatorId] = @FreightIndicatorId
                                               ,[ShipperId] = @ShipperId
                                               ,[CustomerId] = @CustomerId
                                               ,[NotifyPartyId] = @NotifyPartyId
                                               ,[DOAgentId] = @DOAgentId
                                               ,[Weight] = @Weight
                                               ,[Measurement] = @Measurement
                                               ,[PortOfLoading] = @PortOfLoading
                                               ,[PortOfUnloading] = @PortOfUnloading
                                               ,[PortOfOrigin] = @PortOfOrigin
                                               ,[OriginalLoadingPort] = @OriginalLoadingPort
                                               ,[PortOfDelivery] = @PortOfDelivery
                                               ,[UltimateDestination] = @UltimateDestination
                                               ,[Description] = @Description
                                               ,[ContainerInfo] = @ContainerInfo
                                           WHERE Id = @HouseBLId

                                       INSERT INTO [dbo].[BLStatusHistory]
                                                   ([BLStatusId]
                                                   ,[HouseBLId]
                                                   ,[DateCreated]
                                                   ,[EmployeeId]
                                                   ,[ClearanceBy])
                                               VALUES
                                                   (@BLStatusId
                                                   ,@HouseBLId
                                                   ,GETDATE()
                                                   ,@EmployeeId
                                                   ,@ClearanceBy)
                               END
                               ELSE
                               BEGIN

                                           DECLARE @HBLLastId INT
                                            INSERT INTO [dbo].[HouseBL]
                                                  ([ShipmentId]
                                                  ,[HouseBL]
                                                  --,[ContainerTypeId]
                                                 -- ,[ContainerNo]
                                                  --,[SealNo]
                                                  ,[NoOfPackage]
                                                  ,[FreightIndicatorId]
                                                  ,[ShipperId]
                                                  ,[CustomerId]
                                                  ,[NotifyPartyId]
                                                  ,[DOAgentId]
                                                  ,[Weight]
                                                  ,[Measurement]
                                                  ,[PortOfLoading]
                                                  ,[PortOfUnloading]
                                                  ,[PortOfOrigin]
                                                  ,[OriginalLoadingPort]
                                                  ,[PortOfDelivery]
                                                  ,[UltimateDestination]
                                                  ,[Description]
                                                  ,[ContainerInfo])
                                            VALUES
                                                  (@ShipmentId
                                                  ,@HouseBL
                                                 -- ,@ContainerTypeId
                                                 -- ,@ContainerNo
                                                 -- ,@SealNo
                                                  ,@NoOfPackage
                                                  ,@FreightIndicatorId
                                                  ,@ShipperId
                                                  ,@CustomerId
                                                  ,@NotifyPartyId
                                                  ,@DOAgentId
                                                  ,@Weight
                                                  ,@Measurement
                                                  ,@PortOfLoading
                                                  ,@PortOfUnloading
                                                  ,@PortOfOrigin
                                                  ,@OriginalLoadingPort
                                                  ,@PortOfDelivery
                                                  ,@UltimateDestination
                                                  ,@Description
                                                  ,@ContainerInfo)

                                      SELECT @HBLLastId = SCOPE_IDENTITY();

                                       INSERT INTO [dbo].[BLStatusHistory]
                                                  ([BLStatusId]
                                                  ,[HouseBLId]
                                                  ,[DateCreated]
                                                  ,[EmployeeId]
                                                  ,[ClearanceBy])
                                            VALUES
                                                  (@BLStatusId
                                                  ,@HBLLastId
                                                  ,GETDATE()
                                                  ,@EmployeeId
                                                  ,@ClearanceBy)
                                 END";

               for (int i = 0; i < HouseBLItems.Count; i++)
               {
                   sqlList.Add(sql);
               }
               for (int j = 0; j < HouseBLItems.Count; j++)
               {

                   List = new List<SqlParameter>();
                   param = new SqlParameter("@HouseBLId", DbType.Int32);
                   param.Value = HouseBLItems[j].HouseBLId;
                   List.Add(param);
                   param = new SqlParameter("@ShipmentId", DbType.Int32);
                   param.Value = ShipmentId;
                   List.Add(param);
                   param = new SqlParameter("@HouseBL", DbType.String);
                   param.Value = HouseBLItems[j].HouseBLno;
                   List.Add(param);
                   param = new SqlParameter("@NoOfPackage", DbType.Int32);
                   param.Value = HouseBLItems[j].NoOfPackage;
                   List.Add(param);
                   param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                   param.Value = HouseBLItems[j].FreightIndicator;
                   List.Add(param);
                   param = new SqlParameter("@ShipperId", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].ShipperNew.id > 0)
                   {
                       param.Value = HouseBLItems[j].ShipperNew.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@CustomerId", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].CustomerNew.id > 0)
                   {
                       param.Value = HouseBLItems[j].CustomerNew.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@NotifyPartyId", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].NotifyPartyNew.id > 0)
                   {
                       param.Value = HouseBLItems[j].NotifyPartyNew.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@DOAgentId", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].DOAgent.id > 0)
                   {
                       param.Value = HouseBLItems[j].DOAgent.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@Weight", DbType.Decimal);
                   param.Value = HouseBLItems[j].Weight;
                   List.Add(param);
                   param = new SqlParameter("@Measurement", DbType.Decimal);
                   param.Value = HouseBLItems[j].Measurement;
                   List.Add(param);
                   param = new SqlParameter("@PortOfLoading", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].PortOfLoading.id > 0)
                   {
                       param.Value = HouseBLItems[j].PortOfLoading.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@PortOfUnloading", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].PortOfUnLoading.id  > 0)
                   {
                       param.Value = HouseBLItems[j].PortOfUnLoading.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@PortOfOrigin", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].PortOfOrigin.id > 0)
                   {
                       param.Value = HouseBLItems[j].PortOfOrigin.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@OriginalLoadingPort", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].OriginalLoadingPort.id > 0)
                   {
                       param.Value = HouseBLItems[j].OriginalLoadingPort.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@PortOfDelivery", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].PortOfDelivery.id > 0)
                   {
                       param.Value = HouseBLItems[j].PortOfDelivery.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@UltimateDestination", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].UltimateDestination.id > 0)
                   {
                       param.Value = HouseBLItems[j].UltimateDestination.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@Description", DbType.String);
                   param.Value = HouseBLItems[j].Description;
                   List.Add(param);
                   param = new SqlParameter("@ContainerInfo", DbType.String);
                   param.Value = HouseBLItems[j].ContainerInfo;
                   List.Add(param);

                   param = new SqlParameter("@BLStatusId", DbType.Int32);
                   if (ConvertJob > 0)
                   {
                       param.Value = ConvertJob;
                   }
                   else
                   {
                       param.Value = HouseBLItems[j].BLStatusId;
                   }
                   List.Add(param);
                   param = new SqlParameter("@ClearanceBy", DbType.Int32);
                   param.Value = DBNull.Value;
                   if (HouseBLItems[j].ClearanceBy.id != 0)
                   {
                       param.Value = HouseBLItems[j].ClearanceBy.id;
                   }
                   List.Add(param);
                   param = new SqlParameter("@EmployeeId", DbType.Int32);
                   param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                   List.Add(param);

                   paramlist.Add(List);

               }

               return DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
           }*/
        public bool UpdateHouseBLItems(List<HouseBLModel> HouseBLItems, int ShipmentId, int ConvertJob = 0)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;


            for (int j = 0; j < HouseBLItems.Count; j++)
            {

                sql = @"IF EXISTS(select Id from HouseBL where Id = @HouseBLId)
                            BEGIN
                                    UPDATE [dbo].[HouseBL]
                                        SET [HouseBL] = @HouseBL
                                            --,[ContainerTypeId] = @ContainerTypeId
                                            --,[ContainerNo] = @ContainerNo
                                            --,[SealNo] = @SealNo
                                            ,[NoOfPackage] = @NoOfPackage
                                            ,[FreightIndicatorId] = @FreightIndicatorId
                                            ,[BLNatureId] = @BLNatureId
                                            ,[BLTypesId] = @BLTypesId
,[BLStateId] = @BLStateId
                                            ,[ShippingMark] = @ShippingMark
                                            ,[ShipperId] = @ShipperId
                                            ,[CustomerId] = @CustomerId
                                            ,[NotifyPartyId] = @NotifyPartyId
                                      --      ,[DOAgentId] = @DOAgentId
                                            ,[Weight] = @Weight
                                            ,[Measurement] = @Measurement
                                            ,[ProjectId] = @ProjectId
                                            ,[PortOfLoading] = @PortOfLoading
                                            ,[PortOfUnloading] = @PortOfUnloading
                                            ,[PortOfOrigin] = @PortOfOrigin
                                            ,[OriginalLoadingPort] = @OriginalLoadingPort
                                            ,[PortOfDelivery] = @PortOfDelivery
                                            ,[UltimateDestination] = @UltimateDestination
                                            ,[Description] = @Description
                                        WHERE Id = @HouseBLId

                                    INSERT INTO [dbo].[ManifestUpdateLog]
                                                ([BLStatusId]
                                                ,[HouseBLId]
                                                ,[DateCreated]
                                                ,[EmployeeId])
                                            VALUES
                                                (@BLStatusId
                                                ,@HouseBLId
                                                ,GETDATE()
                                                ,@EmployeeId)
                            END
                            ELSE
                            BEGIN

                                        DECLARE @HBLLastId INT
                                         INSERT INTO [dbo].[HouseBL]
                                               ([ShipmentId]
                                               ,[HouseBL]
                                               --,[ContainerTypeId]
                                              -- ,[ContainerNo]
                                               --,[SealNo]
                                               ,[NoOfPackage]
                                               ,[FreightIndicatorId]
                                               ,[BLNatureId]
,[BLTypesId]
,[BLStateId]
                                               ,[ShippingMark]
                                               ,[ShipperId]
                                               ,[CustomerId]
                                               ,[NotifyPartyId]
                                      --         ,[DOAgentId]
                                               ,[Weight]
                                               ,[Measurement]
                                               ,[ProjectId]
                                               ,[PortOfLoading]
                                               ,[PortOfUnloading]
                                               ,[PortOfOrigin]
                                               ,[OriginalLoadingPort]
                                               ,[PortOfDelivery]
                                               ,[UltimateDestination]
                                               ,[Description])
                                         VALUES
                                               (@ShipmentId
                                               ,@HouseBL
                                              -- ,@ContainerTypeId
                                              -- ,@ContainerNo
                                              -- ,@SealNo
                                               ,@NoOfPackage
                                               ,@FreightIndicatorId
                                               ,@BLNatureId
,@BLTypesId
,@BLStateId
                                               ,@ShippingMark
                                               ,@ShipperId
                                               ,@CustomerId
                                               ,@NotifyPartyId
                                           --    ,@DOAgentId
                                               ,@Weight
                                               ,@Measurement
                                               ,@ProjectId
                                               ,@PortOfLoading
                                               ,@PortOfUnloading
                                               ,@PortOfOrigin
                                               ,@OriginalLoadingPort
                                               ,@PortOfDelivery
                                               ,@UltimateDestination
                                               ,@Description)

                                   SELECT @HBLLastId = SCOPE_IDENTITY();

                                    INSERT INTO [dbo].[BLStatusHistory]
                                               ([BLStatusId]
                                               ,[HouseBLId]
                                               ,[DateCreated]
                                               ,[EmployeeId])
                                         VALUES
                                               (@BLStatusId
                                               ,@HBLLastId
                                               ,GETDATE()
                                               ,@EmployeeId)

                                                    IF EXISTS(select ShipmentId from ShipmentStatusHistory where ShipmentStatusId = 3 AND ShipmentId = @ShipmentId)
                                                    BEGIN
	                                                    INSERT INTO [dbo].[Payments]
			                                                    ([HouseBLId]
			                                                    ,[PaymentTypesId]
			                                                    ,[UpdatedDate]
			                                                    ,[PaidUpdatedBy]
			                                                    ,[PaymentStat])
		                                                    VALUES
			                                                    (@HBLLastId
			                                                    ,1
			                                                    ,GETDATE()
			                                                    ,@EmployeeId
			                                                    ,0)

                                                            INSERT INTO [dbo].[BLStatusHistory]
											                           ([HouseBLId]
											                           ,[BLStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId]
                                                                       ,[DOCollectedTypeId])
										                         VALUES
											                           (@HBLLastId
											                           ,5
											                           ,GETDATE()
											                           ,@EmployeeId
                                                                       ,1)
                                                    END

                              END";

                List = new List<SqlParameter>();
                param = new SqlParameter("@HouseBLId", DbType.Int32);
                param.Value = HouseBLItems[j].HouseBLId;
                List.Add(param);
                param = new SqlParameter("@ShipmentId", DbType.Int32);
                param.Value = ShipmentId;
                List.Add(param);
                /* param = new SqlParameter("@HouseBL", DbType.String);
                 param.Value = HouseBLItems[j].HouseBLno;
                 List.Add(param);*/
                param = new SqlParameter("@HouseBL", DbType.String);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].HouseBLno != "")
                {
                    param.Value = HouseBLItems[j].HouseBLno.Replace(" ", String.Empty);
                }
                List.Add(param);
                param = new SqlParameter("@NoOfPackage", DbType.Int32);
                param.Value = HouseBLItems[j].TotalBLPackages;
                List.Add(param);
                param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                param.Value = HouseBLItems[j].FreightIndicator;
                List.Add(param);
                param = new SqlParameter("@BLNatureId", DbType.Int32);
                param.Value = HouseBLItems[j].BLNature;
                List.Add(param);
                param = new SqlParameter("@BLTypesId", DbType.Int32);
                param.Value = HouseBLItems[j].BLTypeId;
                List.Add(param);

                param = new SqlParameter("@BLStateId", DbType.Int32);
                param.Value = HouseBLItems[j].BLStateId;
                List.Add(param); 

                param = new SqlParameter("@ShippingMark", DbType.String);
                param.Value = HouseBLItems[j].ShippingMark;
                List.Add(param);
                param = new SqlParameter("@ShipperId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ShipperNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].ShipperNew.id > 0)
                {
                    param.Value = HouseBLItems[j].ShipperNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@CustomerId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].CustomerNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].CustomerNew.id > 0)
                {
                    param.Value = HouseBLItems[j].CustomerNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@NotifyPartyId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].NotifyPartyNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].NotifyPartyNew.id > 0)
                {
                    param.Value = HouseBLItems[j].NotifyPartyNew.id;
                }
                List.Add(param);
                /*param = new SqlParameter("@DOAgentId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].DOAgent == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].DOAgent.id > 0)
                {
                    param.Value = HouseBLItems[j].DOAgent.id;
                }
                List.Add(param);*/
                param = new SqlParameter("@Weight", DbType.Decimal);
                param.Value = HouseBLItems[j].Weight;
                List.Add(param);
                param = new SqlParameter("@Measurement", DbType.Decimal);
                param.Value = HouseBLItems[j].Measurement;
                List.Add(param);
                param = new SqlParameter("@ProjectId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ProjectId == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].ProjectId.id > 0)
                {
                    param.Value = HouseBLItems[j].ProjectId.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfLoading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfLoading == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfLoading.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfUnloading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfUnLoading == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfUnLoading.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfUnLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfOrigin", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfOrigin == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfOrigin.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfOrigin.id;
                }
                List.Add(param);
                param = new SqlParameter("@OriginalLoadingPort", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].OriginalLoadingPort == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].OriginalLoadingPort.id > 0)
                {
                    param.Value = HouseBLItems[j].OriginalLoadingPort.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfDelivery", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfDelivery == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfDelivery.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfDelivery.id;
                }
                List.Add(param);
                param = new SqlParameter("@UltimateDestination", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].UltimateDestination == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].UltimateDestination.id > 0)
                {
                    param.Value = HouseBLItems[j].UltimateDestination.id;
                }
                List.Add(param);
                param = new SqlParameter("@Description", DbType.String);
                param.Value = HouseBLItems[j].Description;
                List.Add(param);

                param = new SqlParameter("@BLStatusId", DbType.Int32);
                if (ConvertJob > 0)
                {
                    param.Value = ConvertJob;
                }
                else
                {
                    param.Value = HouseBLItems[j].BLStatusId;
                }
                List.Add(param);
                param = new SqlParameter("@ClearanceBy", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ClearanceBy != null)
                {
                    if (HouseBLItems[j].ClearanceBy.id != 0)
                    {
                        param.Value = HouseBLItems[j].ClearanceBy.id;
                    }
                }
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);
                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // cotainer item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < HouseBLItems[j].ContainerItems.Count; k++)
                    {
                        if (HouseBLItems[j].ContainerItems[k].PackingType == 2)
                        {
                            sql = @"IF EXISTS(select Id from ContainerInfo where Id = @ContainerId)
			                    BEGIN
					                    UPDATE [dbo].[ContainerInfo]
						                    SET [TypeofPackageId] = @TypeofPackageId
							                   ,[CNoOfPackage] = @CNoOfPackage
						                    WHERE Id = @ContainerId
			                    END
			                    ELSE
			                    BEGIN
					                    INSERT INTO [dbo].[ContainerInfo]
							                    (HouseBLId
							                    ,[PackingId]
							                    ,[TypeofPackageId]
							                    ,[CNoOfPackage])
						                    VALUES
							                    (--%HouseBLId%
							                    ,@PackingId
							                    ,@TypeofPackageId
							                    ,@CNoOfPackage)
				                    END";
                        }
                        else
                        {
                            sql = @"IF EXISTS(select Id from ContainerInfo where Id = @ContainerId)
			                    BEGIN
					                    UPDATE [dbo].[ContainerInfo]
						                    SET  [ContainerNo] = @ContainerNo
							                    ,[ContainerTypeId] = @ContainerTypeId
							                    ,[CNoOfPackage] = @CNoOfPackage
							                    ,[Size] = @Size
							                    ,[SealNo] = @SealNo
                                                ,[TypeofPackageId] = @TypeofPackageId
							                    ,[ContainerIndicatorId] = @ContainerIndicatorId
						                    WHERE Id = @ContainerId
			                    END
			                    ELSE
			                    BEGIN
					                    INSERT INTO [dbo].[ContainerInfo]
							                    (HouseBLId
							                    ,[PackingId]
							                    ,[ContainerNo]
							                    ,[ContainerTypeId]
							                    ,[TypeofPackageId]
							                    ,[CNoOfPackage]
							                    ,[Size]
							                    ,[SealNo]
							                    ,[ContainerIndicatorId])
						                    VALUES
							                    (--%HouseBLId%
							                    ,@PackingId
							                    ,@ContainerNo
							                    ,@ContainerTypeId
							                    ,@TypeofPackageId
							                    ,@CNoOfPackage
							                    ,@Size
							                    ,@SealNo
							                    ,@ContainerIndicatorId)
				                    END";
                        }
                        if (HouseBLItems[j].HouseBLId > 0)
                        {
                            sql = sql.Replace("--%HouseBLId%", "@HouseBLId");
                        }
                        else
                        {
                            sql = sql.Replace("--%HouseBLId%", "(SELECT IDENT_CURRENT('HouseBL'))");
                        }
                        sqlList.Add(sql);
                    }

                    for (int i = 0; i < HouseBLItems[j].ContainerItems.Count; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@PackingId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackingType;
                        List.Add(param);
                        param = new SqlParameter("@ContainerNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerTypeId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerType > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        }
                        List.Add(param);
                        param = new SqlParameter("@TypeofPackageId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackageType;
                        List.Add(param);
                        param = new SqlParameter("@CNoOfPackage", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].CNoOfPackage;
                        List.Add(param);
                        param = new SqlParameter("@Size", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].ContainerSize;
                        List.Add(param);
                        param = new SqlParameter("@SealNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].SealNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].SealNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerIndicatorId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].Indicator > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        }
                        List.Add(param);

                        param = new SqlParameter("@ContainerId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].containerId;
                        List.Add(param);
                        if (HouseBLItems[j].HouseBLId > 0)
                        {
                            param = new SqlParameter("@HouseBLId", DbType.Int32);
                            param.Value = HouseBLItems[j].HouseBLId;
                            List.Add(param);
                        }

                        paramlist.Add(List);
                    }
                    DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHouseBLItemsConvertJob(List<HouseBLModel> HouseBLItems, int ShipmentId, int ConvertJob = 0)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;


            for (int j = 0; j < HouseBLItems.Count; j++)
            {

                sql = @"IF EXISTS(select Id from HouseBL where Id = @HouseBLId)
                            BEGIN
                                    UPDATE [dbo].[HouseBL]
                                        SET [HouseBL] = @HouseBL
                                            --,[ContainerTypeId] = @ContainerTypeId
                                            --,[ContainerNo] = @ContainerNo
                                            --,[SealNo] = @SealNo
                                            ,[NoOfPackage] = @NoOfPackage
                                            ,[FreightIndicatorId] = @FreightIndicatorId
                                            ,[BLNatureId] = @BLNatureId
                                            ,[BLTypesId] = @BLTypesId
,[BLStateId] = @BLStateId
                                            ,[ShippingMark] = @ShippingMark
                                            ,[ShipperId] = @ShipperId
                                            ,[CustomerId] = @CustomerId
                                            ,[NotifyPartyId] = @NotifyPartyId
                                      --      ,[DOAgentId] = @DOAgentId
                                            ,[Weight] = @Weight
                                            ,[Measurement] = @Measurement
                                            ,[ProjectId] = @ProjectId
                                            ,[PortOfLoading] = @PortOfLoading
                                            ,[PortOfUnloading] = @PortOfUnloading
                                            ,[PortOfOrigin] = @PortOfOrigin
                                            ,[OriginalLoadingPort] = @OriginalLoadingPort
                                            ,[PortOfDelivery] = @PortOfDelivery
                                            ,[UltimateDestination] = @UltimateDestination
                                            ,[Description] = @Description
                                        WHERE Id = @HouseBLId

                                    INSERT INTO [dbo].[BLStatusHistory]
                                                ([BLStatusId]
                                                ,[HouseBLId]
                                                ,[DateCreated]
                                                ,[EmployeeId]
                                                ,[ClearanceBy])
                                            VALUES
                                                (@BLStatusId
                                                ,@HouseBLId
                                                ,GETDATE()
                                                ,@EmployeeId
                                                ,@ClearanceBy)
                            END
                            ELSE
                            BEGIN

                                        DECLARE @HBLLastId INT
                                         INSERT INTO [dbo].[HouseBL]
                                               ([ShipmentId]
                                               ,[HouseBL]
                                               --,[ContainerTypeId]
                                              -- ,[ContainerNo]
                                               --,[SealNo]
                                               ,[NoOfPackage]
                                               ,[FreightIndicatorId]
                                               ,[BLNatureId]
,[BLTypesId]
,[BLStateId]
                                               ,[ShippingMark]
                                               ,[ShipperId]
                                               ,[CustomerId]
                                               ,[NotifyPartyId]
                                      --         ,[DOAgentId]
                                               ,[Weight]
                                               ,[Measurement]
                                               ,[ProjectId]
                                               ,[PortOfLoading]
                                               ,[PortOfUnloading]
                                               ,[PortOfOrigin]
                                               ,[OriginalLoadingPort]
                                               ,[PortOfDelivery]
                                               ,[UltimateDestination]
                                               ,[Description])
                                         VALUES
                                               (@ShipmentId
                                               ,@HouseBL
                                              -- ,@ContainerTypeId
                                              -- ,@ContainerNo
                                              -- ,@SealNo
                                               ,@NoOfPackage
                                               ,@FreightIndicatorId
                                               ,@BLNatureId
,@BLTypesId
,@BLStateId
                                               ,@ShippingMark
                                               ,@ShipperId
                                               ,@CustomerId
                                               ,@NotifyPartyId
                                           --    ,@DOAgentId
                                               ,@Weight
                                               ,@Measurement
                                               ,@ProjectId
                                               ,@PortOfLoading
                                               ,@PortOfUnloading
                                               ,@PortOfOrigin
                                               ,@OriginalLoadingPort
                                               ,@PortOfDelivery
                                               ,@UltimateDestination
                                               ,@Description)

                                   SELECT @HBLLastId = SCOPE_IDENTITY();

                                    INSERT INTO [dbo].[BLStatusHistory]
                                               ([BLStatusId]
                                               ,[HouseBLId]
                                               ,[DateCreated]
                                               ,[EmployeeId]
                                               ,[ClearanceBy])
                                         VALUES
                                               (@BLStatusId
                                               ,@HBLLastId
                                               ,GETDATE()
                                               ,@EmployeeId
                                               ,@ClearanceBy)

                                                    IF EXISTS(select ShipmentId from ShipmentStatusHistory where ShipmentStatusId = 3 AND ShipmentId = @ShipmentId)
                                                    BEGIN
	                                                    INSERT INTO [dbo].[Payments]
			                                                    ([HouseBLId]
			                                                    ,[PaymentTypesId]
			                                                    ,[UpdatedDate]
			                                                    ,[PaidUpdatedBy]
			                                                    ,[PaymentStat])
		                                                    VALUES
			                                                    (@HBLLastId
			                                                    ,1
			                                                    ,GETDATE()
			                                                    ,@EmployeeId
			                                                    ,0)

                                                            INSERT INTO [dbo].[BLStatusHistory]
											                           ([HouseBLId]
											                           ,[BLStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId]
                                                                       ,[DOCollectedTypeId])
										                         VALUES
											                           (@HBLLastId
											                           ,5
											                           ,GETDATE()
											                           ,@EmployeeId
                                                                       ,1)
                                                    END

                              END";

                List = new List<SqlParameter>();
                param = new SqlParameter("@HouseBLId", DbType.Int32);
                param.Value = HouseBLItems[j].HouseBLId;
                List.Add(param);
                param = new SqlParameter("@ShipmentId", DbType.Int32);
                param.Value = ShipmentId;
                List.Add(param);
                /* param = new SqlParameter("@HouseBL", DbType.String);
                 param.Value = HouseBLItems[j].HouseBLno;
                 List.Add(param);*/
                param = new SqlParameter("@HouseBL", DbType.String);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].HouseBLno != "")
                {
                    param.Value = HouseBLItems[j].HouseBLno.Replace(" ", String.Empty);
                }
                List.Add(param);
                param = new SqlParameter("@NoOfPackage", DbType.Int32);
                param.Value = HouseBLItems[j].TotalBLPackages;
                List.Add(param);
                param = new SqlParameter("@FreightIndicatorId", DbType.Int32);
                param.Value = HouseBLItems[j].FreightIndicator;
                List.Add(param);
                param = new SqlParameter("@BLNatureId", DbType.Int32);
                param.Value = HouseBLItems[j].BLNature;
                List.Add(param);
                param = new SqlParameter("@BLTypesId", DbType.Int32);
                param.Value = HouseBLItems[j].BLTypeId;
                List.Add(param);
                param = new SqlParameter("@BLStateId", DbType.Int32);
                param.Value = HouseBLItems[j].BLStateId;
                List.Add(param); 
                param = new SqlParameter("@ShippingMark", DbType.String);
                param.Value = HouseBLItems[j].ShippingMark;
                List.Add(param);
                param = new SqlParameter("@ShipperId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ShipperNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].ShipperNew.id > 0)
                {
                    param.Value = HouseBLItems[j].ShipperNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@CustomerId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].CustomerNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].CustomerNew.id > 0)
                {
                    param.Value = HouseBLItems[j].CustomerNew.id;
                }
                List.Add(param);
                param = new SqlParameter("@NotifyPartyId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].NotifyPartyNew == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].NotifyPartyNew.id > 0)
                {
                    param.Value = HouseBLItems[j].NotifyPartyNew.id;
                }
                List.Add(param);
                /*param = new SqlParameter("@DOAgentId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].DOAgent == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].DOAgent.id > 0)
                {
                    param.Value = HouseBLItems[j].DOAgent.id;
                }
                List.Add(param);*/
                param = new SqlParameter("@Weight", DbType.Decimal);
                param.Value = HouseBLItems[j].Weight;
                List.Add(param);
                param = new SqlParameter("@Measurement", DbType.Decimal);
                param.Value = HouseBLItems[j].Measurement;
                List.Add(param);
                param = new SqlParameter("@ProjectId", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ProjectId == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].ProjectId.id > 0)
                {
                    param.Value = HouseBLItems[j].ProjectId.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfLoading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfLoading == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfLoading.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfUnloading", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfUnLoading == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfUnLoading.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfUnLoading.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfOrigin", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfOrigin == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfOrigin.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfOrigin.id;
                }
                List.Add(param);
                param = new SqlParameter("@OriginalLoadingPort", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].OriginalLoadingPort == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].OriginalLoadingPort.id > 0)
                {
                    param.Value = HouseBLItems[j].OriginalLoadingPort.id;
                }
                List.Add(param);
                param = new SqlParameter("@PortOfDelivery", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].PortOfDelivery == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].PortOfDelivery.id > 0)
                {
                    param.Value = HouseBLItems[j].PortOfDelivery.id;
                }
                List.Add(param);
                param = new SqlParameter("@UltimateDestination", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].UltimateDestination == null)
                {
                    param.Value = DBNull.Value;
                }
                else if (HouseBLItems[j].UltimateDestination.id > 0)
                {
                    param.Value = HouseBLItems[j].UltimateDestination.id;
                }
                List.Add(param);
                param = new SqlParameter("@Description", DbType.String);
                param.Value = HouseBLItems[j].Description;
                List.Add(param);

                param = new SqlParameter("@BLStatusId", DbType.Int32);
                if (ConvertJob > 0)
                {
                    param.Value = ConvertJob;
                }
                else
                {
                    param.Value = HouseBLItems[j].BLStatusId;
                }
                List.Add(param);
                param = new SqlParameter("@ClearanceBy", DbType.Int32);
                param.Value = DBNull.Value;
                if (HouseBLItems[j].ClearanceBy != null)
                {
                    if (HouseBLItems[j].ClearanceBy.id != 0)
                    {
                        param.Value = HouseBLItems[j].ClearanceBy.id;
                    }
                }
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);
                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // cotainer item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < HouseBLItems[j].ContainerItems.Count; k++)
                    {
                        if (HouseBLItems[j].ContainerItems[k].PackingType == 2)
                        {
                            sql = @"IF EXISTS(select Id from ContainerInfo where Id = @ContainerId)
			                    BEGIN
					                    UPDATE [dbo].[ContainerInfo]
						                    SET [TypeofPackageId] = @TypeofPackageId
							                   ,[CNoOfPackage] = @CNoOfPackage
						                    WHERE Id = @ContainerId
			                    END
			                    ELSE
			                    BEGIN
					                    INSERT INTO [dbo].[ContainerInfo]
							                    (HouseBLId
							                    ,[PackingId]
							                    ,[TypeofPackageId]
							                    ,[CNoOfPackage])
						                    VALUES
							                    (--%HouseBLId%
							                    ,@PackingId
							                    ,@TypeofPackageId
							                    ,@CNoOfPackage)
				                    END";
                        }
                        else
                        {
                            sql = @"IF EXISTS(select Id from ContainerInfo where Id = @ContainerId)
			                    BEGIN
					                    UPDATE [dbo].[ContainerInfo]
						                    SET  [ContainerNo] = @ContainerNo
							                    ,[ContainerTypeId] = @ContainerTypeId
							                    ,[CNoOfPackage] = @CNoOfPackage
							                    ,[Size] = @Size
							                    ,[SealNo] = @SealNo
                                                ,[TypeofPackageId] = @TypeofPackageId
							                    ,[ContainerIndicatorId] = @ContainerIndicatorId
						                    WHERE Id = @ContainerId
			                    END
			                    ELSE
			                    BEGIN
					                    INSERT INTO [dbo].[ContainerInfo]
							                    (HouseBLId
							                    ,[PackingId]
							                    ,[ContainerNo]
							                    ,[ContainerTypeId]
							                    ,[TypeofPackageId]
							                    ,[CNoOfPackage]
							                    ,[Size]
							                    ,[SealNo]
							                    ,[ContainerIndicatorId])
						                    VALUES
							                    (--%HouseBLId%
							                    ,@PackingId
							                    ,@ContainerNo
							                    ,@ContainerTypeId
							                    ,@TypeofPackageId
							                    ,@CNoOfPackage
							                    ,@Size
							                    ,@SealNo
							                    ,@ContainerIndicatorId)
				                    END";
                        }
                        if (HouseBLItems[j].HouseBLId > 0)
                        {
                            sql = sql.Replace("--%HouseBLId%", "@HouseBLId");
                        }
                        else
                        {
                            sql = sql.Replace("--%HouseBLId%", "(SELECT IDENT_CURRENT('HouseBL'))");
                        }
                        sqlList.Add(sql);
                    }

                    for (int i = 0; i < HouseBLItems[j].ContainerItems.Count; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@PackingId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackingType;
                        List.Add(param);
                        param = new SqlParameter("@ContainerNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerTypeId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].ContainerType > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].ContainerType;
                        }
                        List.Add(param);
                        param = new SqlParameter("@TypeofPackageId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].PackageType;
                        List.Add(param);
                        param = new SqlParameter("@CNoOfPackage", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].CNoOfPackage;
                        List.Add(param);
                        param = new SqlParameter("@Size", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].ContainerSize;
                        List.Add(param);
                        param = new SqlParameter("@SealNo", DbType.String);
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].SealNo != "")
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].SealNo;
                        }
                        List.Add(param);
                        param = new SqlParameter("@ContainerIndicatorId", DbType.Int32);
                        // param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        param.Value = DBNull.Value;
                        if (HouseBLItems[j].ContainerItems[i].Indicator > 0)
                        {
                            param.Value = HouseBLItems[j].ContainerItems[i].Indicator;
                        }
                        List.Add(param);

                        param = new SqlParameter("@ContainerId", DbType.Int32);
                        param.Value = HouseBLItems[j].ContainerItems[i].containerId;
                        List.Add(param);
                        if (HouseBLItems[j].HouseBLId > 0)
                        {
                            param = new SqlParameter("@HouseBLId", DbType.Int32);
                            param.Value = HouseBLItems[j].HouseBLId;
                            List.Add(param);
                        }

                        paramlist.Add(List);
                    }
                    DBAccess.InsertMany(sqlList, paramlist, ConnectionString.DEFAULT);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateSequence()
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = @Sequence
                                WHERE Id= 1";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Sequence", DbType.Int32);
            param.Value = 1 + GetMenifestSequence();
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static bool UpdateHBLSequence()
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = @Sequence
                                WHERE Id= 6";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Sequence", DbType.Int32);
            param.Value = 1 + GetHBLSequence();
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateJobSequence()
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = @Sequence
                                WHERE Id= 2";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Sequence", DbType.Int32);
            param.Value = 1 + GetJobSequence();
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public bool SkipStatus(int HBLid)
        {
            String sql = @"UPDATE BLStatusHistory
                            SET StatusSkipped = GETDATE()
                            where HouseBLId = @HBLid AND BLStatusId in (2,4)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLid", DbType.Int32);
            param.Value = HBLid;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static Dictionary<Object, Object> getLastManifestInfo(int LastShipmentId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"select M.Id  AS ManifestId,M.Number,S.Id from 
                            Shipment AS S
                            INNER JOIN Manifest AS M ON S.ManifestId = M.Id
                            WHERE S.Id = @LastShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@LastShipmentId", DbType.Int32);
            param.Value = LastShipmentId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("LastSid", reader["ManifestId"].ToString());
                dictionary.Add("LastSnumber", reader["Number"].ToString());
                dictionary.Add("lastShipmentId", reader["Id"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GETHBL(int HBLid)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT shipper.Name AS Shipper,
                                   CONCAT(Shipper.Name,'<br>', Shipper.RegNo,'<br>',ShipperAdd.Address1,' ',ShipperNT.Name,'<br>','Tel: ', ShipperCT.Tel,' Fax: ',ShipperCT.Fax,' Email: ', ShipperCT.Email) AS ShipperFull,
	                               CONCAT(consignee.Name,'<br>', consignee.RegNo,'<br>',ConsigneeAdd.Address1,' ',ConsigneeNT.Name,'<br>','Tel: ', ConsigneeCT.Tel,' Fax: ',ConsigneeCT.Fax,' Email: ', ConsigneeCT.Email) AS Customer,
	                               CONCAT(Notify.Name,'<br>', Notify.RegNo,'<br>',NotifyAdd.Address1,' ',NotifyNT.Name,'<br>','Tel: ', NotifyCT.Tel,' Fax: ',NotifyCT.Fax,' Email: ', NotifyCT.Email) AS NotifyParty,
CONCAT(DAgent.Name,'<br>', DAgent.RegNo,'<br>',DAgentAdd.Address1,' ',DAgentNT.Name,'<br>','Tel: ', DAgentCT.Tel,' Fax: ',DAgentCT.Fax,' Email: ', DAgentCT.Email) AS DeliveryAgent,
	                               HBL.HouseBL,
	                               CONCAT(VS.Name,' / ',SP.VoyageNo) AS Vessel,
	                               FI.NAME AS FreightIndicator,
	                               PlaceofDelivery.Name AS PlaceOfDelivery,
	                               PofLoading.Code AS PortOfLoading,
	                               PlaceofLoading.Name AS PlaceOfLoading,
	                               SPAgent.Name AS ShippingAgent,
								   PortOrigin.Name AS PlaceOfReceipt,
								   PortUnloading.Code AS PortOfUnloading,
PortOfDeparture.Name AS PlaceOfDeparture,
PortOfDeparture.Code AS PortOfDeparture,
PortOfDestination.Name AS PlaceOfDestination,
PortOfDestination.Code AS PortOfDestination,
								   HBL.ShippingMark,
								   HBL.[Description],
								   HBL.[Weight],
								   HBL.Measurement,
SP.MasterBL,
CONVERT(VARCHAR(10), SP.DateDeparture, 103) AS DateDeparture,
BLState.Name AS BLReleaseState
                            FROM HouseBL AS HBL
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            INNER JOIN Party AS Shipper ON Shipper.Id = HBL.ShipperId
								LEFT JOIN [Address] AS ShipperAdd ON ShipperAdd.Id = Shipper.AddressId
	                            LEFT JOIN Nationality AS ShipperNT ON ShipperNT.Id = ShipperAdd.NationalityId
	                            LEFT JOIN Contact AS ShipperCT ON ShipperCT.Id = Shipper.ContactId
                            INNER JOIN Party AS Consignee ON Consignee.Id = HBL.CustomerId
	                            LEFT JOIN [Address] AS ConsigneeAdd ON ConsigneeAdd.Id = Consignee.AddressId
	                            LEFT JOIN Nationality AS ConsigneeNT ON ConsigneeNT.Id = ConsigneeAdd.NationalityId
	                            LEFT JOIN Contact AS ConsigneeCT ON ConsigneeCT.Id = Consignee.ContactId
                            LEFT JOIN Party AS Notify ON Notify.Id = HBL.NotifyPartyId
	                            LEFT JOIN [Address] AS NotifyAdd ON NotifyAdd.Id = Notify.AddressId
	                            LEFT JOIN Nationality AS NotifyNT ON NotifyNT.Id = NotifyAdd.NationalityId
	                            LEFT JOIN Contact AS NotifyCT ON NotifyCT.Id = Notify.ContactId
                            INNER JOIN Party AS DAgent ON DAgent.Id =   SP.DeliveryAgentId
								LEFT JOIN [Address] AS DAgentAdd ON DAgentAdd.Id = DAgent.AddressId
	                            LEFT JOIN Nationality AS DAgentNT ON DAgentNT.Id = DAgentAdd.NationalityId
	                            LEFT JOIN Contact AS DAgentCT ON DAgentCT.Id = DAgent.ContactId
                            INNER JOIN Vessel AS VS ON VS.Id = SP.VesselId
                            INNER JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                            INNER JOIN Port AS PlaceofDelivery ON PlaceofDelivery.Id = HBL.PortOfDelivery
                            INNER JOIN Port AS PofLoading ON PofLoading.Id = HBL.PortOfLoading
                            INNER JOIN Port AS PlaceofLoading ON PlaceofLoading.Id = HBL.PortOfLoading
							LEFT JOIN Port AS PortOrigin ON PortOrigin.Id = HBL.PortOfOrigin
							LEFT JOIN Port AS PortUnloading ON PortUnloading.Id = HBL.PortOfUnloading
LEFT JOIN Port AS PortOfDeparture ON PortOfDeparture.Id = SP.PortOfDeparture
LEFT JOIN Port AS PortOfDestination ON PortOfDestination.Id = SP.PortOfDestination
                            LEFT JOIN Party AS SPAgent ON SPAgent.Id = SP.ShippingAgentId
LEFT join BLState ON BLState.Id = HBL.BLStateId
                            WHERE HBL.Id = @Id";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = HBLid;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("Shipper", reader["Shipper"].ToString());
                dictionary.Add("ShipperFull", reader["ShipperFull"].ToString());
                dictionary.Add("Customer", reader["Customer"].ToString());
                dictionary.Add("NotifyParty", reader["NotifyParty"].ToString());
                dictionary.Add("HouseBL", reader["HouseBL"].ToString());
                dictionary.Add("Vessel", reader["Vessel"].ToString());
                dictionary.Add("FreightIndicator", reader["FreightIndicator"].ToString());
                dictionary.Add("PlaceOfDelivery", reader["PlaceOfDelivery"].ToString());
                dictionary.Add("PortOfLoading", reader["PortOfLoading"].ToString());
                dictionary.Add("PlaceOfLoading", reader["PlaceOfLoading"].ToString());
                dictionary.Add("ShippingAgent", reader["ShippingAgent"].ToString());
                dictionary.Add("PlaceOfReceipt", reader["PlaceOfReceipt"].ToString());
                dictionary.Add("PortOfUnloading", reader["PortOfUnloading"].ToString());
                dictionary.Add("ShippingMark", reader["ShippingMark"].ToString());
                dictionary.Add("Description", reader["Description"].ToString());
                dictionary.Add("Weight", reader["Weight"].ToString());
                dictionary.Add("Measurement", reader["Measurement"].ToString());
                dictionary.Add("MasterBL", reader["MasterBL"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("BLReleaseState", reader["BLReleaseState"].ToString());
                dictionary.Add("DeliveryAgent", reader["DeliveryAgent"].ToString());

                dictionary.Add("PlaceOfDeparture", reader["PlaceOfDeparture"].ToString());
                dictionary.Add("PortOfDeparture", reader["PortOfDeparture"].ToString());
                dictionary.Add("PlaceOfDestination", reader["PlaceOfDestination"].ToString());
                dictionary.Add("PortOfDestination", reader["PortOfDestination"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GETAWBL(int HBLid)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT shipper.Name AS Shipper,
                                   CONCAT(Shipper.Name,'<br>', Shipper.RegNo,'<br>',ShipperAdd.Address1,' ',ShipperNT.Name,'<br>','Tel: ', ShipperCT.Tel,' Fax: ',ShipperCT.Fax,' Email: ', ShipperCT.Email) AS ShipperFull,
	                               CONCAT(consignee.Name,'<br>', consignee.RegNo,'<br>',ConsigneeAdd.Address1,' ',ConsigneeNT.Name,'<br>','Tel: ', ConsigneeCT.Tel,' Fax: ',ConsigneeCT.Fax,' Email: ', ConsigneeCT.Email) AS Customer,
	                            --   CONCAT(Notify.Name,'<br>', Notify.RegNo,'<br>',NotifyAdd.Address1,' ',NotifyNT.Name,'<br>','Tel: ', NotifyCT.Tel,' Fax: ',NotifyCT.Fax,' Email: ', NotifyCT.Email) AS NotifyParty,
CONCAT(DAgent.Name,'<br>', DAgent.RegNo,'<br>',DAgentAdd.Address1,' ',DAgentNT.Name,'<br>','Tel: ', DAgentCT.Tel,' Fax: ',DAgentCT.Fax,' Email: ', DAgentCT.Email) AS DeliveryAgent,
	                               HBL.HouseBL,
	                               CONCAT(VS.Name,' / ',SP.VoyageNo) AS Vessel,
	                               FI.Name AS FreightIndicator,
                                   FI.CODE AS FreightIndicatorC,
	                               --PlaceofDelivery.Name AS PlaceOfDelivery,
	                               --PofLoading.Code AS PortOfLoading,
	                               --PlaceofLoading.Name AS PlaceOfLoading,
	                               SPAgent.Name AS ShippingAgent,
								   --PortOrigin.Name AS PlaceOfReceipt,
								   --PortUnloading.Code AS PortOfUnloading,
PortOfDeparture.Name AS PlaceOfDeparture,
PortOfDeparture.Code AS PortOfDeparture,
PortOfDestination.[Name] AS PlaceOfDestination,
PortOfDestination.Code AS PortOfDestination,
								   HBL.ShippingMark,
								   HBL.[Description],
								   HBL.[Weight],
								   HBL.Measurement,
SP.MasterBL,
CONVERT(VARCHAR(10), SP.DateDeparture, 103) AS DateDeparture,
CONVERT(VARCHAR(10), SP.DateArrival, 103) AS DateArrival,
BLState.Name AS BLReleaseState
                            FROM HouseBL AS HBL
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            INNER JOIN Party AS Shipper ON Shipper.Id = HBL.ShipperId
								LEFT JOIN [Address] AS ShipperAdd ON ShipperAdd.Id = Shipper.AddressId
	                            LEFT JOIN Nationality AS ShipperNT ON ShipperNT.Id = ShipperAdd.NationalityId
	                            LEFT JOIN Contact AS ShipperCT ON ShipperCT.Id = Shipper.ContactId
                            INNER JOIN Party AS Consignee ON Consignee.Id = HBL.CustomerId
	                            LEFT JOIN [Address] AS ConsigneeAdd ON ConsigneeAdd.Id = Consignee.AddressId
	                            LEFT JOIN Nationality AS ConsigneeNT ON ConsigneeNT.Id = ConsigneeAdd.NationalityId
	                            LEFT JOIN Contact AS ConsigneeCT ON ConsigneeCT.Id = Consignee.ContactId
                           -- LEFT JOIN Party AS Notify ON Notify.Id = HBL.NotifyPartyId
	                        --    LEFT JOIN [Address] AS NotifyAdd ON NotifyAdd.Id = Notify.AddressId
	                          --  LEFT JOIN Nationality AS NotifyNT ON NotifyNT.Id = NotifyAdd.NationalityId
	                          --  LEFT JOIN Contact AS NotifyCT ON NotifyCT.Id = Notify.ContactId
                            INNER JOIN Party AS DAgent ON DAgent.Id =   SP.DeliveryAgentId
								LEFT JOIN [Address] AS DAgentAdd ON DAgentAdd.Id = DAgent.AddressId
	                            LEFT JOIN Nationality AS DAgentNT ON DAgentNT.Id = DAgentAdd.NationalityId
	                            LEFT JOIN Contact AS DAgentCT ON DAgentCT.Id = DAgent.ContactId
                            INNER JOIN Vessel AS VS ON VS.Id = SP.VesselId
                            INNER JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                            --INNER JOIN Port AS PlaceofDelivery ON PlaceofDelivery.Id = HBL.PortOfDelivery
                            --INNER JOIN Port AS PofLoading ON PofLoading.Id = HBL.PortOfLoading
                            --INNER JOIN Port AS PlaceofLoading ON PlaceofLoading.Id = HBL.PortOfLoading
							--LEFT JOIN Port AS PortOrigin ON PortOrigin.Id = HBL.PortOfOrigin
							--LEFT JOIN Port AS PortUnloading ON PortUnloading.Id = HBL.PortOfUnloading
LEFT JOIN Port AS PortOfDeparture ON PortOfDeparture.Id = SP.PortOfDeparture
LEFT JOIN Port AS PortOfDestination ON PortOfDestination.Id = SP.PortOfDestination
                            LEFT JOIN Party AS SPAgent ON SPAgent.Id = SP.ShippingAgentId
LEFT join BLState ON BLState.Id = HBL.BLStateId
                            WHERE HBL.Id = @Id";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = HBLid;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("Shipper", reader["Shipper"].ToString());
                dictionary.Add("ShipperFull", reader["ShipperFull"].ToString());
                dictionary.Add("Customer", reader["Customer"].ToString());
              //  dictionary.Add("NotifyParty", reader["NotifyParty"].ToString());
                dictionary.Add("HouseBL", reader["HouseBL"].ToString());
                dictionary.Add("Vessel", reader["Vessel"].ToString());
                dictionary.Add("FreightIndicator", reader["FreightIndicator"].ToString());
                dictionary.Add("FreightIndicatorC", reader["FreightIndicatorC"].ToString());
               // dictionary.Add("PlaceOfDelivery", reader["PlaceOfDelivery"].ToString());
              //  dictionary.Add("PortOfLoading", reader["PortOfLoading"].ToString());
              //  dictionary.Add("PlaceOfLoading", reader["PlaceOfLoading"].ToString());
                dictionary.Add("ShippingAgent", reader["ShippingAgent"].ToString());
             //   dictionary.Add("PlaceOfReceipt", reader["PlaceOfReceipt"].ToString());
               // dictionary.Add("PortOfUnloading", reader["PortOfUnloading"].ToString());
                dictionary.Add("ShippingMark", reader["ShippingMark"].ToString());
                dictionary.Add("Description", reader["Description"].ToString());
                dictionary.Add("Weight", reader["Weight"].ToString());
                dictionary.Add("Measurement", reader["Measurement"].ToString());
                dictionary.Add("MasterBL", reader["MasterBL"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("BLReleaseState", reader["BLReleaseState"].ToString());
                dictionary.Add("DeliveryAgent", reader["DeliveryAgent"].ToString());

                dictionary.Add("PlaceOfDeparture", reader["PlaceOfDeparture"].ToString());
                dictionary.Add("PortOfDeparture", reader["PortOfDeparture"].ToString());
                dictionary.Add("PlaceOfDestination", reader["PlaceOfDestination"].ToString());
                dictionary.Add("PortOfDestination", reader["PortOfDestination"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<Dictionary<Object, Object>> GetDailyClearance(string CLdate, int ModeofShipment)
        {
            List<Dictionary<object, object>> DictionaryList = new List<Dictionary<object, object>>();
           
            /*string sql = @"SELECT JB.JobNo,
	                      CONCAT(cust.[name],', ',cust.RegNo) AS Customer,
	                       HBL.HouseBL,
	                       HBL.Id AS HBLid,
	                       --container number form container data
	                       --Type/pkg/qty
	                       JB.RNumber,
	                       JBP.[Name] AS ShiftRequest,
	                       CM.[Name] AS ClearanceMode,
	                       CP.[Name] AS ClearancePort,
	                       CS.[Name] AS ClearanceShift,
	                       CONCAT(CLparty.[name],', ',CLparty.RegNo) AS ClearanceParty,
	                       JB.AssignStaffMCS,
	                       JB.AssignStaffMPL,
	                       JB.AssignStaffOffice,

	  (select ISNULL(CI.containerNo,'NIL') + ',' AS 'data()'
	    from ContainerInfo AS CI
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS ContainerNo,

	 (select ISNULL(CI.SealNo,'NIL') + ', ' AS 'data()'
	    from ContainerInfo AS CI
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS SealNo,

	(select ISNULL(CONVERT(VARCHAR(12), CI.Size),'NIL') + ', ' AS 'data()'
	    from ContainerInfo AS CI
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS Containersize,--

    (select ISNULL(CT.[Name],'NIL') + ', ' AS 'data()'
	    from ContainerInfo AS CI
		LEFT JOIN ContainerType AS CT ON CT.Id = CI.ContainerTypeId 
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS ContainerType,

    (select ISNULL(Cin.[Description],'NIL') + ', ' AS 'data()'
	    from ContainerInfo AS CI
		INNER JOIN ContainerIndicator AS Cin ON Cin.Id = CI.ContainerIndicatorId
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS ContainerIndicator,--

     (select ISNULL(CONVERT(VARCHAR(12), CI.CNoOfPackage),'NIL') + ', ' AS 'data()'
	    from ContainerInfo AS CI
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS CNoOfPackage,--

	(select ISNULL(Tpk.CODE,'NIL') + ', ' AS 'data()'
		from ContainerInfo AS CI
		INNER JOIN TypeofPackage AS Tpk ON Tpk.Id = CI.TypeofPackageId
		WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL AND CI.RemovedDate IS NULL
		order by HBL.Id asc
		FOR XML PATH('')) AS TypeofPackage,--

		CONCAT(HBL.Weight,HBL.Measurement) AS Measure,
        JB.DeliveryPlace,


	                       JB.ClearanceDate,
	                       SP.ModeofShipmentId
                    FROM Job AS JB
                    INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
                    INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId AND SP.DateRemoved IS NULL
                    INNER JOIN JobPriority AS JBP ON JBP.Id = JB.JobPriorityId
                    LEFT JOIN Party AS Cust On Cust.Id = HBL.CustomerId
                    LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
                    LEFT JOIN ClearancePort AS CP ON CP.Id = JB.ClearancePortId
                    LEFT JOIN ClearanceShift AS CS ON CS.Id = JB.ClearanceShiftId
                    LEFT JOIN Party AS CLparty ON CLparty.Id = JB.ClearancePartyId
                    WHERE JB.DateRemoved IS NULL AND SP.ModeofShipmentId = 1 AND CAST(JB.ClearanceDate AS DATE)= @CLdate
                    --AND Convert(DATE,JB.ClearanceDate)='2023-07-31'
                    ORDER BY JB.ClearanceDate DESC";*/
            string sql = @"SELECT JB.JobNo,
	                      CONCAT(cust.[name],', ',cust.RegNo) AS Customer,
	                       HBL.HouseBL,
	                       HBL.Id AS HBLid,
	                       --container number form container data
	                       --Type/pkg/qty
	                       JB.RNumber,
	                       JBP.[Name] AS ShiftRequest,
	                       CM.[Name] AS ClearanceMode,
	                       CP.[Name] AS ClearancePort,
	                       CS.[Name] AS ClearanceShift,
	                       CONCAT(CLparty.[name],', ',CLparty.RegNo) AS ClearanceParty,
	                       JB.AssignStaffMCS,
	                       JB.AssignStaffMPL,
	                       JB.AssignStaffOffice,

		CONCAT(HBL.Weight,' / ',HBL.Measurement) AS WightMeasure,
        JB.DeliveryPlace,


	                       JB.ClearanceDate,
	                       SP.ModeofShipmentId
                    FROM Job AS JB
                    INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
                    INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId AND SP.DateRemoved IS NULL
                    INNER JOIN JobPriority AS JBP ON JBP.Id = JB.JobPriorityId
                    LEFT JOIN Party AS Cust On Cust.Id = HBL.CustomerId
                    LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
                    LEFT JOIN ClearancePort AS CP ON CP.Id = JB.ClearancePortId
                    LEFT JOIN ClearanceShift AS CS ON CS.Id = JB.ClearanceShiftId
                    LEFT JOIN Party AS CLparty ON CLparty.Id = JB.ClearancePartyId
                    WHERE JB.DateRemoved IS NULL AND SP.ModeofShipmentId = 1 AND CAST(JB.ClearanceDate AS DATE)= @CLdate
                    --AND Convert(DATE,JB.ClearanceDate)='2023-07-31'
                    ORDER BY JB.ClearanceDate DESC";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ModeofShipment", DbType.Int32);
            param.Value = ModeofShipment;
            list.Add(param);
            param = new SqlParameter("@CLdate", DbType.String);
            param.Value = CLdate;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int HblId = 0;
            while (reader.Read())
            {
                Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
                dictionary.Add("JobNo", reader["JobNo"].ToString());
                dictionary.Add("Customer", reader["Customer"].ToString());
                dictionary.Add("HouseBL", reader["HouseBL"].ToString());
               /* dictionary.Add("ContainerNo", reader["ContainerNo"].ToString());

                dictionary.Add("ContainerInfo", reader["ContainerIndicator"].ToString()+'/'+
                                reader["Containersize"].ToString() + '/' +
                                reader["Measure"].ToString() + '/' +
                                reader["CNoOfPackage"].ToString() + '/' +
                                reader["TypeofPackage"].ToString()
                                );*/
                dictionary.Add("ContainerInfo", HouseBLModel.GetContainerData5(Convert.ToInt32(reader["HBLid"])));
                dictionary.Add("WightMeasure", reader["WightMeasure"].ToString());
                dictionary.Add("RNumber", reader["RNumber"].ToString());
                dictionary.Add("ShiftRequest", reader["ShiftRequest"].ToString());
                dictionary.Add("ClearanceMode", reader["ClearanceMode"].ToString());
                dictionary.Add("ClearancePort", reader["ClearancePort"].ToString());
                dictionary.Add("ClearanceShift", reader["ClearanceShift"].ToString());
                dictionary.Add("ClearanceParty", reader["ClearanceParty"].ToString());
                dictionary.Add("AssignStaffMCS", reader["AssignStaffMCS"].ToString());
                dictionary.Add("AssignStaffMPL", reader["AssignStaffMPL"].ToString());
                dictionary.Add("AssignStaffOffice", reader["AssignStaffOffice"].ToString());
                dictionary.Add("DeliveryPlace", reader["DeliveryPlace"].ToString());
                DictionaryList.Add(dictionary);
            }
            reader.Close();
            return DictionaryList;
        }

        public static Dictionary<object, object> GetArrivalNoticeInfo(int HBLid)
        {
            string sql = @"SELECT CONVERT(VARCHAR(10), SP.DateArrival, 103) AS DateArrival,
		                    CONCAT(P1.Name,ISNULL(', '+N1.Name,NULL)) AS Shipper,
		                    P2.Name AS CustomerName,
		                    A2.Address1 AS CustomerAddress,
		                    CONCAT(A2.Island,' ',N2.Name) AS CustomerNation,
		                    P2.RegNo AS CustomerRegNo,
		                    JB.JobNo,
		                    V.Name AS Vessel,
		                    HBL.HouseBL,
		                    SP.VoyageNo,
		                    PL.Name AS PortofLoading,
		                    SP.MasterBL,
		                    PD.Name AS PortofDelivery,
		                    FI.Name AS FreightIndicator,
		                    TC.Name AS TypeofCommodity,
		                    HBL.NoOfPackage,
		                    HBL.Measurement,
		                    HBL.[Weight]
                    FROM HouseBL AS HBL
                    INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                    LEFT JOIN Party AS P1 ON P1.Id = HBL.ShipperId
                    LEFT JOIN [Address] AS A1 ON A1.Id = P1.AddressId
                    LEFT JOIN Nationality AS N1 ON N1.Id = A1.NationalityId
                    LEFT JOIN Party AS P2 ON P2.Id = HBL.CustomerId
                    LEFT JOIN [Address] AS A2 ON A2.Id = P2.AddressId
                    LEFT JOIN Nationality AS N2 ON N2.Id = A2.NationalityId
                    LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                    LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
                    LEFT JOIN Port AS PL ON PL.Id = HBL.PortOfLoading
                    LEFT JOIN Port AS PD ON PD.Id = HBL.PortOfDelivery
                    LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                    LEFT JOIN TypeOfCommodity AS TC ON TC.Id = JB.TypeOfCommodityId
                    WHERE HBL.Id = @HBLid";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLid", DbType.Int32);
            param.Value = HBLid;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> ArrivalNotice = new Dictionary<object, object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    ArrivalNotice.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return ArrivalNotice;
        }

        public static Dictionary<object, object> GetDebitNoteInfo(int SPId)
        {
            string sql = @"SELECT 
		                    MF.Number AS ManifestNo,
		                    SP.MasterBL,
		                    V.Name AS VesselName,
		                    SP.VoyageNo
                    FROM shipment AS SP
                    LEFT JOIN Vessel AS V ON V.Id =  SP.VesselId
                    LEFT JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                    WHERE SP.Id = @SPId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SPId", DbType.Int32);
            param.Value = SPId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> DebitNote = new Dictionary<object, object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DebitNote.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return DebitNote;
        }

        public static Dictionary<object, object> GetPODInfo(int HBLid)
        {
            string sql = @"SELECT 
		                    MF.Number AS ManifestNo,
		                    SP.MasterBL,
		                    V.Name AS VesselName,
		                    SP.VoyageNo,
							HBL.HouseBL,
	                        PCUstomer.Name AS Consignee,
	                        FI.Name AS FreightIndicator,
	                        JB.JobNo
                        FROM HouseBL AS HBL
                        INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                        LEFT JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                        LEFT JOIN Vessel AS V ON V.Id =  SP.VesselId
                        LEFT JOIN Party AS PCUstomer ON PCUstomer.Id = HBL.CustomerId
                        LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                        LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                        WHERE HBL.Id = @HBLid";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLid", DbType.Int32);
            param.Value = HBLid;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> PODnote = new Dictionary<object, object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PODnote.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return PODnote;
        }

        public static string GetHBLList(int ShipmentId)
        {
            string sql = @"SELECT HBL.HouseBL,
	                        PCUstomer.Name AS Consignee,
	                        FI.Name AS FreightIndicator,
	                        JB.JobNo
                        FROM HouseBL AS HBL
                        LEFT JOIN Party AS PCUstomer ON PCUstomer.Id = HBL.CustomerId
                        LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                        LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
                        WHERE HBL.ShipmentId = @ShipmentId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = ShipmentId;
            list.Add(param);
            string HBLList = "";
            int Numbering = 0;
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Numbering++;
                HBLList += @"<tr>
					            <td width='10' valign='top'><font style='font-size: 12px;'>" + Numbering + @"</font></td>
					            <td width='80' valign='top'><font style='font-size: 12px;'>" + reader["HouseBL"] + @"</font></td>
					            <td width='180' valign='top'><font style='font-size: 12px;'>" + reader["Consignee"] + @"</font></td>
					            <td width='60' valign='top'><font style='font-size: 12px;'>" + reader["FreightIndicator"] + @"</font></td>
					            <td width='70' valign='top'><font style='font-size: 12px;'>" + reader["JobNo"] + @"</font></td>
                            </tr>";
            }
            reader.Close();
            return HBLList;
        }

        public static Dictionary<Object, Object> GetPODinfo(int HBLid)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT CONCAT(PCustomer.Name,' (',PCustomer.RegNo,')') AS Customer,
		                            PShipper.Name AS Shipper,
		                            CONCAT(EMP.FirstName,' ',EMP.LastName) AS Employee,
		                            CONCAT(BLLS.DOCollectedBy,' (',BLLS.DOCollectedContact,')') AS CollectedBy,
	                                CONVERT(VARCHAR(10), BLLS.DOCollectedDate, 103) AS DOCollectedDate,
									ISNULL(HBL.HouseBL,SP.MasterBL) AS BOLno,
									MF.Number AS ManifestNo,
									JB.JobNo,
									MOS.Name AS modeS
                            FROM HouseBL AS HBL
							INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLId = HBL.Id
                            INNER JOIN Employee AS EMP ON EMP.Id = BLLS.EmployeeId
							INNER JOIN ModeOfShipment AS MOS ON MOS.Id = SP.ModeofShipmentId
							LEFT JOIN Party AS PCustomer ON PCustomer.Id = HBL.CustomerId
                            LEFT JOIN Party AS PShipper ON PShipper.Id = HBL.CustomerId
							LEFT JOIN Manifest AS MF ON MF.Id =  SP.ManifestId
							LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.ID
                            WHERE HBL.Id = @Id";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = HBLid;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("Customer", reader["Customer"].ToString());
                dictionary.Add("Shipper", reader["Shipper"].ToString());
                dictionary.Add("Employee", reader["Employee"].ToString());
                dictionary.Add("CollectedBy", reader["CollectedBy"].ToString());
                dictionary.Add("DOCollectedDate", reader["DOCollectedDate"].ToString());
                dictionary.Add("BOLno", reader["BOLno"].ToString());
                dictionary.Add("ManifestNo", reader["ManifestNo"].ToString());
                dictionary.Add("JobNo", reader["JobNo"].ToString());
                dictionary.Add("modeS", reader["modeS"].ToString());
                dictionary.Add("IssueDate", DateTime.Now);
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> getLastJobInfo(int LastJobId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT J.Id, J.JobNo,BL.ShipmentId 
                            FROM job AS J
                            INNER JOIN HouseBL AS BL ON BL.Id = J.HouseBLId
                            WHERE J.Id = @LastJobId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@LastJobId", DbType.Int32);
            param.Value = LastJobId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("LastJid", reader["Id"].ToString());
                dictionary.Add("LastJnumber", reader["JobNo"].ToString());
                dictionary.Add("lastShipmentId", reader["ShipmentId"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public bool SaveShipmentStatus(int shipmentId, int ShipmentStatusId)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT ShipmentId 
		                        FROM ShipmentStatusHistory
		                        WHERE ShipmentStatusId = @ShipmentStatusId AND ShipmentId = @ShipmentId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[ShipmentStatusHistory]
											                           ([ShipmentId]
											                           ,[ShipmentStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId])
										                         VALUES
											                           (@ShipmentId
											                           ,@ShipmentStatusId
											                           ,GETDATE()
											                           ,@EmployeeId)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            List.Add(param);
            param = new SqlParameter("@ShipmentStatusId", DbType.Int32);
            param.Value = ShipmentStatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveShipmentReview(int shipmentId, int ShipmentStatusId)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT ShipmentId 
		                        FROM ShipmentStatusReview
		                        WHERE ShipmentStatusId = @ShipmentStatusId AND ShipmentId = @ShipmentId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[ShipmentStatusReview]
											                           ([ShipmentId]
											                           ,[ShipmentStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId])
										                         VALUES
											                           (@ShipmentId
											                           ,@ShipmentStatusId
											                           ,GETDATE()
											                           ,@EmployeeId)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            List.Add(param);
            param = new SqlParameter("@ShipmentStatusId", DbType.Int32);
            param.Value = ShipmentStatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveBLstatusReview(int HBLId, int HBLStatusId)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT HouseBLId 
		                        FROM BLStatusReview
		                        WHERE HouseBLId = @HBLId AND BLStatusId = @HBLStatusId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[BLStatusReview]
											                           ([HouseBLId]
											                           ,[BLStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId])
										                         VALUES
											                           (@HBLId
											                           ,@HBLStatusId
											                           ,GETDATE()
											                           ,@EmployeeId)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            param = new SqlParameter("@HBLStatusId", DbType.Int32);
            param.Value = HBLStatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateShipmentStatusNEW(int shipmentId, int ShipmentStatusId)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT ShipmentId 
		                        FROM ShipmentStatusHistory
		                        WHERE ShipmentId = @ShipmentId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[ShipmentStatusHistory]
											                           ([ShipmentId]
											                           ,[ShipmentStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId])
										                         VALUES
											                           (@ShipmentId
											                           ,@ShipmentStatusId
											                           ,GETDATE()
											                           ,@EmployeeId)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            List.Add(param);
            param = new SqlParameter("@ShipmentStatusId", DbType.Int32);
            param.Value = ShipmentStatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }
        public bool UpdateShipmentStatusPay(int shipmentId, int ShipmentStatusId)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT ShipmentId 
		                        FROM ShipmentLastStatus
		                        WHERE ShipmentId = @ShipmentId AND ShipmentStatusId >= @ShipmentStatusId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[ShipmentStatusHistory]
											                           ([ShipmentId]
											                           ,[ShipmentStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId])
										                         VALUES
											                           (@ShipmentId
											                           ,@ShipmentStatusId
											                           ,GETDATE()
											                           ,@EmployeeId)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            List.Add(param);
            param = new SqlParameter("@ShipmentStatusId", DbType.Int32);
            param.Value = ShipmentStatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetBLbyStatus(int StatusId)
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @"SELECT MF.Id,
                                   SP.Id AS ShipmentId,
	                               MF.Number,
                                   HBL.Id AS HBLId,
	                               SP.MasterBL,
	                               HBL.HouseBL,
	                               --CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
	                               SP.VoyageNo, 
	                               VS.Name AS VesselName,
	                               --Ctype.Name AS ContainerType,
	                              -- SP.TotalNoOfContainer,
	                               Convert(nvarchar(10),BLLS.DateCreated,103) AS DateCreated,
	                               Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
								   BLLS.BLStatusId,
								   SP.ModeOfShipmentId,
								   MOS.Name AS ModeOfShipmentName,
			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
BLS.Name AS RecentStatus
                            FROM Manifest AS MF
                            INNER JOIN Shipment AS SP ON SP.ManifestId = MF.Id AND SP.DateRemoved IS NULL
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
							INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId = @StatusId AND BLLS.BLId = HBL.Id --AND BLLS.DOAttached IS NULL
LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
							INNER JOIN ModeOfShipment AS MOS ON MOS.Id = SP.ModeofShipmentId
							LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
INNER JOIN Employee AS EMP ON EMP.Id = MF.CreatedBy
LEFT JOIN Contact AS C ON C.Id = P.ContactId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@StatusId", DbType.Int32);
            param.Value = StatusId;
            spList.Add(param);
            if (StatusId == 3)
            {
                sql = sql.Replace("--AND BLLS.DOAttached IS NULL", "AND BLLS.DOAttached IS NULL");
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Manifest.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> FilterManifest(DateTime StartDate, DateTime EndDate)
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @"select MF.Id,
	                               SP.Id AS ShipmentId,
								   SP.ModeofShipmentId,
	                               MF.Number,
	                               SP.VoyageNo,
	                               V.Name AS VesselName,
	                               MS.Name AS ModeOfShipmentName,
SPS.Name AS RecentStatus,
	                               Convert(nvarchar(10),MF.CreatedDate,103) AS DateCreated,
			(select ISNULL(HBL.HouseBL,'NIL') + ', ' AS 'data()'
	                                from HouseBL AS HBL
		                            WHERE HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS HouseBL,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee
                            from Manifest AS MF
                            INNER JOIN Shipment AS SP ON MF.Id = SP.ManifestId
                            LEFT JOIN Vessel AS V ON SP.VesselId = V.Id
                            LEFT JOIN ModeOfShipment AS MS ON SP.ModeofShipmentId = MS.Id
							LEFT JOIN ShipmentLastStatus AS SS ON SS.ShipmentId = SP.Id
							LEFT JOIN ShipmentStatus SPS ON SPS.Id = SS.ShipmentStatusId
LEFT JOIN Employee AS EMP ON EMP.Id = MF.CreatedBy
                            where MF.CreatedDate Between @StartDate AND @EndDate
ORDER BY MF.CreatedDate DESC";

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
                Manifest.Add("Customer", GetShipmentCustomer(Convert.ToInt32(reader["ShipmentId"])));
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> FilterManifestBy(string query)
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @"select MF.Id,
	                               SP.Id AS ShipmentId,
								   SP.ModeofShipmentId,
	                               MF.Number,
	                               SP.VoyageNo,
	                               V.Name AS VesselName,
	                               MS.Name AS ModeOfShipmentName,
 SPS.Name AS RecentStatus,
	                               Convert(nvarchar(10),MF.CreatedDate,103) AS DateCreated,
			(select ISNULL(HBL.HouseBL,'NIL') + ', ' AS 'data()'
	                                from HouseBL AS HBL
		                            WHERE HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS HouseBL,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee
                            from Manifest AS MF
                            INNER JOIN Shipment AS SP ON MF.Id = SP.ManifestId
                            LEFT JOIN Vessel AS V ON SP.VesselId = V.Id
                            LEFT JOIN ModeOfShipment AS MS ON SP.ModeofShipmentId = MS.Id
							LEFT JOIN ShipmentLastStatus AS SS ON SS.ShipmentId = SP.Id
							LEFT JOIN ShipmentStatus SPS ON SPS.Id = SS.ShipmentStatusId
LEFT JOIN Employee AS EMP ON EMP.Id = MF.CreatedBy
                            WHERE 
                                    (
                                    SP.VoyageNo like concat('%',@query,'%')
                                    OR
                                    SP.MasterBL like concat('%',@query,'%')
									OR
									V.Name like concat('%',@query,'%')
									OR
									MF.Number like concat('%',@query,'%')
									)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                Manifest.Add("Customer", GetShipmentCustomer(Convert.ToInt32(reader["ShipmentId"])));
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> GetShipmentByStatus(int StatusId)
        {

            List<Dictionary<object, object>> ShipmentList = new List<Dictionary<object, object>>();

            string sql = @"SELECT MF.Id,
	                               SP.Id AS ShipmentId,
								   SP.ModeofShipmentId,
                                   SPLS.RequestedPayment,
	                               MF.Number,
								   SP.EntryTypeId,
								   ET.Name AS EntryType,
	                               SP.VoyageNo,
	                               V.Name AS VesselName,
	                               MS.Name AS ModeOfShipmentName,
								   P.Name AS ShippinfAgent,
                                   CONCAT(SP.DebitNoteNo,', ',SP.DebitNoteAmount,'',SP.CurrencyType) AS PayInfo,
	                               Convert(nvarchar(10),MF.CreatedDate,103) AS DateCreated,
			(select ISNULL(HBL.HouseBL,'NIL') + ', ' AS 'data()'
	                                from HouseBL AS HBL
		                            WHERE HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS HouseBL,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
SPS.Name AS RecentStatus
                            FROM ShipmentLastStatus AS SPLS
                            INNER JOIN Shipment SP ON SP.Id = SPLS.ShipmentId
                            INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
                            LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
							LEFT JOIN Party AS P ON P.Id = SP.ShippingAgentId
INNER JOIN Employee AS EMP ON EMP.Id = MF.CreatedBy
LEFT JOIN EntryType AS ET ON ET.Id = SP.EntryTypeId
LEFT JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
                            WHERE SPLS.ShipmentStatusId = @ShipmentStatusId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ShipmentStatusId", DbType.Int32);
            param.Value = StatusId;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Shipment = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Shipment.Add(reader.GetName(i), reader.GetValue(i));
                }
                Shipment.Add("Customer", GetShipmentCustomer(Convert.ToInt32(reader["ShipmentId"])));
                ShipmentList.Add(Shipment);
            }
            reader.Close();
            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ShipmentList);
            return dataTable;
        }

        public static Dictionary<object, object> GetShipmentByStatus(int SPstatus, int BLstatus)
        {

            List<Dictionary<object, object>> BOLList = new List<Dictionary<object, object>>();

            string sql = @"SELECT MF.Id,
                                   SP.Id AS ShipmentId,
	                               MF.Number,
                                   HBL.Id AS HBLId,
	                               SP.MasterBL,
	                               HBL.HouseBL,
	                               --CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
	                               SP.VoyageNo, 
	                               VS.Name AS VesselName,
	                               --Ctype.Name AS ContainerType,
	                              -- SP.TotalNoOfContainer,
	                               Convert(nvarchar(10),BLLS.DateCreated,103) AS DateCreated,
	                               Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
								   BLLS.BLStatusId,
								   SP.ModeOfShipmentId,
								   MOS.Name AS ModeOfShipmentName,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
SPS.Name AS RecentStatus
                        FROM ShipmentLastStatus AS SPLS
                        INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId AND SP.DateRemoved IS NULL
                        INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
                        INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                        INNER JOIN BLLastStatus AS BLLS ON BLLS.BLId = HBL.Id AND BLLS.BLStatusId = @BLstatus
                        INNER JOIN ModeOfShipment AS MOS ON MOS.Id = SP.ModeofShipmentId
                        LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
                        LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Job AS JB ON JB.HouseBLId = HBL.Id
LEFT JOIN Contact AS C ON C.Id = P.ContactId
INNER JOIN Employee AS EMP ON EMP.Id = MF.CreatedBy
LEFT JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
                        where SPLS.ShipmentStatusId = @SPstatus AND JB.JobNo IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SPstatus", DbType.Int32);
            param.Value = SPstatus;
            spList.Add(param);
            param = new SqlParameter("@BLstatus", DbType.Int32);
            param.Value = BLstatus;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Manifest.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                BOLList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", BOLList);
            return dataTable;
        }

        public static string GetShipmentCustomer(int ShipmentId)
        {

            String Parties = "";

            /*string sql = @"select CONCAT(P.Name,' (',P.RegNo,')') AS Customer
                            from Shipment AS SP
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                            INNER JOIN Party AS P ON P.Id =  HBL.CustomerId
                            WHERE SP.Id = @ShipmentId";*/
            string sql = @"select CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer
                            from Shipment AS SP
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                            INNER JOIN Party AS P ON P.Id =  HBL.CustomerId
							LEFT JOIN Contact AS C ON C.Id = P.ContactId
                            WHERE SP.Id = @ShipmentId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Shipment = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Parties = Parties + " " + reader.GetValue(i);
                }
            }
            reader.Close();
            return Parties;
        }

        public static Dictionary<object, object> FilterJob(DateTime StartDate, DateTime EndDate)
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

           /* string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                           -- CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                    Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
BLS.Name AS RecentStatus
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN BLLastStatus AS BLLS ON BLLS.BLId = JB.HouseBLId
								LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
								where JB.RegistrationDate Between @StartDate AND @EndDate
ORDER BY JB.RegistrationDate DESC";*/
            string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                           -- CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
SP.VoyageNo,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                    Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
BLS.Name AS RecentStatus,
JB.RNumber,

			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN BLLastStatus AS BLLS ON BLLS.BLId = JB.HouseBLId
								LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
							LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
								where JB.RegistrationDate Between @StartDate AND @EndDate
ORDER BY JB.RegistrationDate DESC";

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
                Dictionary<object, object> Job = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Job.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                //Manifest.Add("DateDemurrageRed", "<font color='red'>"+reader.GetValue(i)+"</font>");
                                Job.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Job.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Job.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Job.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Job);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", JobList);
            return dataTable;
        }

        public static Dictionary<object, object> FilterJobBy(string query)
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

            string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                            --CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
SP.VoyageNo,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                    Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
BLS.Name AS RecentStatus,
JB.RNumber,
			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee

	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN BLLastStatus AS BLLS ON BLLS.BLId = JB.HouseBLId
								LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
							LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
								WHERE 
                                    (
                                    JB.JobNo like concat('%',@query,'%')
                                    OR
                                    HBL.HouseBL like concat('%',@query,'%')
									OR
									SP.MasterBL like concat('%',@query,'%')
									OR
									SP.VoyageNo like concat('%',@query,'%')
									OR
									VS.Name like concat('%',@query,'%')
									)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Job = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Job.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                //Manifest.Add("DateDemurrageRed", "<font color='red'>"+reader.GetValue(i)+"</font>");
                                Job.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Job.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Job.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Job.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Job);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", JobList);
            return dataTable;
        }

        public static string GetContainerNo(int HBLid)
        {
            string sql = "select ContainerNo from ContainerInfo where HouseBLId = @HBLid";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter();

            param = new SqlParameter("@HBLid", DbType.Int32) { Value = HBLid };
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            string ContainerList = "";
            while (reader.Read())
            {
                ContainerList += (reader["ContainerNo"].ToString()) + ", ";
            }
            reader.Close();

            return ContainerList;
        }

        public static string GetContainerTypeofPackage(int HBLid)
        {
            string sql = @"select TP.Name from ContainerInfo AS CI
                            LEFT JOIN TypeofPackage AS TP ON TP.Id = CI.TypeofPackageId
                            where CI.HouseBLId = @HBLid";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter();

            param = new SqlParameter("@HBLid", DbType.Int32) { Value = HBLid };
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            string TypeofPackageList = "";
            while (reader.Read())
            {
                TypeofPackageList += (reader["Name"].ToString()) + ", ";
            }
            reader.Close();

            return TypeofPackageList;
        }

        public static Dictionary<object, object> GetJobbyStatus(int StatusId)
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @" SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                           -- CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            --HBL.HouseBL,
SP.VoyageNo,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
		                            VS.Name AS VesselName,
		                           -- Ctype.Name AS ContainerType,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                    --Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
JB.RNumber,

			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(JB.RNumber,' ',Convert(nvarchar(10),JB.RNumberReqDate,103)) AS RNumberDate,
CONCAT(JB.ANumber,' ',Convert(nvarchar(10),JB.ANumberReqDate,103)) AS ANumberDate,
Convert(nvarchar(10),JB.ShiftingRequestedDate,103) AS ShiftingRequestedDate,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
BLS.Name AS RecentStatus
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId = @StatusId AND BLLS.BLId = HBL.Id
LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
	                           -- INNER JOIN ContainerType AS Ctype ON Ctype.Id = HBL.ContainerTypeId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@StatusId", DbType.Int32);
            param.Value = StatusId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Manifest.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Manifest.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                //Manifest.Add("DateDemurrageRed", "<font color='red'>"+reader.GetValue(i)+"</font>");
                                Manifest.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Manifest.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Manifest.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> GetNewJobs(int StatusId)
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @" SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                           -- CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            --HBL.HouseBL,
SP.VoyageNo,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
		                            VS.Name AS VesselName,
		                           -- Ctype.Name AS ContainerType,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                    --Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
                                    JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
JB.RNumber,
			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(JB.RNumber,' ',Convert(nvarchar(10),JB.RNumberReqDate,103)) AS RNumberDate,
CONCAT(JB.ANumber,' ',Convert(nvarchar(10),JB.ANumberReqDate,103)) AS ANumberDate,
Convert(nvarchar(10),JB.ShiftingRequestedDate,103) AS ShiftingRequestedDate,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
BLS.Name AS RecentStatus
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                            INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId = @StatusId AND BLLS.BLId = HBL.Id
LEFT JOIN BLStatus BLS ON BLS.Id = BLLS.BLStatusId
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
	                           -- INNER JOIN ContainerType AS Ctype ON Ctype.Id = HBL.ContainerTypeId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								LEFT JOIN ShipmentLastStatus AS SPLS ON SPLS.ShipmentId = SP.Id
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
								where SPLS.ShipmentStatusId < 3 OR SPLS.ShipmentStatusId IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@StatusId", DbType.Int32);
            param.Value = StatusId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Manifest.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Manifest.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                //Manifest.Add("DateDemurrageRed", "<font color='red'>"+reader.GetValue(i)+"</font>");
                                Manifest.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Manifest.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Manifest.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> GetUnpaidJobs()
        {

            List<Dictionary<object, object>> ManifestList = new List<Dictionary<object, object>>();

            string sql = @" SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                            CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
		                            SP.MasterBL,
		                            --HBL.HouseBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
		                            VS.Name AS VesselName,
		                         --   Ctype.Name AS ContainerType,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
                                   -- Convert(nvarchar(10),JB.DateDemurrage,103) AS DateDemurrage,
                                    JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project
	                            FROM Job AS JB
	                            INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId AND HBL.DateRemoved IS NULL
	                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
	                            LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                         --   INNER JOIN BLLastStatus AS BLLS ON BLLS.BLStatusId = @StatusId AND BLLS.BLId = HBL.Id
	                            LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
	                          --  INNER JOIN ContainerType AS Ctype ON Ctype.Id = HBL.ContainerTypeId
	                            INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
	                            INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
	                            LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
								WHERE JB.FreightInvoice IS NULL AND HBL.FreightIndicatorId > 1 OR JB.ClearanceInvoice IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Manifest = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Manifest.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Manifest.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                // Manifest.Add("DateDemurrageRed", "<font color='red'>" + reader.GetValue(i) + "</font>");
                                Manifest.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                // Manifest.Add("DateDemurrageRed", reader.GetValue(i));
                                Manifest.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Manifest.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Manifest.Add(reader.GetName(i), reader.GetValue(i));
                }
                ManifestList.Add(Manifest);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ManifestList);
            return dataTable;
        }

        public static Dictionary<object, object> GetDOpayRequestedJobs()
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

            string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                            --CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
SP.VoyageNo,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
									JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
		                            Pro.Name AS Project,
JB.RNumber,

			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(JB.RNumber,' ',Convert(nvarchar(10),JB.RNumberReqDate,103)) AS RNumberDate,
CONCAT(JB.ANumber,' ',Convert(nvarchar(10),JB.ANumberReqDate,103)) AS ANumberDate,
Convert(nvarchar(10),JB.ShiftingRequestedDate,103) AS ShiftingRequestedDate,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
SPS.Name AS RecentStatus                       												
							FROM ShipmentLastStatus  SPLS
                            INNER JOIN ShipmentStatus SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId
							INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SPLS.ShipmentId
							INNER JOIN Job AS JB ON JB.HouseBLId = HBL.Id
							INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
							INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
						    LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
						    LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
							LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
                            WHERE SPLS.ShipmentStatusId  in (3)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Jobs = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Jobs.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Jobs.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                Jobs.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Jobs.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Jobs.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Jobs.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Jobs);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", JobList);
            return dataTable;
        }

        public static Dictionary<object, object> GetDOPendingJobs()
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

            string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                           -- CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
SP.VoyageNo,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
									JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
JB.RNumber,

			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(JB.RNumber,' ',Convert(nvarchar(10),JB.RNumberReqDate,103)) AS RNumberDate,
CONCAT(JB.ANumber,' ',Convert(nvarchar(10),JB.ANumberReqDate,103)) AS ANumberDate,
Convert(nvarchar(10),JB.ShiftingRequestedDate,103) AS ShiftingRequestedDate,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
BLS.Name AS RecentStatus
                            FROM BLLastStatusSkipped  LBLS
                            INNER JOIN BLStatus BLS ON BLS.Id = LBLS.BLStatusId
							INNER JOIN Job AS JB ON JB.HouseBLId = LBLS.BLId
							INNER JOIN HouseBL AS HBL ON HBL.Id = LBLS.BLId
						    INNER JOIN Shipment AS SP ON SP.Id = LBLS.ShipmentId
							INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
							INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
						    LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
						    LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
							LEFT JOIN Project AS Pro ON Pro.Id = HBL.ProjectId

							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
                            WHERE LBLS.BLStatusId  in (5)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Jobs = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Jobs.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Jobs.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                Jobs.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Jobs.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Jobs.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Jobs.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Jobs);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", JobList);
            return dataTable;
        }

        public static Dictionary<object, object> GetDOCollectedJobs()
        {

            List<Dictionary<object, object>> JobList = new List<Dictionary<object, object>>();

            string sql = @"SELECT JB.Id,
		                            SP.Id AS ShipmentId,
		                            JB.JobNo,
                                    HBL.Id AS HBLId,
		                            Convert(nvarchar(10),JB.RegistrationDate,103) AS RegistrationDate,
		                            --CONCAT(P.Name,' (',P.RegNo,')') AS Customer,
CONCAT(P.Name,' (',P.RegNo,')',' Contact:',C.Name,C.Tel,'/',C.Mobile,', ') AS Customer,
		                            SP.MasterBL,
		                            ISNULL(HBL.HouseBL,SP.MasterBL) AS HouseBL,
SP.VoyageNo,
		                            VS.Name AS VesselName,
	                                Convert(nvarchar(10),SP.DateArrival,103) AS DateArrival,
		                            Convert(nvarchar(10),JB.DateDischarge,103) AS DateDischarge,
									JB.DateDemurrage,
		                            MS.Name AS ShipmentMode,
		                            CT.Name AS Commodity,
JB.RNumber,

			(select ISNULL(CI.containerNo,'NIL') + ', ' AS 'data()'
	                                from ContainerInfo AS CI
		                            WHERE CI.HouseBLId = HBL.Id AND HBL.DateRemoved IS NULL
		                            order by HBL.Id asc
		                            FOR XML PATH('')) AS ContainerNo,

Convert(nvarchar(10),JB.ClearanceDate,103) AS ClearanceDate,
CM.Name AS ClearanceMode,
CP.Name AS ClearanceBy,
JB.DeliveryPlace,
JB.Remarks,
Cport.Name AS ClearancePort,
CONCAT(JB.RNumber,' ',Convert(nvarchar(10),JB.RNumberReqDate,103)) AS RNumberDate,
CONCAT(JB.ANumber,' ',Convert(nvarchar(10),JB.ANumberReqDate,103)) AS ANumberDate,
Convert(nvarchar(10),JB.ShiftingRequestedDate,103) AS ShiftingRequestedDate,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee,
BLS.Name AS RecentStatus
                            FROM ShipmentLastStatus AS SPLS
                            INNER JOIN Shipment AS SP ON SP.Id = SPLS.ShipmentId AND SP.DateRemoved IS NULL
                            INNER JOIN HouseBL AS HBL ON HBL.ShipmentId = SP.Id AND HBL.DateRemoved IS NULL
                            INNER JOIN BLLastStatusSkipped AS BLLSS ON BLLSS.BLId = HBL.Id AND BLLSS.BLStatusId in(8)
LEFT JOIN BLStatus BLS ON BLS.Id = BLLSS.BLStatusId
                            INNER JOIN ShipmentStatus AS SPS ON SPS.Id = SPLS.ShipmentStatusId
							INNER JOIN Job AS JB ON JB.HouseBLId = HBL.Id
							INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
							INNER JOIN TypeOfCommodity AS CT ON CT.Id = JB.TypeOfCommodityId
							LEFT JOIN Party AS P ON P.Id = HBL.CustomerId
LEFT JOIN Contact AS C ON C.Id = P.ContactId
							LEFT JOIN Vessel AS VS ON VS.Id = SP.VesselId
							LEFT JOIN ClearanceMode AS CM ON CM.Id = JB.ClearanceModeId
							LEFT JOIN Party AS CP ON CP.Id = JB.ClearancePartyId
LEFT JOIN ClearancePort AS Cport ON Cport.Id = JB.ClearancePortId
LEFT JOIN Employee AS EMP ON EMP.Id = JB.EmployeeId
                            where SPLS.ShipmentStatusId in (4)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Jobs = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("HBLId"))
                    {
                        Jobs.Add("ContainerInfo", GetContainerNo(Convert.ToInt32(reader.GetValue(i))));
                    }
                    if (reader.GetName(i).Equals("DateDemurrage"))
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            Jobs.Add("DateDemurrageRed", reader.GetValue(i));
                        }
                        else
                        {
                            DateTime DemmurageDate = Convert.ToDateTime(reader.GetValue(i));
                            if (DemmurageDate < DateTime.Now)
                            {
                                Jobs.Add("DateDemurrageRed", "<font color='red'>" + DemmurageDate.ToString("dd/MM/yyyy") + "</font>");
                            }
                            else
                            {
                                Jobs.Add("DateDemurrageRed", DemmurageDate.ToString("dd/MM/yyyy"));
                            }
                        }
                    }
                    if (reader.GetName(i).Equals("DateDischarge"))
                    {
                        Jobs.Add("DateDischargeGreen", reader.GetValue(i) == DBNull.Value ? "" : Convert.ToString("<font color='green'>" + reader.GetValue(i) + "</font>"));
                    }
                    Jobs.Add(reader.GetName(i), reader.GetValue(i));
                }
                JobList.Add(Jobs);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", JobList);
            return dataTable;
        }

        public static Dictionary<Object, Object> GetManifest(int MfId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT 
                           SP.[Id] AS ShipmentId
                          ,SP.ManifestId
						   ,MF.Number AS MFNumber
                          ,SP.[VoyageNo]
                          ,SP.[VesselId]
                          ,SP.[ModeofShipmentId]
                         -- ,SP.[NationalityId]
                          ,SP.[MasterBL]
                          ,SP.[PortOfDeparture]
                         ,CONVERT(VARCHAR(10), SP.DateDeparture, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateDeparture, 108) AS DateDeparture
						 ,CONVERT(VARCHAR(10), SP.DateArrival, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateArrival, 108) AS DateArrival
						 ,Convert(nvarchar(12),SP.DateDeparture,106) AS OnlyDateDeparture
						 ,Convert(nvarchar(12),SP.DateArrival,106) AS OnlyDateArrival
						 ,Convert(nvarchar(5),SP.DateArrival,108) AS TimeArrival
                          ,SP.[PortOfDestination]
                          ,SP.[MasterName]
                         -- ,SP.[TotalNoOfContainer]
                         -- ,SP.[TotalNoOfBL]
                         -- ,SP.[TotalNoOfPackages]
                          ,SP.[NETtonnage]
                          ,SP.[GROSStonnage]
						  ,SP.ShippingAgentId
,SP.DeliveryAgentId
                          ,SP.CustomOfficeId
						  ,PT.Name AS ShippingAgentName
,PTDA.Name AS DeliveryAgentName
						  ,V.Name AS VesselName
						--  ,N.ISO3 AS Nationality
						  ,PDepart.Code AS PDeparture
						  ,PDesti.Code AS PDestination
						  ,MS.Name AS ShipmentMODE
						  ,Convert(nvarchar(12),MF.CreatedDate,106) AS ManifestDate
                      FROM [dbo].[Shipment] AS SP
					  INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
					  LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
					 -- INNER JOIN Nationality AS N ON N.Id = SP.NationalityId
					  LEFT JOIN Port AS PDepart ON PDepart.Id = SP.PortOfDeparture
					  LEFT JOIN Port AS PDesti ON PDesti.Id = SP.PortOfDestination
                      INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
					  LEFT JOIN Party AS PT ON PT.Id = SP.ShippingAgentId
LEFT JOIN Party AS PTDA ON PTDA.Id = SP.DeliveryAgentId
                    WHERE SP.ManifestId = @MfId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@MfId", DbType.Int32);
            param.Value = MfId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ShipmentId", reader["ShipmentId"].ToString());
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("MFNumber", reader["MFNumber"].ToString());
                dictionary.Add("VoyageNo", reader["VoyageNo"].ToString());
                dictionary.Add("VesselId", reader["VesselId"].ToString());
                dictionary.Add("VesselName", reader["VesselName"].ToString());
                dictionary.Add("ModeofShipmentId", reader["ModeofShipmentId"].ToString());
                //   dictionary.Add("NationalityId", reader["NationalityId"].ToString());
                //    dictionary.Add("Nationality", reader["Nationality"].ToString());
                dictionary.Add("MasterBL", reader["MasterBL"].ToString());
                dictionary.Add("PortOfDeparture", reader["PortOfDeparture"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("OnlyDateDeparture", reader["OnlyDateDeparture"].ToString());
                dictionary.Add("OnlyDateArrival", reader["OnlyDateArrival"].ToString());
                dictionary.Add("TimeArrival", reader["TimeArrival"].ToString());
                dictionary.Add("PortOfDestination", reader["PortOfDestination"].ToString());
                dictionary.Add("DateArrival", reader["DateArrival"].ToString());
                dictionary.Add("MasterName", reader["MasterName"].ToString());
                dictionary.Add("TotalContainerCount", ContainerModel.CountShipmentContainer(Convert.ToInt32(reader["ShipmentId"])));
                /*dictionary.Add("TotalNoOfContainer", reader["TotalNoOfContainer"].ToString());
                dictionary.Add("TotalNoOfBL", reader["TotalNoOfBL"].ToString());
                dictionary.Add("TotalNoOfPackages", reader["TotalNoOfPackages"].ToString());*/
                dictionary.Add("NETtonnage", reader["NETtonnage"].ToString());
                dictionary.Add("GROSStonnage", reader["GROSStonnage"].ToString());
                dictionary.Add("PDeparture", reader["PDeparture"].ToString());
                dictionary.Add("PDestination", reader["PDestination"].ToString());
                dictionary.Add("ShipmentMODE", reader["ShipmentMODE"].ToString());
                dictionary.Add("ManifestDate", reader["ManifestDate"].ToString());
                dictionary.Add("ShippingAgentId", reader["ShippingAgentId"].ToString());
                dictionary.Add("DeliveryAgentId", reader["DeliveryAgentId"].ToString());
                dictionary.Add("CustomOfficeId", reader["CustomOfficeId"].ToString());
                dictionary.Add("DeliveryAgentName", reader["DeliveryAgentName"].ToString());
                dictionary.Add("ShippingAgentName", reader["ShippingAgentName"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetManifestData(int MfId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT 
                           SP.[Id] AS ShipmentId
                          ,SP.ManifestId
                          ,MF.Number AS ManifestNumber
						  ,CO.Code AS CustomCode
						  ,SP.[VoyageNo]
						  ,CONVERT(VARCHAR(10), SP.DateDeparture, 23) AS DateDeparture
						  ,CONVERT(VARCHAR(10), SP.DateArrival, 23) AS DateArrival
						  ,Convert(nvarchar(5),SP.DateArrival,108) AS TimeArrival
						  ,(select COUNT(Id) from HouseBL where ShipmentId = SP.Id AND DateRemoved IS NULL) AS NumberOfBL
						  ,(select SUM(sCI.CNoOfPackage) AS TotalPackage from HouseBL sHBL INNER JOIN ContainerInfo AS sCI ON sCI.HouseBLId = sHBL.Id AND sCI.RemovedDate IS NULL AND sHBL.DateRemoved IS NULL where sHBL.ShipmentId = SP.Id) AS TotalPackage
						  ,(select COUNT(DISTINCT sCI.ContainerNo) AS TotalContainer from HouseBL sHBL INNER JOIN ContainerInfo AS sCI ON sCI.HouseBLId = sHBL.Id  AND sCI.RemovedDate IS NULL AND sHBL.DateRemoved IS NULL where sHBL.ShipmentId = SP.Id) AS TotalContainer
						  ,(SELECT SUM([Weight])FROM HouseBL WHERE DateRemoved IS NULL AND ShipmentId = SP.Id) AS TotalWeight
						  ,V.Name AS CarrierName
						  ,VP.Name AS CarrierOperator
						  ,VN.ISO2 AS CarrierNation
						  ,ShAggent.Name AS ShippingAgentName
						  ,MS.CustomId AS ModeOfTransportCode
						  ,PDepart.Code AS PortDeparture
						  ,PDesti.Code AS PortDestination
						  ,SP.[NETtonnage]
                          ,SP.[GROSStonnage]
                      FROM [dbo].[Shipment] AS SP
					  INNER JOIN Manifest AS MF ON MF.Id = SP.ManifestId
					  LEFT JOIN CustomOffice AS CO ON CO.Id = SP.CustomOfficeId
					  LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
					  LEFT JOIN Party AS VP ON VP.Id = V.Company
					  LEFT JOIN Nationality AS VN ON VN.Id = V.RegCountry
					  LEFT JOIN Party AS ShAggent ON ShAggent.Id = SP.ShippingAgentId
					  INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
					  LEFT JOIN Port AS PDepart ON PDepart.Id = SP.PortOfDeparture
					  LEFT JOIN Port AS PDesti ON PDesti.Id = SP.PortOfDestination
                    WHERE SP.ManifestId = @MfId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@MfId", DbType.Int32);
            param.Value = MfId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ShipmentId", reader["ShipmentId"].ToString());
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("ManifestNumber", reader["ManifestNumber"].ToString());
                dictionary.Add("CustomCode", reader["CustomCode"].ToString());
                dictionary.Add("VoyageNo", reader["VoyageNo"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("DateArrival", reader["DateArrival"].ToString());
                dictionary.Add("TimeArrival", reader["TimeArrival"].ToString());
                dictionary.Add("NumberOfBL", reader["NumberOfBL"].ToString());
                dictionary.Add("TotalPackage", reader["TotalPackage"].ToString());
                dictionary.Add("TotalContainer", reader["TotalContainer"].ToString());
                dictionary.Add("TotalWeight", reader["TotalWeight"].ToString());
                dictionary.Add("CarrierName", reader["CarrierName"].ToString());
                dictionary.Add("CarrierOperator", reader["CarrierOperator"].ToString());
                dictionary.Add("CarrierNation", reader["CarrierNation"].ToString());
                dictionary.Add("ShippingAgentName", reader["ShippingAgentName"].ToString());
                dictionary.Add("ModeOfTransportCode", reader["ModeOfTransportCode"].ToString());
                dictionary.Add("PortDeparture", reader["PortDeparture"].ToString());
                dictionary.Add("PortDestination", reader["PortDestination"].ToString());
                dictionary.Add("NETtonnage", reader["NETtonnage"].ToString());
                dictionary.Add("GROSStonnage", reader["GROSStonnage"].ToString());

            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetShipment(int SPId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT 
                           SP.[Id] AS ShipmentId
                          ,SP.ManifestId
						   ,MF.Number AS MFNumber
                          ,SP.[VoyageNo]
                          ,SP.[VesselId]
                          ,SP.[ModeofShipmentId]
                         -- ,SP.[NationalityId]
                          ,SP.[MasterBL]
                          ,SP.[PortOfDeparture]
                         ,CONVERT(VARCHAR(10), SP.DateDeparture, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateDeparture, 108) AS DateDeparture
						 ,CONVERT(VARCHAR(10), SP.DateArrival, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateArrival, 108) AS DateArrival
						 ,Convert(nvarchar(12),SP.DateDeparture,106) AS OnlyDateDeparture
						 ,Convert(nvarchar(12),SP.DateArrival,106) AS OnlyDateArrival
						 ,Convert(nvarchar(5),SP.DateArrival,108) AS TimeArrival
                          ,SP.[PortOfDestination]
                          ,SP.[MasterName]
                         -- ,SP.[TotalNoOfContainer]
                         -- ,SP.[TotalNoOfBL]
                         -- ,SP.[TotalNoOfPackages]
                          ,SP.[NETtonnage]
                          ,SP.[GROSStonnage]
						  ,SP.ShippingAgentId
						  ,PT.Name AS ShippingAgentName
						  ,V.Name AS VesselName
						--  ,N.ISO3 AS Nationality
						  ,PDepart.Code AS PDeparture
						  ,PDesti.Code AS PDestination
						  ,MS.Name AS ShipmentMODE
						  ,Convert(nvarchar(12),MF.CreatedDate,106) AS ManifestDate
                      FROM [dbo].[Shipment] AS SP
					  LEFT JOIN Manifest AS MF ON MF.Id = SP.ManifestId
					  LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
					 -- INNER JOIN Nationality AS N ON N.Id = SP.NationalityId
					  LEFT JOIN Port AS PDepart ON PDepart.Id = SP.PortOfDeparture
					  LEFT JOIN Port AS PDesti ON PDesti.Id = SP.PortOfDestination
                      INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
                      LEFT JOIN Party AS PT ON PT.Id = SP.ShippingAgentId
                    WHERE SP.Id = @SPId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SPId", DbType.Int32);
            param.Value = SPId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ShipmentId", reader["ShipmentId"].ToString());
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("MFNumber", reader["MFNumber"].ToString());
                dictionary.Add("VoyageNo", reader["VoyageNo"].ToString());
                dictionary.Add("VesselId", reader["VesselId"].ToString());
                dictionary.Add("VesselName", reader["VesselName"].ToString());
                dictionary.Add("ModeofShipmentId", reader["ModeofShipmentId"].ToString());
                //   dictionary.Add("NationalityId", reader["NationalityId"].ToString());
                //    dictionary.Add("Nationality", reader["Nationality"].ToString());
                dictionary.Add("MasterBL", reader["MasterBL"].ToString());
                dictionary.Add("PortOfDeparture", reader["PortOfDeparture"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("OnlyDateDeparture", reader["OnlyDateDeparture"].ToString());
                dictionary.Add("OnlyDateArrival", reader["OnlyDateArrival"].ToString());
                dictionary.Add("TimeArrival", reader["TimeArrival"].ToString());
                dictionary.Add("PortOfDestination", reader["PortOfDestination"].ToString());
                dictionary.Add("DateArrival", reader["DateArrival"].ToString());
                dictionary.Add("MasterName", reader["MasterName"].ToString());
                dictionary.Add("TotalContainerCount", ContainerModel.CountShipmentContainer(Convert.ToInt32(reader["ShipmentId"])));
                /*dictionary.Add("TotalNoOfContainer", reader["TotalNoOfContainer"].ToString());
                dictionary.Add("TotalNoOfBL", reader["TotalNoOfBL"].ToString());
                dictionary.Add("TotalNoOfPackages", reader["TotalNoOfPackages"].ToString());*/
                dictionary.Add("NETtonnage", reader["NETtonnage"].ToString());
                dictionary.Add("GROSStonnage", reader["GROSStonnage"].ToString());
                dictionary.Add("PDeparture", reader["PDeparture"].ToString());
                dictionary.Add("PDestination", reader["PDestination"].ToString());
                dictionary.Add("ShipmentMODE", reader["ShipmentMODE"].ToString());
                dictionary.Add("ManifestDate", reader["ManifestDate"].ToString());
                dictionary.Add("ShippingAgentId", reader["ShippingAgentId"].ToString());
                dictionary.Add("ShippingAgentName", reader["ShippingAgentName"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetMasterBL(string MasterBLnumber)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT Id,ManifestId FROM Shipment WHERE DateRemoved IS NULL AND MasterBL = @MasterBLnumber";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@MasterBLnumber", DbType.String);
            param.Value = MasterBLnumber;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("ShipmentId", reader["Id"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetHouseBL(string HouseBLnumber)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            /* string sql = @"SELECT SP.Id,SP.ManifestId 
                             FROM HouseBL AS HBL
                             INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
                             WHERE HBL.DateRemoved IS NULL AND HBL.HouseBL = @HouseBLnumber";*/
            string sql = @"SELECT SP.Id,SP.ManifestId,J.Id AS JobId,HBL.Id AS HBLId
                            FROM HouseBL AS HBL
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
							LEFT JOIN Job AS J ON J.HouseBLId = HBL.Id
                            WHERE HBL.DateRemoved IS NULL AND HBL.HouseBL  = @HouseBLnumber";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HouseBLnumber", DbType.String);
            param.Value = HouseBLnumber;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("ShipmentId", reader["Id"].ToString());
                dictionary.Add("JobId", reader["JobId"].ToString());
                dictionary.Add("HBLId", reader["HBLId"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetHouseBLinfo(int HBLId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT SP.Id,SP.ManifestId,J.Id AS JobId,HBL.Id AS HBLId
                            FROM HouseBL AS HBL
                            INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
							LEFT JOIN Job AS J ON J.HouseBLId = HBL.Id
                            WHERE HBL.DateRemoved IS NULL AND HBL.Id  = @HBLId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.String);
            param.Value = HBLId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ManifestId", reader["ManifestId"].ToString());
                dictionary.Add("ShipmentId", reader["Id"].ToString());
                dictionary.Add("JobId", reader["JobId"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static bool ConvertJobToManifest(int ShipmentId)
        {
            string ManifestNumber = FormatManifestNumber();
            String sql = @"DECLARE @ManifestLastId INT
                            INSERT INTO [dbo].[Manifest]
                                       ([Number]
                                       ,[CreatedDate]
                                       ,[CreatedBy])
                                 VALUES
                                       (@Number
                                       ,GETDATE()
                                       ,@EmployeeId )
                            SELECT @ManifestLastId = SCOPE_IDENTITY();

                            UPDATE Shipment
                            SET ManifestId = @ManifestLastId
                            WHERE Id = @ShipmentId";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            List.Add(param);
            param = new SqlParameter("@Number", DbType.String);
            param.Value = ManifestNumber;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static Dictionary<Object, Object> GetJob(int JBId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT 
                           SP.[Id] AS ShipmentId
                          ,JB.Id AS JobId
						  ,JB.JobNo

						  ,CONVERT(VARCHAR(10), JB.RegistrationDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.RegistrationDate, 108) AS RegistrationDate
						  ,CONVERT(VARCHAR(10), JB.DateDemurrage, 103) + ' ' + CONVERT(VARCHAR(5), JB.DateDemurrage, 108) AS DateDemurrage
						  ,CONVERT(VARCHAR(10), JB.DateDischarge, 103) + ' ' + CONVERT(VARCHAR(5), JB.DateDischarge, 108) AS DateDischarge
						  ,JB.TypeOfCommodityId
						  ,HBL.ProjectId
						  ,JB.JobPriorityId
						--  ,JB.CNumber
						  ,JB.RNumber
,CONVERT(VARCHAR(10), JB.RNumberReqDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.RNumberReqDate, 108) AS RNumberReqDate
,JB.ANumber
,CONVERT(VARCHAR(10), JB.ANumberReqDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.ANumberReqDate, 108) AS ANumberReqDate
,CONVERT(VARCHAR(10), JB.ShiftingRequestedDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.ShiftingRequestedDate, 108) AS ShiftingRequestedDate
						  ,CONVERT(VARCHAR(10), JB.ClearanceDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.ClearanceDate, 108) AS ClearanceDate
						  ,JB.ClearancePartyId
						  ,JB.ClearanceModeId
						  ,JB.ClearanceVehicleId
						  ,JB.ClearanceShiftId
,JB.ClearancePortId
						  ,JB.DeliveryPlace
,JB.AssignStaffMCS
,JB.AssignStaffMPL
,JB.AssignStaffOffice

						  ,JB.Remarks
						 -- ,JB.DeliveryDate
,CONVERT(VARCHAR(10), JB.DeliveryDate, 103) + ' ' + CONVERT(VARCHAR(5), JB.DeliveryDate, 108) AS DeliveryDate
                          ,JB.CustomerReference
						  ,JB.NoOfClearedPackage
						  ,JB.NoOfDeliveredPackage
,JB.NoOfDamagePackage
						  ,JB.DeliveredBy
						  ,JB.ReceivedBy

                          ,SP.[VoyageNo]
                          ,SP.[VesselId]
                          ,SP.[ModeofShipmentId]
                       --   ,SP.[NationalityId]
                          ,SP.[MasterBL]
                          ,SP.[PortOfDeparture]
                         ,CONVERT(VARCHAR(10), SP.DateDeparture, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateDeparture, 108) AS DateDeparture
						 ,CONVERT(VARCHAR(10), SP.DateArrival, 103) + ' ' + CONVERT(VARCHAR(5), SP.DateArrival, 108) AS DateArrival
						 ,Convert(nvarchar(12),SP.DateDeparture,106) AS OnlyDateDeparture
						 ,Convert(nvarchar(12),SP.DateArrival,106) AS OnlyDateArrival
						 ,Convert(nvarchar(5),SP.DateArrival,108) AS TimeArrival
                          ,SP.[PortOfDestination]
                          ,SP.[MasterName]
                         -- ,SP.[TotalNoOfContainer]
                         -- ,SP.[TotalNoOfBL]
                         -- ,SP.[TotalNoOfPackages]
                          ,SP.[NETtonnage]
                          ,SP.[GROSStonnage]
						  ,SP.ShippingAgentId
,SP.DeliveryAgentId
						  ,V.Name AS VesselName
						--  ,N.ISO3 AS Nationality
						  ,PDepart.Code AS PDeparture
						  ,PDesti.Code AS PDestination
						  ,MS.Name AS ShipmentMODE
						  ,Pr.Name AS ProjectName
						  ,Pt.Name AS ClearancePartyName
						  ,Vc.Name AS ClearanceVehicleName
						  ,DParty.Name AS DeliveredByName
						  ,SAparty.Name AS ShippingAgentName
,PTDA.Name AS DeliveryAgentName
					FROM Job AS JB
					INNER JOIN HouseBL AS HBL ON HBL.Id = JB.HouseBLId
					INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
					  LEFT JOIN Vessel AS V ON V.Id = SP.VesselId
					--  INNER JOIN Nationality AS N ON N.Id = SP.NationalityId
					  LEFT JOIN Port AS PDepart ON PDepart.Id = SP.PortOfDeparture
					  LEFT JOIN Port AS PDesti ON PDesti.Id = SP.PortOfDestination
                      INNER JOIN ModeOfShipment AS MS ON MS.Id = SP.ModeofShipmentId
					  LEFT JOIN Project AS Pr ON Pr.Id = HBL.ProjectId
					  LEFT JOIN Party AS Pt ON Pt.Id = JB.ClearancePartyId
					  LEFT JOIN ClearanceVehicle AS Vc ON Vc.Id = JB.ClearanceVehicleId
					  LEFT JOIN Party AS DParty ON DParty.Id = JB.DeliveredBy
                      LEFT JOIN Party AS SAparty ON SAparty.Id = SP.ShippingAgentId
LEFT JOIN Party AS PTDA ON PTDA.Id = SP.DeliveryAgentId
                    WHERE JB.Id = @JBId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@JBId", DbType.Int32);
            param.Value = JBId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ShipmentId", reader["ShipmentId"].ToString());
                dictionary.Add("JobId", reader["JobId"].ToString());
                dictionary.Add("JobNo", reader["JobNo"].ToString());

                dictionary.Add("RegistrationDate", reader["RegistrationDate"].ToString());
                dictionary.Add("DateDemurrage", reader["DateDemurrage"].ToString());
                dictionary.Add("DateDischarge", reader["DateDischarge"].ToString());
                dictionary.Add("TypeOfCommodityId", reader["TypeOfCommodityId"].ToString());
                dictionary.Add("ProjectId", reader["ProjectId"].ToString());
                dictionary.Add("ProjectName", reader["ProjectName"].ToString());
                dictionary.Add("JobPriorityId", reader["JobPriorityId"].ToString());
                //   dictionary.Add("CNumber", reader["CNumber"].ToString());
                dictionary.Add("RNumber", reader["RNumber"].ToString());
                dictionary.Add("RNumberReqDate", reader["RNumberReqDate"].ToString());
                dictionary.Add("ANumber", reader["ANumber"].ToString());
                dictionary.Add("ANumberReqDate", reader["ANumberReqDate"].ToString());
                dictionary.Add("ShiftingRequestedDate", reader["ShiftingRequestedDate"].ToString());


                dictionary.Add("ClearanceDate", reader["ClearanceDate"].ToString());
                dictionary.Add("ClearancePartyId", reader["ClearancePartyId"].ToString());
                dictionary.Add("ClearancePartyName", reader["ClearancePartyName"].ToString());
                dictionary.Add("ClearanceModeId", reader["ClearanceModeId"].ToString());
                dictionary.Add("ClearanceVehicleId", reader["ClearanceVehicleId"].ToString());
                dictionary.Add("ClearanceVehicleName", reader["ClearanceVehicleName"].ToString());
                dictionary.Add("ClearanceShiftId", reader["ClearanceShiftId"].ToString());
                dictionary.Add("ClearancePortId", reader["ClearancePortId"].ToString());
                dictionary.Add("DeliveryPlace", reader["DeliveryPlace"].ToString());
                dictionary.Add("Remarks", reader["Remarks"].ToString());
                dictionary.Add("DeliveryDate", reader["DeliveryDate"].ToString());
                dictionary.Add("CustomerReference", reader["CustomerReference"].ToString());
                dictionary.Add("DeliveredBy", reader["DeliveredBy"].ToString());
                dictionary.Add("DeliveredByName", reader["DeliveredByName"].ToString());
                dictionary.Add("ReceivedBy", reader["ReceivedBy"].ToString());


                dictionary.Add("VoyageNo", reader["VoyageNo"].ToString());
                dictionary.Add("VesselId", reader["VesselId"].ToString());
                dictionary.Add("VesselName", reader["VesselName"].ToString());
                dictionary.Add("ModeofShipmentId", reader["ModeofShipmentId"].ToString());
                //   dictionary.Add("NationalityId", reader["NationalityId"].ToString());
                //     dictionary.Add("Nationality", reader["Nationality"].ToString());
                dictionary.Add("MasterBL", reader["MasterBL"].ToString());
                dictionary.Add("PortOfDeparture", reader["PortOfDeparture"].ToString());
                dictionary.Add("DateDeparture", reader["DateDeparture"].ToString());
                dictionary.Add("OnlyDateDeparture", reader["OnlyDateDeparture"].ToString());
                dictionary.Add("OnlyDateArrival", reader["OnlyDateArrival"].ToString());
                dictionary.Add("TimeArrival", reader["TimeArrival"].ToString());
                dictionary.Add("PortOfDestination", reader["PortOfDestination"].ToString());
                dictionary.Add("DateArrival", reader["DateArrival"].ToString());
                dictionary.Add("MasterName", reader["MasterName"].ToString());
                /*dictionary.Add("TotalNoOfContainer", reader["TotalNoOfContainer"].ToString());
                dictionary.Add("TotalNoOfBL", reader["TotalNoOfBL"].ToString());
                dictionary.Add("TotalNoOfPackages", reader["TotalNoOfPackages"].ToString());*/
                dictionary.Add("NETtonnage", reader["NETtonnage"].ToString());
                dictionary.Add("GROSStonnage", reader["GROSStonnage"].ToString());
                dictionary.Add("PDeparture", reader["PDeparture"].ToString());
                dictionary.Add("PDestination", reader["PDestination"].ToString());
                dictionary.Add("ShipmentMODE", reader["ShipmentMODE"].ToString());
                dictionary.Add("NoOfClearedPackage", reader["NoOfClearedPackage"].ToString());
                dictionary.Add("NoOfDeliveredPackage", reader["NoOfDeliveredPackage"].ToString());
                dictionary.Add("ShippingAgentId", reader["ShippingAgentId"].ToString());
                dictionary.Add("ShippingAgentName", reader["ShippingAgentName"].ToString());
                dictionary.Add("DeliveryAgentId", reader["DeliveryAgentId"].ToString());
                dictionary.Add("DeliveryAgentName", reader["DeliveryAgentName"].ToString());
                dictionary.Add("AssignStaffMCS", reader["AssignStaffMCS"].ToString());
                dictionary.Add("AssignStaffMPL", reader["AssignStaffMPL"].ToString());
                dictionary.Add("AssignStaffOffice", reader["AssignStaffOffice"].ToString());
                dictionary.Add("NoOfDamagePackage", reader["NoOfDamagePackage"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<Dictionary<object, object>> GetHBLIdList(int ShipmentId)
        {
            string sql = @"SELECT Id,HouseBL FROM HouseBL WHERE ShipmentId = @ShipmentId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = ShipmentId;
            list.Add(param);
            List<Dictionary<object, object>> HBLIdList = new List<Dictionary<object, object>>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Dictionary = new Dictionary<object, object>();
                Dictionary.Add("HBLid", reader["Id"]);
                HBLIdList.Add(Dictionary);
            }
            reader.Close();
            return HBLIdList;
        }

    }

    public class AutoComplete
    {
        public int id { get; set; }
        public string value { get; set; }
    }


    public class HouseBLModel
    {
        public int HouseBLId { get; set; }
        public string HouseBLno { get; set; }
        public int ContainerType { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public int TypeofPackage { get; set; }
        public string NoOfPackage { get; set; }
        public string TotalBLPackages { get; set; }
        public int FreightIndicator { get; set; }
        public int BLNature { get; set; }
        public int BLTypeId { get; set; }
        public int BLStateId { get; set; }
        public string ShippingMark { get; set; }
        public int Shipper { get; set; }
        //  public int Customer { get; set; }
        public int NotifyParty { get; set; }
        public double Weight { get; set; }
        public double Measurement { get; set; }
        public AutoComplete ProjectId { get; set; }
        public AutoComplete PortOfLoading { get; set; }
        public AutoComplete PortOfUnLoading { get; set; }
        public AutoComplete PortOfOrigin { get; set; }
        public AutoComplete OriginalLoadingPort { get; set; }
        public AutoComplete PortOfDelivery { get; set; }
        public AutoComplete UltimateDestination { get; set; }
        public string Description { get; set; }
        public int TypeOfCommodity { get; set; }
        public int BLStatusId { get; set; }
        public List<ContainerModel> ContainerItems { get; set; }
        public string ContainerInfo { get; set; }
        public AutoComplete ShipperNew { get; set; }
        public AutoComplete CustomerNew { get; set; }
        public AutoComplete NotifyPartyNew { get; set; }
        // public AutoComplete DOAgent { get; set; }
        public AutoComplete ClearanceBy { get; set; }

        public HouseBLModel()
        {
            ContainerItems = new List<ContainerModel>();
            ContainerType ContainerType = new ContainerType();
            AutoComplete ShipperNew = new AutoComplete();
            AutoComplete CustomerNew = new AutoComplete();
            AutoComplete NotifyPartyNew = new AutoComplete();
            AutoComplete DOAgent = new AutoComplete();
            AutoComplete PortOfLoading = new AutoComplete();
            AutoComplete PortOfUnLoading = new AutoComplete();
            AutoComplete PortOfOrigin = new AutoComplete();
            AutoComplete OriginalLoadingPort = new AutoComplete();
            AutoComplete PortOfDelivery = new AutoComplete();
            AutoComplete UltimateDestination = new AutoComplete();
            AutoComplete ClearanceBy = new AutoComplete();
            TypeofPackage TypeofPackage = new TypeofPackage();
            FreightIndicator FreightIndicator = new FreightIndicator();
            Party Shipper = new Party();
            Party Customer = new Party();
            Party NotifyParty = new Party();
            //  Port PortOfLoading = new Port();
            //  Port PortOfUnLoading = new Port();
            // Port PortOfOrigin = new Port();
            // Port OriginalLoadingPort = new Port();
            //  Port PortOfDelivery = new Port();
            // Port UltimateDestination = new Port();
        }

        public static List<Dictionary<object, object>> GetHouseBLItems(int SPId, int HBLId = 0)
        {
            List<Dictionary<object, object>> HBLList = new List<Dictionary<object, object>>();

            string sql = @"SELECT  HBL.[PortOfLoading]
                                  ,HBL.[PortOfUnloading]
                                  ,HBL.[PortOfOrigin]
                                  ,HBL.[OriginalLoadingPort]
	                              ,HBL.[PortOfDelivery]
                                  ,HBL.[UltimateDestination]
                                  ,SP.MasterBL
                                  ,HBL.[HouseBL]
								  ,ISNULL(SP.MasterBL,'-') AS MBL
								  ,ISNULL(HBL.[HouseBL],'-') AS HBL
								  --,CONCAT(ShipP.Name,', ',ConsigneeP.Name,', ',NotifyP.Name) AS Parties
								 -- ,CONCAT('<strong>Shipper:</strong><br>'+ShipP.Name,', ','<br><strong>Consignee:</strong><br>'+ConsigneeP.Name,', ','<br><strong>Notify:</strong><br>'+NotifyP.Name) AS Parties
                      --  ,CONCAT('<strong>Shipper</strong><br>',ShipP.Name,' ,(',ShipP.RegNo,'), ',SAdd.Address1,'/ ',SAdd.Atoll,',',SAdd.Island,', ',SNat.Name,
	                   --     '<br><strong>Consignee</strong><br>',ConsigneeP.Name,' ,(',ConsigneeP.RegNo,'), ',CAdd.Address1,'/ ',CAdd.Atoll,', ',CAdd.Island,',',CNat.Name,
	                   --     '<br><strong>Notify</strong><br>',NotifyP.Name,' ,(',NotifyP.RegNo,'), ',NAdd.Address1,'/ ',NAdd.Atoll,',',NAdd.Island,', ',NNat.Name) AS Parties
,CONCAT('<strong>Shipper</strong><br>',ShipP.Name,' ,(',ShipP.RegNo,'), ',SAdd.Atoll,',',SNat.Name,
	  '<br><strong>Consignee</strong><br>',ConsigneeP.Name,' ,(',ConsigneeP.RegNo,'), ',CAdd.Address1,'/ ',CAdd.Atoll,', ',CAdd.Island,',',CNat.Name,
	  '<br><strong>Notify</strong><br>',NotifyP.Name,' ,(',NotifyP.RegNo,'), ',NAdd.Atoll,',',NAdd.Island,', ',NNat.Name) AS Parties
								  ,ShipP.Name AS ShipperName
								  ,ConsigneeP.Name AS CustomerName
								  ,NotifyP.Name AS NotifyName
                                 -- ,DOAgentP.Name AS DOAgentName
								  --,CONCAT('ConainerNo:',HBL.ContainerNo,', ContainerType:',CT.Name,', SealNo:',HBL.SealNo) AS ContainerInfo
                                  ,HBL.Id AS HouseBLId
								--  ,CONCAT('<strong>No.of Pkg:</strong>',HBL.NoOfPackage,', <br><strong>PkgType:</strong>') AS PackageInfo
                                  ,HBL.[Description]
								  ,CONCAT('<strong>BL Nature:</strong>',BLN.Name) AS BLnature
								  ,CONCAT('<strong>Mark:</strong>',HBL.ShippingMark) AS Mark
								  ,FI.Name AS FreightIndicatorName
							      ,CONCAT('<strong>Freight Indicator:</strong>', FI.Name,'   |','<strong>Total:<strong>') AS FreightIndicatorNameTotal
                                  ,HBL.[Weight]
                                  ,HBL.[Measurement]
						          ,HBL.ProjectId
                                  ,HBL.[ShipperId]
                                  ,HBL.[CustomerId]
                                  ,HBL.[NotifyPartyId]
                                --  ,HBL.[DOAgentId]
	                             -- ,HBL.[ContainerTypeId]
                                  --,HBL.[ContainerNo]
                                  --,HBL.[SealNo]
                               --   ,HBL.[TypeofPackageId]
                                  ,HBL.[NoOfPackage]
                                  ,HBL.[FreightIndicatorId]
								  ,BLLS.BLStatusId
                                  ,BLLS.ClearanceBy
								  ,HBL.BLNatureId
								  ,HBL.BLTypesId
,HBL.BLStateId
								  ,HBL.ShippingMark
                                  ,Pr.Name AS ProjectName
								  ,BLS.Name AS BLStatusName
                              FROM [HouseBL] AS HBL
							  INNER JOIN BLLastStatus AS BLLS ON BLLS.BLId = HBL.Id
                              LEFT JOIN BLStatus AS BLS ON BLS.Id = BLLS.BLStatusId
							  INNER JOIN Shipment AS SP ON SP.Id = BLLS.ShipmentId
							  LEFT JOIN Party AS ShipP ON ShipP.Id = HBL.ShipperId
									LEFT JOIN [Address] AS SAdd ON SAdd.Id = ShipP.AddressId
									LEFT JOIN Nationality AS SNat ON SNat.Id = SAdd.NationalityId
									LEFT JOIN Contact AS SCon ON SCon.Id = ShipP.ContactId
							  LEFT JOIN Party AS ConsigneeP ON ConsigneeP.Id = HBL.CustomerId
									LEFT JOIN [Address] AS CAdd ON CAdd.Id = ConsigneeP.AddressId
									LEFT JOIN Nationality AS CNat ON CNat.Id = CAdd.NationalityId
									LEFT JOIN Contact AS CCon ON CCon.Id = ConsigneeP.ContactId
							  LEFT JOIN Party AS NotifyP ON NotifyP.Id = HBL.NotifyPartyId
									LEFT JOIN [Address] AS NAdd ON NAdd.Id = NotifyP.AddressId
									LEFT JOIN Nationality AS NNat ON NNat.Id = NAdd.NationalityId
									LEFT JOIN Contact AS NCon ON NCon.Id = NotifyP.ContactId
							 -- LEFT JOIN Party AS DOAgentP ON DOAgentP.Id = HBL.DOAgentId
							  LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
							  LEFT JOIN BLNature AS BLN ON BLN.Id = HBL.BLNatureId
                              LEFT JOIN Project AS Pr ON Pr.Id = HBL.ProjectId
							  --INNER JOIN ContainerType AS CT ON CT.Id = HBL.ContainerTypeId
							 -- INNER JOIN TypeofPackage AS TP ON TP.Id = HBL.TypeofPackageId
							  WHERE HBL.DateRemoved IS NULL AND HBL.ShipmentId = @SPId --AND HBL.Id = @HBLId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SPId", DbType.Int32);
            param.Value = SPId;
            spList.Add(param);

            if (HBLId > 0)
            {
                sql = sql.Replace("--AND HBL.Id = @HBLId", "AND HBL.Id = @HBLId");
                param = new SqlParameter("@HBLId", DbType.Int32);
                param.Value = HBLId;
                spList.Add(param);
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> HBL = new Dictionary<object, object>();
                string PortOfLoading = "";
                string PortOfUnloading = "";
                string PortOfOrigin = "";
                string OriginalLoadingPort = "";
                string UltimateDestination = "";
                string PortOfDelivery = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("PortOfLoading"))
                    {
                        PortOfLoading = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("PortOfLoadingName", PortOfLoading);
                        // HBL.Add("PortOfLoadingName", reader.GetValue(i) == DBNull.Value ? "" :  Port.GetPortName(Convert.ToInt32(reader.GetValue(i))));
                    }
                    else if (reader.GetName(i).Equals("PortOfUnloading"))
                    {
                        PortOfUnloading = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("PortOfUnloadingName", PortOfUnloading);
                    }
                    else if (reader.GetName(i).Equals("PortOfOrigin"))
                    {
                        PortOfOrigin = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("PortOfOriginName", PortOfOrigin);
                    }
                    else if (reader.GetName(i).Equals("OriginalLoadingPort"))
                    {
                        OriginalLoadingPort = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("OriginalLoadingPortName", OriginalLoadingPort);
                    }
                    else if (reader.GetName(i).Equals("UltimateDestination"))
                    {
                        UltimateDestination = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("UltimateDestinationName", UltimateDestination);
                    }
                    else if (reader.GetName(i).Equals("PortOfDelivery"))
                    {
                        PortOfDelivery = reader.GetValue(i) == DBNull.Value ? "" : Port.GetPortName(Convert.ToInt32(reader.GetValue(i)));
                        HBL.Add("PortOfDeliveryName", PortOfDelivery);
                    }
                    else if (reader.GetName(i).Equals("ClearanceBy"))
                    {
                        HBL.Add("ClearanceByName", reader.GetValue(i) == DBNull.Value ? "" : Port.GetPartyName(Convert.ToInt32(reader.GetValue(i))));
                    }

                    if (reader.GetName(i).Equals("HouseBLId"))
                    {
                        string ContainerNo = "";
                        string Type = "";
                        string Size = "";
                        string Pack = "";
                        string Indicator = "";
                        string SealNo = "";
                        string TypeOfPackageList = "";
                        List<Dictionary<object, object>> Containerist = new List<Dictionary<object, object>>();

                        Containerist = GetContainerItems(Convert.ToInt32(reader.GetValue(i)));
                        foreach (Dictionary<object, object> container in Containerist)
                        {
                            foreach (string key in container.Keys)
                            {
                                switch (key)
                                {
                                    case "ContainerNo":
                                        ContainerNo += container[key].ToString() + ",";
                                        break;
                                    case "ContainerTypeName":
                                        Type += container[key].ToString() + ",";
                                        break;
                                    case "Size":
                                        Size += container[key].ToString() + ",";
                                        break;
                                    case "CNoOfPackage":
                                        Pack += container[key].ToString() + ",";
                                        break;
                                    case "IndicatorName":
                                        Indicator += container[key].ToString() + ",";
                                        break;
                                    case "SealNo":
                                        SealNo += container[key].ToString() + ",";
                                        break;
                                    case "TypeOfPackage":
                                        TypeOfPackageList += container[key].ToString() + ",";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        HBL.Add("ContainerInfo", "<center><strong>Container No </strong><br>" + ContainerNo + "<br>" + "<strong>Type: </strong>" + Type + "<br>" + "<strong>Size: </strong>" + Size + "<br>" + "<strong>Pack: </strong>" + Pack + "<br>" + "<strong>Seal No </strong><br>" + SealNo + "</center>");
                        HBL.Add("TypeOfPackageList", "<center><strong>No.of Pkg: </strong>" + Convert.ToString(reader["NoOfPackage"]) + "<br><strong>Pkg Types</strong><br>" + TypeOfPackageList + "</center>");
                        HBL.Add("ContainerNoList", ContainerNo);
                        HBL.Add("WitnessName", "");
                        HBL.Add("WitnessSign", "");
                        HBL.Add("WitnessDate", "");
                    }

                    if (reader.GetName(i).Equals("MBL"))
                    {
                        HBL.Add("MaterBLandPorts", reader.GetValue(i) +
                                                   "<p style='background-color: #ccc9c8; margin:0;padding:0;'>Port of Loading</p>" + "<p>" + PortOfLoading + "</p>" +
                                                   "<p style='background-color: #ccc9c8; margin:0;padding:0;'>Port of Origin</p>" + "<p>" + PortOfOrigin + "</p>" +
                                                   "<font style='background-color: #ccc9c8;'>Port of Delivery</font>" //value add in last row
                                                   );
                    }
                    if (reader.GetName(i).Equals("HBL"))
                    {
                        HBL.Add("HouseBLandPorts", reader.GetValue(i) +
                            /*STRONG"<p style='background-color: #ccc9c8; margin:0;padding:0;'><strong>Port of UnLoading</strong></p>" + "<p>" + PortOfUnloading + "</p>" +
                            "<p style='background-color: #ccc9c8; margin:0;padding:0;'><strong>Original Loading Port</strong></p>" + "<p>" + OriginalLoadingPort + "</p>" +
                            "<p style='background-color: #ccc9c8; margin:0;padding:0;'><strong>Ultimate Destination</strong></p>" //add in last row*/
                                                   "<p style='background-color: #ccc9c8; margin:0;padding:0;'>Port of UnLoading</p>" + "<p>" + PortOfUnloading + "</p>" +
                                                   "<p style='background-color: #ccc9c8; margin:0;padding:0;'>Original Loading Port</p>" + "<p>" + OriginalLoadingPort + "</p>" +
                                                   "<font style='background-color: #ccc9c8;'>Ultimate Destination</font>" //value add in last row
                                                   );
                    }

                    HBL.Add(reader.GetName(i), reader.GetValue(i));

                }
                HBLList.Add(HBL);
            }
            reader.Close();
            return HBLList;
        }

        public static List<Dictionary<object, object>> GetHouseBLItemsData(int SPId)
        {
            List<Dictionary<object, object>> BLList = new List<Dictionary<object, object>>();

            string sql = @"SELECT  HBL.Id 
                            ,ISNULL(HBL.[HouseBL],SP.MasterBL) AS HouseBL
		                    ,BLN.CustomId AS BLNature
		                    ,BLT.CODE AS BLType
		                    ,SP.MasterBL
		                    ,PTLoading.Code AS PortOfLoading
                            ,PTUnLoading.Code AS PortOfUnloading
                            ,PTOrigin.Code AS PortOfOrigin
                            ,PTOriginal.Code AS OriginalLoadingPort
	                        ,PTDelivery.Code AS PortOfDelivery
                            ,PTUltimate.Code AS UltimateDestination
		                    ,VS.Name AS Carrier
		                    ,VO.Name AS CarrierOperator
		                    ,ShipP.Name AS ShipperName
		                    ,CONCAT(ShippAdd.Address1,'/ ',ShippAdd.Island,',',ShippAdd.Atoll,', ',ShippNat.Name,', (',ShipP.RegNo,')') AS ShipperAddress
		                    ,NotifyP.Name AS NotifyPartyName
		                    ,CONCAT(NotifyAdd.Address1,'/ ',NotifyAdd.Island,',',NotifyAdd.Atoll,', ',NotifyNat.Name,', (',NotifyP.RegNo,')') AS NotifyPartyAddress
		                    ,ConsigneeP.Name AS ConsigneeName
		                    ,CONCAT(CAdd.Address1,'/ ',CAdd.Island,',',CAdd.Atoll,', ',CNat.Name,', (',ConsigneeP.RegNo,')') AS ConsigneeAddress
                        FROM [HouseBL] AS HBL
	                    LEFT JOIN BLNature AS BLN ON BLN.Id = HBL.BLNatureId
	                    LEFT JOIN BLTypes AS BLT ON BLT.Id = HBL.BLTypesId
	                    INNER JOIN Shipment AS SP ON SP.Id = HBL.ShipmentId
		                    LEFT JOIN Port AS PTLoading ON PTLoading.Id = HBL.PortOfLoading
		                    LEFT JOIN Port AS PTUnLoading ON PTUnLoading.Id = HBL.PortOfUnloading
		                    LEFT JOIN Port AS PTOrigin ON PTOrigin.Id = HBL.PortOfOrigin
		                    LEFT JOIN Port AS PTOriginal ON PTOriginal.Id = HBL.OriginalLoadingPort
		                    LEFT JOIN Port AS PTDelivery ON PTDelivery.Id = HBL.PortOfDelivery
		                    LEFT JOIN Port AS PTUltimate ON PTUltimate.Id = HBL.UltimateDestination
	                    INNER JOIN Vessel AS VS ON VS.Id = SP.VesselId
	                    LEFT JOIN Party AS VO ON VO.Id = VS.Company
	                    LEFT JOIN Party AS ShipP ON ShipP.Id = HBL.ShipperId
			                    LEFT JOIN [Address] AS ShippAdd ON ShippAdd.Id = ShipP.AddressId
			                    LEFT JOIN Nationality AS ShippNat ON ShippNat.Id = ShippAdd.NationalityId
	                    LEFT JOIN Party AS NotifyP ON NotifyP.Id = HBL.NotifyPartyId
			                    LEFT JOIN [Address] AS NotifyAdd ON NotifyAdd.Id = NotifyP.AddressId
			                    LEFT JOIN Nationality AS NotifyNat ON NotifyNat.Id = NotifyAdd.NationalityId
	                    LEFT JOIN Party AS ConsigneeP ON ConsigneeP.Id = HBL.CustomerId
			                    LEFT JOIN [Address] AS CAdd ON CAdd.Id = ConsigneeP.AddressId
			                    LEFT JOIN Nationality AS CNat ON CNat.Id = CAdd.NationalityId
	                    WHERE HBL.DateRemoved IS NULL AND HBL.ShipmentId = @SPId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SPId", DbType.Int32);
            param.Value = SPId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> BL = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    BL.Add(reader.GetName(i), reader.GetValue(i));
                }
                BLList.Add(BL);
            }
            reader.Close();
            return BLList;
        }

        public static bool DeleteHBLitem(int HBLId)
        {
            String sql = @"UPDATE HouseBL
                            SET DateRemoved = GETDATE()
		                       ,RemovedBy = @EmployeeId
                          WHERE Id = @HBLId";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static bool DeleteContainerItem(int ContainerId)
        {
            String sql = @"UPDATE ContainerInfo
                            SET RemovedDate = GETDATE()
		                       ,RemovedBy = @EmployeeId
                            WHERE Id = @ContainerId";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ContainerId", DbType.Int32);
            param.Value = ContainerId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static List<Dictionary<object, object>> GetContainerItems(int HBLId)
        {
            List<Dictionary<object, object>> ContainerList = new List<Dictionary<object, object>>();

            string sql = @"SELECT CI.Id,
                                CI.PackingId,
                                CI.ContainerNo,
		                        CI.ContainerTypeId,
								CI.TypeofPackageId,
		                        CI.Size,
		                        CI.CNoOfPackage,
		                        CI.ContainerIndicatorId,
		                        CI.SealNo,
		                        CT.Name AS ContainerTypeName,
		                        Indi.Name AS IndicatorName,
								TP.Name AS TypeOfPackage
                        FROM ContainerInfo AS CI
                        LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                        LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                        LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                        WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Container = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
                ContainerList.Add(Container);
            }
            reader.Close();
            return ContainerList;
        }

        public static List<Dictionary<object, object>> GetContainerData(int HBLId)
        {
            List<Dictionary<object, object>> ContainerList = new List<Dictionary<object, object>>();

            string sql = @"SELECT 
                                CI.ContainerNo,
		                        CI.CNoOfPackage,
								CONCAT(CI.Size,CT.Name) AS ContainerType,
								Indi.Name AS IndicatorName,
			                    CI.SealNo
                        FROM ContainerInfo AS CI
                        LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                        LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                        LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                        WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Container = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
                ContainerList.Add(Container);
            }
            reader.Close();
            return ContainerList;
        }

        public static List<Dictionary<object, object>> GetContainerData3(int HBLId)
        {
            List<Dictionary<object, object>> ContainerList = new List<Dictionary<object, object>>();

            string sql = @"SELECT 
                                --CONCAT(CI.ContainerNo,' | ', CI.SealNo) AS ContainerNo,
								--CONCAT(CI.CNoOfPackage,' | ',CI.Size,CT.Name) AS ContainerType,
								CI.ContainerNo,
								CI.SealNo,
								Indi.[Description] AS IndicatorName,
								CI.Size AS ContainerSize,
								CT.Name AS ContainerType,
								CI.CNoOfPackage,
								TP.CODE AS TypeofPackage
                        FROM ContainerInfo AS CI
                        LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                        LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                        LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                        WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Container = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
                ContainerList.Add(Container);
            }
            reader.Close();
            return ContainerList;
        }

        public static Dictionary<object, object> GetContainerData4(int HBLId)
        {
            Dictionary<object, object> Container = new Dictionary<object, object>();
            string sql = @"SELECT 
                                --CONCAT(CI.ContainerNo,' | ', CI.SealNo) AS ContainerNo,
								--CONCAT(CI.CNoOfPackage,' | ',CI.Size,CT.Name) AS ContainerType,
								CI.ContainerNo,
								CI.SealNo,
								Indi.[Description] AS IndicatorName,
								CI.Size AS ContainerSize,
								CT.Name AS ContainerType,
								CI.CNoOfPackage,
								TP.CODE AS TypeofPackage
                        FROM ContainerInfo AS CI
                        LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                        LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                        LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                        WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return Container;
        }

        public static string GetContainerData5(int HBLId)
        {
            string sql = @"SELECT 
                                CI.ContainerNo,--
								CI.SealNo,
								CONCAT(CI.Size,' ',CT.Name) AS ContainerSizeType,
								--CONCAT(Indi.Name,'-',indi.Description) AS ContainerIndicator,
                                indi.Description AS ContainerIndicator,
		                        CONCAT(CI.CNoOfPackage,' ',TP.CODE) AS Package
                        FROM ContainerInfo AS CI
                        LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                        LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                        LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                        WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            string containerInfos = "";
            while (reader.Read())
            {
                string containerInfo;
                containerInfo = reader["ContainerNo"].ToString() + "/" + reader["SealNo"].ToString() + "/" + reader["ContainerSizeType"].ToString() + "/" + reader["ContainerIndicator"].ToString() + "/" + reader["Package"].ToString() +"<br/>";
                containerInfos = containerInfos + containerInfo;
            }
            reader.Close();
            return containerInfos;
        }

    }

    public class Party
    {
        public int PartyTypeId { get; set; }
        public string PartyTypeName { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string RegNo { get; set; }
        public string TinNo { get; set; }
        public string CNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Atoll { get; set; }
        public string Island { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string MobileNo { get; set; }
        public int NationalityId { get; set; }
        public int AddressId { get; set; }
        public int ContactId { get; set; }
        public string MBuser { get; set; }
        public string MBpassword { get; set; }
        public string ACuser { get; set; }
        public string ACpassword { get; set; }
        public string CustomUser { get; set; }
        public string CustomPwd { get; set; }

        public Party()
        {

        }
        public static Dictionary<String, String> GetParty()
        {
            String sql = @"SELECT Id,Name FROM Party;";
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
        public static IQueryable<Party> GetPartyIQ()
        {
            string sql = "SELECT Id,Name FROM Party;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Party> list = new List<Party>();
            while (reader.Read())
            {
                Party Party = new Party { PartyId = (int)reader[0], PartyName = (string)reader[1] };
                list.Add(Party);
            }
            reader.Close();
            return list.AsQueryable<Party>();
        }

        public static IQueryable<Party> GetPartyType()
        {
            string sql = "SELECT Id,Name FROM PartyType;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Party> list = new List<Party>();
            while (reader.Read())
            {
                Party Party = new Party { PartyTypeId = (int)reader[0], PartyTypeName = (string)reader[1] };
                list.Add(Party);
            }
            reader.Close();
            return list.AsQueryable<Party>();
        }

        public bool SaveParty()
        {
            string sql = @"DECLARE @AddressLastId INT
                           DECLARE @ContactLastId INT

                         INSERT INTO [dbo].[Address]
                                           ([Address1]
                                          -- ,[Address2]
                                           ,[Island]
                                           ,[Atoll]
                                           ,[NationalityId])
                                     VALUES
                                           (@Address1
                                         --  ,@Address2
                                           ,@Island
                                           ,@Atoll
                                           ,@NationalityId)
                                            SELECT @AddressLastId = SCOPE_IDENTITY();


                         INSERT INTO [dbo].[Contact]
                                           ([Name]
                                           ,[Tel]
                                           ,[Fax]
                                           ,[Mobile]
                                           ,[Email])
                                     VALUES
                                           (@ContactName
                                           ,@TelNo
                                           ,@FaxNo
                                           ,@MobileNo
                                           ,@Email)
                                            SELECT @ContactLastId = SCOPE_IDENTITY();

                         INSERT INTO [dbo].[Party]
                                           ([PartyTypeId]
                                           ,[Name]
                                           ,[RegNo]
                                           ,[TIN]
                                           ,[CNumber]
                                           ,[AddressId]
                                           ,[ContactId]
                                           ,[MyBandharuUser]
                                           ,[MyBandharuPwd]
                                           ,[CustomUser]
                                           ,[CustomPwd]
                                           ,[AsycudaUser]
                                           ,[AsycudaPwd]
)
                                     VALUES
                                           (@PartyTypeId
                                           ,@PartyName
                                           ,@RegNo
                                           ,@TinNo
                                           ,@CNumber
                                           ,@AddressLastId
                                           ,@ContactLastId
                                           ,@MyBandharuUser
                                           ,@MyBandharuPwd
                                           ,@CustomUser
                                           ,@CustomPwd
                                           ,@ACUser
                                           ,@ACPwd)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Address1", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Address1) ? (object)DBNull.Value : Address1;
            list.Add(param);
            /*param = new SqlParameter("@Address2", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Address2) ? (object)DBNull.Value : Address2;
            list.Add(param);*/
            param = new SqlParameter("@Island", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Island) ? (object)DBNull.Value : Island;
            list.Add(param);
            param = new SqlParameter("@Atoll", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Atoll) ? (object)DBNull.Value : Atoll;
            list.Add(param);
            param = new SqlParameter("@NationalityId", DbType.Int32);
            param.Value = NationalityId;
            list.Add(param);
            param = new SqlParameter("@ContactName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ContactName) ? (object)DBNull.Value : ContactName;
            list.Add(param);
            param = new SqlParameter("@TelNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(TelNo) ? (object)DBNull.Value : TelNo;
            list.Add(param);
            param = new SqlParameter("@FaxNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(FaxNo) ? (object)DBNull.Value : FaxNo;
            list.Add(param);
            param = new SqlParameter("@MobileNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(MobileNo) ? (object)DBNull.Value : MobileNo;
            list.Add(param);
            param = new SqlParameter("@Email", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Email) ? (object)DBNull.Value : Email;
            list.Add(param);
            param = new SqlParameter("@PartyTypeId", DbType.Int32);
            param.Value = PartyTypeId;
            list.Add(param);
            param = new SqlParameter("@PartyName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(PartyName) ? (object)DBNull.Value : PartyName;
            list.Add(param);
            param = new SqlParameter("@RegNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(RegNo) ? (object)DBNull.Value : RegNo;
            list.Add(param);
            param = new SqlParameter("@TinNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(TinNo) ? (object)DBNull.Value : TinNo;
            list.Add(param);
            param = new SqlParameter("@CNumber", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(CNumber) ? (object)DBNull.Value : CNumber;
            list.Add(param);

            param = new SqlParameter("@MyBandharuUser", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(MBuser) ? (object)DBNull.Value : MBuser;
            list.Add(param);
            param = new SqlParameter("@MyBandharuPwd", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(MBpassword) ? (object)DBNull.Value : MBpassword;
            list.Add(param);

            param = new SqlParameter("@CustomUser", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(CustomUser) ? (object)DBNull.Value : CustomUser;
            list.Add(param);
            param = new SqlParameter("@CustomPWD", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(CustomPwd) ? (object)DBNull.Value : CustomPwd;
            list.Add(param);

            param = new SqlParameter("@ACUser", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ACuser) ? (object)DBNull.Value : ACuser;
            list.Add(param);
            param = new SqlParameter("@ACPwd", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ACpassword) ? (object)DBNull.Value : ACpassword;
            list.Add(param);

            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public Dictionary<object, object> GetSelectedParty(int PartyId)
        {
            string sql = @"SELECT 
                                P.PartyTypeId,
                                P.Name AS PartyName,
                                P.RegNo,
                                P.TIN AS TinNo,
								P.CNumber,
                                A.NationalityId,
                                A.Atoll,
                                A.Island,
                                A.Address1,
                                A.Address2,
                                C.Tel AS TelNo,
                                C.Fax AS FaxNo,
                                C.Email,
                                C.Name AS ContactName,
                                C.Mobile AS MobileNo,
								A.Id AS AddressId,
								C.Id AS ContactId,
								P.MyBandharuUser,
								P.MyBandharuPwd,
								P.CustomUser,
								P.CustomPwd,
								P.AsycudaUser,
								P.AsycudaPwd
                                FROM Party AS P 
                                INNER JOIN [Address] AS A ON A.Id = P.AddressId
                                INNER JOIN Contact AS C ON C.Id = P.ContactId
                                WHERE P.Id=@PartyId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@PartyId", DbType.Int32);
            param.Value = PartyId;
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

        public bool UpdateParty()
        {
            string sql = @" UPDATE [dbo].[Party]
                               SET [PartyTypeId] = @PartyTypeId
                                  ,[Name] = @PartyName
                                  ,[RegNo] = @RegNo
                                  ,[TIN] = @TinNo
                                  ,[CNumber] = @CNumber
                                  ,[MyBandharuUser] = @MBuser
                                  ,[MyBandharuPwd] = @MBpassword
                                  ,[CustomUser] = @CustomUser
                                  ,[CustomPwd] = @CustomPwd
                                  ,[AsycudaUser] = @ACUser
                                  ,[AsycudaPwd] = @ACPwd
                            WHERE Id = @PartyId

                            UPDATE [dbo].[Address]
                               SET [Address1] = @Address1
                                 -- ,[Address2] = @Address2
                                  ,[Island] = @Island
                                  ,[Atoll] = @Atoll
                                  ,[NationalityId] = @NationalityId
                             WHERE Id = @AddressId

                            UPDATE [dbo].[Contact]
                               SET [Name] = @ContactName
                                  ,[Tel] = @Tel
                                  ,[Fax] = @Fax
                                  ,[Mobile] = @Mobile
                                  ,[Email] = @Email
                             WHERE Id = @ContactId";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@PartyTypeId", DbType.Int32);
            param.Value = PartyTypeId;
            list.Add(param);
            param = new SqlParameter("@PartyName", DbType.String);
            param.Value = PartyName;
            list.Add(param);
            param = new SqlParameter("@RegNo", DbType.String);
            param.Value = RegNo;
            list.Add(param);
            param = new SqlParameter("@TinNo", DbType.String);
            param.Value = TinNo;
            list.Add(param);
            param = new SqlParameter("@CNumber", DbType.String);
            param.Value = CNumber;
            list.Add(param);
            param = new SqlParameter("@PartyId", DbType.Int32);
            param.Value = PartyId;
            list.Add(param);
            param = new SqlParameter("@MBuser", DbType.String);
            param.Value = MBuser;
            list.Add(param);
            param = new SqlParameter("@MBpassword", DbType.String);
            param.Value = MBpassword;
            list.Add(param);
            param = new SqlParameter("@CustomUser", DbType.String);
            param.Value = CustomUser;
            list.Add(param);
            param = new SqlParameter("@CustomPwd", DbType.String);
            param.Value = CustomPwd;
            list.Add(param);

            param = new SqlParameter("@ACUser", DbType.String);
            param.Value = ACuser;
            list.Add(param);
            param = new SqlParameter("@ACPwd", DbType.String);
            param.Value = ACpassword;
            list.Add(param);

            param = new SqlParameter("@Address1", DbType.String);
            param.Value = Address1;
            list.Add(param);
            /*param = new SqlParameter("@Address2", DbType.String);
            param.Value = Address2;
            list.Add(param);*/
            param = new SqlParameter("@Island", DbType.String);
            param.Value = Island;
            list.Add(param);
            param = new SqlParameter("@Atoll", DbType.String);
            param.Value = Atoll;
            list.Add(param);
            param = new SqlParameter("@NationalityId", DbType.Int32);
            param.Value = NationalityId;
            list.Add(param);
            param = new SqlParameter("@AddressId", DbType.Int32);
            param.Value = AddressId;
            list.Add(param);

            param = new SqlParameter("@ContactName", DbType.String);
            param.Value = ContactName;
            list.Add(param);
            param = new SqlParameter("@Tel", DbType.String);
            param.Value = TelNo;
            list.Add(param);
            param = new SqlParameter("@Fax", DbType.String);
            param.Value = FaxNo;
            list.Add(param);
            param = new SqlParameter("@Mobile", DbType.String);
            param.Value = MobileNo;
            list.Add(param);
            param = new SqlParameter("@Email", DbType.String);
            param.Value = Email;
            list.Add(param);
            param = new SqlParameter("@ContactId", DbType.Int32);
            param.Value = ContactId;
            list.Add(param);

            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null)
        {
            string sql = @"SELECT Id,Name,RegNo FROM 
                                    PARTY
                                    WHERE 
                                    (
                                    RegNo like concat('%',@query,'%')
                                    OR
                                    Name like concat('%',@query,'%')
                                    ) ";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> partylist = new List<Dictionary<object, object>>();

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
                partylist.Add(dictionary);
            }
            reader.Close();
            return partylist;
        }

        public static Dictionary<object, object> GetContactList()
        {

            List<Dictionary<object, object>> ContactList = new List<Dictionary<object, object>>();

            string sql = @"SELECT P.Id,
	                           PT.[Name] AS PartyType,
	                           P.[Name],
	                           P.RegNo,
	                           CONCAT(C.Name,',',C.Mobile,'/',C.Tel,',',C.Email) AS Contact,
	                           AD.Address1,
	                          CONCAT(P.MyBandharuUser,' / ', P.MyBandharuPwd) AS MyBandharu,
	                           CONCAT(P.CustomUser,' / ', P.CustomPwd) AS [Custom],
	                           CONCAT(P.AsycudaUser,' / ', P.AsycudaPwd) AS Asycuda
                        FROM Party AS P
                        INNER JOIN PartyType AS PT ON PT.Id = P.PartyTypeId
                        LEFT JOIN [Address] AS AD ON AD.Id = P.AddressId
                        LEFT JOIN Contact AS C ON C.Id = P.ContactId";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Contact = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Contact.Add(reader.GetName(i), reader.GetValue(i));
                }
                ContactList.Add(Contact);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ContactList);
            return dataTable;
        }
    }

    public class Vessel
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string Name { get; set; }
        public int Company { get; set; }
        public int VesselRegCountry { get; set; }

        public Vessel() { }

        public static Dictionary<String, String> GetVessel()
        {
            String sql = @"SELECT Id,Name FROM Vessel;";
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
                                    Vessel
                                    WHERE 
                                    (
                                   -- RegNo like concat('%',@query,'%')
                                   -- OR
                                    Name like concat('%',@query,'%')
                                    ) ";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> Vessellist = new List<Dictionary<object, object>>();

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
                Vessellist.Add(dictionary);
            }
            reader.Close();
            return Vessellist;
        }

        public bool SaveVessel()
        {
            string sql = @"INSERT INTO [dbo].[Vessel]
                                   ([Name]
                                   ,[Company]
                                   ,[RegCountry])
                             VALUES
                                   (@Name
                                   ,@CompanyId
                                   ,@RegCountry)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Name) ? (object)DBNull.Value : Name;
            list.Add(param);
            param = new SqlParameter("@CompanyId", DbType.Int32);
            param.Value = Company;
            list.Add(param);
            param = new SqlParameter("@RegCountry", DbType.Int32);
            param.Value = VesselRegCountry;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public Dictionary<object, object> GetSelectedVessel(int VesselId)
        {
            string sql = @"SELECT V.Name, V.Company AS CompanyId, P.Name AS CompanyName, RegCountry AS VesselRegCountry
                            FROM Vessel AS V
                            LEFT JOIN Party AS P ON P.Id = V.Company
                            WHERE V.Id =@VesselId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = VesselId;
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

        public bool UpdateVessel()
        {
            string sql = @"UPDATE [dbo].[Vessel]
                           SET [Name] = @Name
                              ,[Company] = @Company
                              ,[RegCountry] = @VesselRegCountry
                           WHERE Id = @VesselId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@VesselId", DbType.Int32);
            param.Value = Id;
            list.Add(param);
            param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Name) ? (object)DBNull.Value : Name;
            list.Add(param);
            param = new SqlParameter("@Company", DbType.Int32);
            param.Value = Company;
            list.Add(param);
            param = new SqlParameter("@VesselRegCountry", DbType.Int32);
            param.Value = VesselRegCountry;
            list.Add(param);
            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }
    }

    public class Nationality
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetNationality()
        {
            String sql = @"SELECT Id,Name FROM Nationality;";
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

        public static IQueryable<Nationality> GetCountries()
        {
            string sql = "SELECT Id,Name FROM Nationality;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Nationality> list = new List<Nationality>();
            while (reader.Read())
            {
                Nationality Country = new Nationality { Id = (int)reader[0], Name = (string)reader[1] };
                list.Add(Country);
            }
            reader.Close();
            return list.AsQueryable<Nationality>();
        }


    }

    public class ModeofShipment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetModeofShipment()
        {
            String sql = @"SELECT Id,Name FROM ModeofShipment;";
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

    public class CustomOffice
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetCustomOffice()
        {
            String sql = @"SELECT Id,CONCAT(Code,' - ',Office) AS Name FROM CustomOffice;";
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

    public class Port
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetPort()
        {
            /*  String sql = @"SELECT Id,Code FROM Port;";
              List<SqlParameter> list = new List<SqlParameter>();
              SqlDataReader reader = DBAccess.FetchResult(sql, list);
              Dictionary<String, String> dictionary = new Dictionary<String, String>();
              while (reader.Read())
              {
                  dictionary.Add(reader["Id"].ToString(), reader["Code"].ToString());
              }
              reader.Close();
              return dictionary;*/
            return null;
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null, int? mode = 0)
        {
            string sql = @"SELECT Id,Concat(Code,' ',Name) AS Code FROM 
                                    Port
                                    WHERE 
                                    (
                                    Code like concat('%',@query,'%') @Type
                                    OR
                                    Name like concat('%',@query,'%') @Type
                                    ) ";

            if (mode == 1)
            {
                sql = sql.Replace("@Type", "AND [Type] ='SEA'");
            }
            else if (mode == 2)
            {
               sql = sql.Replace("@Type", "AND Type ='AIR'");
            }
            else if (mode == 3)
            {
                sql = sql.Replace("@Type", "AND Type ='AIR'");
            }
            else
            {
               sql = sql.Replace("@Type", "");
            }

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> portlist = new List<Dictionary<object, object>>();

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
                dictionary.Add("Name", (reader["Code"].ToString()));
                portlist.Add(dictionary);
            }
            reader.Close();
            return portlist;
        }

        public static string GetPortName(int portId)
        {
            string sql = @"SELECT Code FROM  Port WHERE Id=@portId";

            string portName = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter("@portId", DbType.Int32) { Value = portId };
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            while (reader.Read())
            {
                portName = (string)reader[0];
            }
            reader.Close();
            return portName;
        }

        public static string GetPartyName(int ptId)
        {
            string sql = @"SELECT Name FROM  Party WHERE Id=@ptId";

            string portName = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter("@ptId", DbType.Int32) { Value = ptId };
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            while (reader.Read())
            {
                portName = (string)reader[0];
            }
            reader.Close();
            return portName;
        }
    }

    public class ContainerType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetContainerType()
        {
            String sql = @"SELECT Id,Name FROM ContainerType;";
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
        public static Dictionary<String, String> GetContainerIndicatorType()
        {
            String sql = @"SELECT Id,CONCAT(Name,' ',[Description]) AS Name FROM ContainerIndicator;";
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

    public class ContainerModel
    {
        public int PackingType { get; set; }
        public int containerId { get; set; }
        public string ContainerNo { get; set; }
        public int ContainerType { get; set; }
        public int PackageType { get; set; }
        public int CNoOfPackage { get; set; }
        public int ContainerSize { get; set; }
        public int Indicator { get; set; }
        public string SealNo { get; set; }

        public ContainerModel()
        {
        }


        public static Dictionary<String, String> GetContainerType()
        {
            String sql = @"SELECT Id,Name FROM ContainerType;";
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

        public static int CountShipmentContainer(int ShipmentId)
        {
            /*string sql = @"SELECT HBL.Id,
                            (SELECT COUNT(Id) FROM ContainerInfo where RemovedDate IS NULL AND HouseBLId = HBL.Id) AS ContainerCount
                            FROM HouseBL AS HBL 
                            WHERE HBL.DateRemoved IS NULL AND HBL.ShipmentId = @ShipmentId";*/
            string sql = @"select COUNT(DISTINCT sCI.ContainerNo) AS ContainerCount 
                            from HouseBL sHBL 
                            INNER JOIN ContainerInfo AS sCI ON sCI.HouseBLId = sHBL.Id  AND sCI.RemovedDate IS NULL AND sHBL.DateRemoved IS NULL where sHBL.ShipmentId = @ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Total = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("ContainerCount"))
                    {
                        // Total = Total + Convert.ToInt32(reader.GetValue(i));
                        Total = Convert.ToInt32(reader.GetValue(i));
                    }
                }
            }
            reader.Close();
            return Total;
        }

        public static int CountBLContainer(int HBLId)
        {
            string sql = @"SELECT COUNT(Id) AS Total from ContainerInfo
                                    WHERE HouseBLId = @HBLId AND RemovedDate IS NULL";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Total = 0;
            while (reader.Read())
            {
                Total = Convert.ToInt32(reader["Total"]);
            }
            reader.Close();
            return Total;
        }

        public static Dictionary<object, object> GetContainerData1(int HBLId)
        {
            Dictionary<object, object> Container = new Dictionary<object, object>();
            string sql = @"SELECT TOP 1  	TP.CODE AS PKTypeCode,
						            TP.Name AS PKType
                                    FROM ContainerInfo AS CI
                                    LEFT JOIN ContainerType AS CT ON CI.ContainerTypeId = CT.Id
                                    LEFT JOIN ContainerIndicator AS Indi ON Indi.Id = CI.ContainerIndicatorId
                                    LEFT JOIN TypeofPackage AS TP ON CI.TypeofPackageId = TP.Id
                                    WHERE HouseBLId =@HBLId AND CI.RemovedDate IS NULL";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return Container;
        }

        public static Dictionary<object, object> GetContainerData2(int HBLId)
        {
            Dictionary<object, object> Container = new Dictionary<object, object>();
            string sql = @"select HBL.[Weight],HBL.[Description],HBL.[Measurement],FI.CODE AS FICOde, HBL.ShippingMark
                            from HouseBL AS HBL 
                            LEFT JOIN FreightIndicator AS FI ON FI.Id = HBL.FreightIndicatorId
                            where HBL.Id = @HBLId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Container.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return Container;
        }
    }

    public class TypeofPackage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetTypeofPackage()
        {
            String sql = @"SELECT Id,Name FROM TypeofPackage;";
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

        public static int CountBLPackages(int HBLId)
        {
            string sql = @"SELECT  SUM(CNoOfPackage) TotalPackage
                            FROM ContainerInfo
                            WHERE HouseBLId =@HBLId AND RemovedDate IS NULL";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLId", DbType.Int32);
            param.Value = HBLId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Total = 0;
            while (reader.Read())
            {
                Total = Convert.ToInt32(reader["TotalPackage"]);
            }
            reader.Close();
            return Total;
        }
    }

    public class TypeofPacking
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetTypeofPacking()
        {
            String sql = @"SELECT Id,Name FROM Packing;";
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

    public class FreightIndicator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetFreightIndicator()
        {
            String sql = @"SELECT Id,Name FROM FreightIndicator;";
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

    public class BLNature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetBLNature()
        {
            String sql = @"SELECT Id,Name FROM BLNature;";
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

    public class BLType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Dictionary<String, String> GetBLTypes()
        {
            String sql = @"SELECT Id,CODE AS Name FROM BLTypes;";
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
        public static Dictionary<String, String> GetBLStates()
        {
            String sql = @"SELECT Id,Name FROM BLState;";
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

    public class BLStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static IQueryable<BLStatus> GetBLStatusList()
        {
            string sql = "SELECT Id,Name FROM BLStatus WHERE Id IN(1,2,3)";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<BLStatus> list = new List<BLStatus>();
            while (reader.Read())
            {
                BLStatus BLS = new BLStatus { Id = (int)reader[0], Name = (string)reader[1] };
                list.Add(BLS);
            }
            reader.Close();
            return list.AsQueryable<BLStatus>();
        }

        public static IQueryable<BLStatus> GetBLJobStatusList()
        {
            string sql = "SELECT Id,Name FROM BLStatus WHERE Id IN(2,4,5,6) ORDER BY CASE WHEN Id = 4 THEN 1 ELSE 2 END, Id";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<BLStatus> list = new List<BLStatus>();
            while (reader.Read())
            {
                BLStatus BLS = new BLStatus { Id = (int)reader[0], Name = (string)reader[1] };
                list.Add(BLS);
            }
            reader.Close();
            return list.AsQueryable<BLStatus>();
        }

        /* public static IQueryable<BLStatus> GetBLAllStatusList()
         {
             string sql = "SELECT Id,Name FROM BLStatus Order by Id";
             List<SqlParameter> spList = new List<SqlParameter>();
             SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
             List<BLStatus> list = new List<BLStatus>();
             while (reader.Read())
             {
                 BLStatus BLS = new BLStatus { Id = (int)reader[0], Name = (string)reader[1] };
                 list.Add(BLS);
             }
             reader.Close();
             return list.AsQueryable<BLStatus>();
         }*/

        public static Dictionary<String, String> GetBLAllStatusList()
        {
            String sql = @"SELECT Id,Name FROM BLStatus Order by Id";
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
        public static bool SaveBLStatus(int HBLid, int StatusId, string DOcollectedBy = null, string DOcollectedContact = null, DateTime? DOcollectedDate = null)
        {
            String sql = @"   IF NOT EXISTS
                                (
		                        SELECT HouseBLId 
		                        FROM BLStatusHistory
		                        WHERE HouseBLId = @HouseBLId AND BLStatusId = @BLStatusId
                                )
                                BEGIN
			                        INSERT INTO [dbo].[BLStatusHistory]
											                           ([HouseBLId]
											                           ,[BLStatusId]
											                           ,[DateCreated]
											                           ,[EmployeeId]
                                                                       ,[DOcollectedBy]
                                                                       ,[DOcollectedContact]
                                                                       ,[DOcollectedDate])
										                         VALUES
											                           (@HouseBLId
											                           ,@BLStatusId
											                           ,GETDATE()
											                           ,@EmployeeId
                                                                       ,@DOcollectedBy
                                                                       ,@DOcollectedContact
                                                                       ,@DOcollectedDate)
                                END";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HBLid;
            List.Add(param);
            param = new SqlParameter("@BLStatusId", DbType.Int32);
            param.Value = StatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@DOcollectedBy", DbType.String);
            if (DOcollectedBy == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = DOcollectedBy;
            }
            List.Add(param);
            param = new SqlParameter("@DOcollectedContact", DbType.String);
            if (DOcollectedContact == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = DOcollectedContact;
            }
            List.Add(param);
            param = new SqlParameter("@DOcollectedDate", DbType.String);
            if (DOcollectedDate == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = DOcollectedDate;
            }
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }
        public static bool UpdateBLStatus(int HBLid, int StatusId)
        {
            String sql = @"UPDATE BLStatusHistory
                            SET DOAttached = GETDATE()
                            WHERE HouseBLId = @HouseBLId AND BLStatusId = @BLStatusId";

            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HBLid;
            List.Add(param);
            param = new SqlParameter("@BLStatusId", DbType.Int32);
            param.Value = StatusId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }
    }

    public class ShipmentStatusVM
    {
        public int? ShipmentId { get; set; }
        public int? HouseBLId { get; set; }

        public ShipmentStatusVM()
        {
        }
        public ShipmentStatusVM(int? shipmentId, int? BOLid)
        {
            this.ShipmentId = shipmentId;
            this.HouseBLId = BOLid;

        }

        public static List<Dictionary<object, object>> GetShipmentStatusList(int? shipmentId)
        {
            string sql = @"SELECT Id, Name FROM ShipmentStatus WHERE Id in(1,2,3,4,6,7) order by Id";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> StatusList = new List<Dictionary<object, object>>();
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            Dictionary<object, object> existingstatus = new Dictionary<object, object>();
            Dictionary<object, object> existingReview = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("Id"))
                    {
                        if (Convert.ToInt32(reader.GetValue(i)) < 6)
                        {
                            existingstatus = GetShipmentExsistStatus(shipmentId, Convert.ToInt32(reader.GetValue(i)));
                            dictionary.Add("StatusName", (reader["Name"].ToString()));
                            if (existingstatus.Count > 0)
                            {
                                dictionary["DateCreated"] = existingstatus["DateCreated"].ToString();
                                dictionary["FirstName"] = existingstatus["FirstName"].ToString();
                            }
                            else
                            {
                                dictionary["DateCreated"] = "-";
                                dictionary["FirstName"] = "-";
                            }

                        }
                        else
                        {
                            existingReview = GetShipmentExsistReview(shipmentId, Convert.ToInt32(reader.GetValue(i)));
                            dictionary.Add("StatusName", (reader["Name"].ToString()));
                            if (existingReview.Count > 0)
                            {
                                dictionary["DateCreated"] = existingReview["DateCreated"].ToString();
                                dictionary["FirstName"] = existingReview["FirstName"].ToString();
                            }
                            else
                            {
                                dictionary["DateCreated"] = "-";
                                dictionary["FirstName"] = "-";
                            }

                        }
                        StatusList.Add(dictionary);
                    }
                }
            }
            reader.Close();
            return StatusList;

        }

        public static Dictionary<object, object> GetShipmentExsistStatus(int? ShipmentId, int SPStatus)
        {
            string sql = @"SELECT SPAS.DateCreated,
		                           -- EMP.FirstName 
                                    CONCAT(EMP.FirstName,' ',ISNULL(EMP.MiddleName,EMP.LastName)) AS FirstName
                            FROM ShipmentAllStatus AS SPAS
                            INNER JOIN Employee AS EMP ON EMP.Id = SPAS.EmployeeId
                            WHERE SPAS.ShipmentId = @ShipmentId AND SPAS.ShipmentStatusId = @SPStatus order by SPAS.ShipmentStatusId";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            spList.Add(param);
            param = new SqlParameter("@SPStatus", DbType.Int32);
            param.Value = SPStatus;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary.Add("DateCreated", (reader["DateCreated"].ToString()));
                dictionary.Add("FirstName", (reader["FirstName"].ToString()));
            }
            reader.Close();
            return dictionary;

        }
        public static Dictionary<object, object> GetShipmentExsistReview(int? ShipmentId, int SPStatus)
        {
            string sql = @"SELECT SPAR.DateCreated,
		                            --EMP.FirstName
                                     CONCAT(EMP.FirstName,' ',ISNULL(EMP.MiddleName,EMP.LastName)) AS FirstName
                            FROM ShipmentAllReview AS SPAR
                            INNER JOIN Employee AS EMP ON EMP.Id = SPAR.EmployeeId
                            WHERE SPAR.ShipmentId = @ShipmentId AND SPAR.ShipmentStatusId = @SPStatus order by SPAR.ShipmentStatusId";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            spList.Add(param);
            param = new SqlParameter("@SPStatus", DbType.Int32);
            param.Value = SPStatus;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary.Add("DateCreated", (reader["DateCreated"].ToString()));
                dictionary.Add("FirstName", (reader["FirstName"].ToString()));
            }
            reader.Close();
            return dictionary;

        }
        /*******BL and Job Status***************/
        public static List<Dictionary<object, object>> GetHouseBLStatusList(int? HouseBLId)
        {
            string sql = @"SELECT Id, Name FROM BLStatus WHERE Id in(3,4,5,6,7,8,9) order by
                                                Case WHEN Id= 3 then 1
	                                                WHEN Id = 4 then 2
	                                                WHEN Id = 5 then 3
	                                                WHEN Id = 8 then 4
	                                                WHEN Id = 9 then 5
	                                                WHEN Id = 6 then 6
	                                                WHEN Id = 7 then 7
	                                                END"; //*****Custom Ordering****/

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Dictionary<object, object>> StatusList = new List<Dictionary<object, object>>();
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            Dictionary<object, object> existingstatus = new Dictionary<object, object>();
            Dictionary<object, object> existingReview = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("Id"))
                    {
                        if (Convert.ToInt32(reader.GetValue(i)) < 9)
                        {
                            existingstatus = GetBLExsistStatus(HouseBLId, Convert.ToInt32(reader.GetValue(i)));
                            dictionary.Add("StatusName", (reader["Name"].ToString()));
                            if (existingstatus.Count > 0)
                            {
                                dictionary["DateCreated"] = existingstatus["DateCreated"].ToString();
                                dictionary["FirstName"] = existingstatus["FirstName"].ToString();
                            }
                            else
                            {
                                dictionary["DateCreated"] = "-";
                                dictionary["FirstName"] = "-";
                            }

                        }
                        else
                        {
                            existingReview = GetBLExsistReview(HouseBLId, Convert.ToInt32(reader.GetValue(i)));
                            dictionary.Add("StatusName", (reader["Name"].ToString()));
                            if (existingReview.Count > 0)
                            {
                                dictionary["DateCreated"] = existingReview["DateCreated"].ToString();
                                dictionary["FirstName"] = existingReview["FirstName"].ToString();
                            }
                            else
                            {
                                dictionary["DateCreated"] = "-";
                                dictionary["FirstName"] = "-";
                            }

                        }
                        StatusList.Add(dictionary);
                    }
                }
            }
            reader.Close();
            return StatusList;
        }
        public static Dictionary<object, object> GetBLExsistStatus(int? HouseBLId, int BLStatus)
        {
            string sql = @"SELECT BLAS.DateCreated,
		                           -- EMP.FirstName 
                                    CONCAT(EMP.FirstName,' ',ISNULL(EMP.MiddleName,EMP.LastName)) AS FirstName
                            FROM BLAllStatus AS BLAS
                            INNER JOIN Employee AS EMP ON EMP.Id = BLAS.EmployeeId
                            WHERE BLAS.BLId = @HouseBLId AND BLAS.BLStatusId = @BLStatus order by BLAS.BLStatusId";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HouseBLId;
            spList.Add(param);
            param = new SqlParameter("@BLStatus", DbType.Int32);
            param.Value = BLStatus;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary.Add("DateCreated", (reader["DateCreated"].ToString()));
                dictionary.Add("FirstName", (reader["FirstName"].ToString()));
            }
            reader.Close();
            return dictionary;

        }
        public static Dictionary<object, object> GetBLExsistReview(int? HouseBLId, int BLStatus)
        {
            string sql = @"SELECT BLAR.DateCreated,
		                            CONCAT(EMP.FirstName,' ',ISNULL(EMP.MiddleName,EMP.LastName)) AS FirstName
                            FROM BLAllReview AS BLAR
                            INNER JOIN Employee AS EMP ON EMP.Id = BLAR.EmployeeId
                            WHERE BLAR.BLId = @HouseBLId AND BLAR.BLStatusId = @BLStatus order by BLAR.BLStatusId";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HouseBLId;
            spList.Add(param);
            param = new SqlParameter("@BLStatus", DbType.Int32);
            param.Value = BLStatus;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary.Add("DateCreated", (reader["DateCreated"].ToString()));
                dictionary.Add("FirstName", (reader["FirstName"].ToString()));
            }
            reader.Close();
            return dictionary;

        }
    }
}