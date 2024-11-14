using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using AYLogistics.Models;
using System.Web.Mvc;
using System.Web.Security;
using MyDBAccess;
using System.Data;
using System.Web.Mvc.Html;

namespace AYLogistics
{
    public static class MySettings
    {
        public const String MaleAddressFormat = "%district%. %address%";


        public static String FormatDate(string Date)
        {
            DateTime CDate = Convert.ToDateTime(Date);
            string DateFormat = CDate.ToString("dd/MM/yyyy");
            return DateFormat;
        }

        public static string FormatPONumber(int POid)
        {
            string FormatPONumber = GetPONumberFormat();
            return FormatPONumber.Replace("%PON%", POid.ToString()).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
        }
                    public static string GetPONumberFormat()
                    {
                        string sql = @"SELECT Format FROM  AdminSetting WHERE Id=1";

                        string CaseNoFmt = null;
                        List<SqlParameter> list = new List<SqlParameter>();

                        SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

                        while (reader.Read())
                        {
                            CaseNoFmt = (string)reader[0];
                        }
                        reader.Close();
                        return CaseNoFmt;
                    }
        //Annually Reset Numbering and get format
        public static string FormatNumbering(int Id)
        {
            Dictionary<Object, Object> Format = GetNumberFormatSequence(Id);
            int LastUpdateYear = Format["LastUpdate"]== DBNull.Value ? 0 : (Convert.ToInt32(Format["LastUpdate"]));
            int CurrYear = Convert.ToInt32(DateTime.Today.Year);
            string Sequence = Format["Sequence"].ToString();
            if (CurrYear > LastUpdateYear)
            {
                Sequence = "1";
                ResetSequenceById(Id);
            }
            return Format["Format"].ToString().Replace("%S%", Sequence).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
        }

        //Annually Reset Numbering and get Single Squence For All Type
        public static string GetSingleSquence(int Id)
        {
            Dictionary<Object, Object> Format = GetNumberFormatSequence(Id);
            int LastUpdateYear = Format["LastUpdate"] == DBNull.Value ? 0 : (Convert.ToInt32(Format["LastUpdate"]));
            int CurrYear = Convert.ToInt32(DateTime.Today.Year);
            string Sequence = Format["Sequence"].ToString();
            if (CurrYear > LastUpdateYear)
            {
                Sequence = "1";
                ResetSequenceById(Id);
            }
            return Sequence;
        }

        public static bool UpdateSequenceById(int Id)
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = [Sequence] + 1
                                        ,LastUpdate = @Year
                                WHERE Id= @Id";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.String);
            param.Value = Id;
            List.Add(param);
            param = new SqlParameter("@Year", DbType.Int32);
            param.Value = Convert.ToInt32(DateTime.Today.Year);
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static bool ResetSequenceById(int Id)
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = 1
                                        ,LastUpdate = @Year
                                WHERE Id= @Id";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.String);
            param.Value = Id;
            List.Add(param);
            param = new SqlParameter("@Year", DbType.Int32);
            param.Value = Convert.ToInt32(DateTime.Today.Year);
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static Dictionary<Object, Object> GetNumberFormatSequence(int Id)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"SELECT [Format],Sequence,LastUpdate FROM  Numbering WHERE Id=@Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.String);
            param.Value = Id;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("Format", reader["Format"].ToString());
                dictionary.Add("Sequence", reader["Sequence"].ToString());
                dictionary.Add("LastUpdate", reader["LastUpdate"]== DBNull.Value ? 0 : (Convert.ToInt32(reader["LastUpdate"])));
            }
            reader.Close();
            return dictionary;
        }

        public static String FormatPersonName(String FirstName, String MiddleName, String LastName)
        {
            return FirstName + (String.IsNullOrWhiteSpace(MiddleName) ? "" : " " + MiddleName) + (String.IsNullOrWhiteSpace(LastName) ? "" : " " + LastName);
        }
        

        public static String FormatAddress(String Address, String District, String Island, String Atoll, String Country)
        {
            return Address + (String.IsNullOrWhiteSpace(Atoll) ? "" : "/ " + Atoll) + (String.IsNullOrWhiteSpace(Island) ? "" : ". " + Island) + (String.IsNullOrWhiteSpace(District) ? "" : ", " + District);
        }

        public static string ValidationErrorMessage(ModelStateDictionary modelState, ModelMetadata modelMetadata) {
            var errorList = modelState.ToDictionary(kvp => kvp.Key,
                                kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            string result ="Problem with below fields";
            List<ModelMetadata> listModelMetadata;
            string fieldName;

            foreach (KeyValuePair<string, string[]> error in errorList){
                string[] errorMessageList = error.Value;
                listModelMetadata = modelMetadata.Properties.Where(p => p.PropertyName.Equals(error.Key)).ToList();
                if (listModelMetadata.Count > 0)
                {
                    fieldName = listModelMetadata[0].DisplayName;
                }
                else
                {
                    fieldName = error.Key;
                }
                result += "<ul>" + fieldName;
                foreach (string message in errorMessageList) {
                    result += "<li>" + message + "</li>";    
                }
                result += "</ul>";
            }

            return result;
        }

        public static string GetDocumentsRoot(int? shipmentId)
        {
            return "~/App_Data/Documents";
        }
        public static string GetReportPath()
        {
            return "~/Templates/";
        }

        public static string Root(int ShipmentId, HttpServerUtilityBase SERVER)
        {
            string EntryInfo = getEntryInfo(ShipmentId);
            string[] list = EntryInfo.Split('/');
            string serverroot = "~/App_Data/Documents/";
            if (System.IO.Directory.Exists(SERVER.MapPath(serverroot + list[1] + "/" + list[0])))
            {
                return serverroot + list[1] + "/" + list[0];
            }
            else
            {
                System.IO.Directory.CreateDirectory(SERVER.MapPath(serverroot + list[1] + "/" + list[0]));
                if (System.IO.Directory.Exists(SERVER.MapPath(serverroot + list[1] + "/" + list[0])))
                {
                    return serverroot + list[1] + "/" + list[0];
                }
            }
            return null;
        }

        public static string getEntryInfo(int shipmentId)
        {
            string sql = "SELECT CONCAT(Id,'/',YEAR(DateEntered)) AS [Entry] FROM Shipment WHERE Id = @ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter();

            param = new SqlParameter("@ShipmentId", DbType.Int32) { Value = shipmentId };
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            string EntryInfo = "";
            while (reader.Read())
            {
                EntryInfo = (reader["Entry"].ToString());
            }
            reader.Close();

            return EntryInfo;
        }

        /* TODO: Get list of valid extensions from settings */
        public static List<String> GetAllowedFileTypes()
        {
            List<String> list = new List<String>();
            list.Add("pdf");
            list.Add("doc");
            list.Add("docx");
            list.Add("xls");
            list.Add("xlsx");
            list.Add("bmp");
            list.Add("jpg");
            list.Add("jpeg");
            list.Add("gif");
            list.Add("tif");
            list.Add("ppt");
            list.Add("txt");
            return list;
        }

        public static String getContentType(string ext)
        {
            Dictionary<String, String> ct = new Dictionary<string, string>();
            ct.Add("atom", "application/atom+xml");
            ct.Add("avi", "video/x-msvideo");
            ct.Add("bmp", "image/bmp");
            ct.Add("gif", "image/gif");
            ct.Add("htm", "text/html");
            ct.Add("html", "text/html");
            ct.Add("jp2", "image/jp2");
            ct.Add("jpe", "image/jpeg");
            ct.Add("jpeg", "image/jpeg");
            ct.Add("jpg", "image/jpeg");
            ct.Add("js", "application/x-javascript");
            ct.Add("mov", "video/quicktime");
            ct.Add("mp2", "audio/mpeg");
            ct.Add("mp3", "audio/mpeg");
            ct.Add("mp4", "video/mp4");
            ct.Add("mpe", "video/mpeg");
            ct.Add("mpeg", "video/mpeg");
            ct.Add("mpg", "video/mpeg");
            ct.Add("mpga", "audio/mpeg");
            ct.Add("ogg", "application/ogg");
            ct.Add("pdf", "application/pdf");
            ct.Add("png", "image/png");
            ct.Add("ppt", "application/vnd.ms-powerpoint");
            ct.Add("ps", "application/postscript");
            ct.Add("rtf", "text/rtf");
            ct.Add("svg", "image/svg+xml");
            ct.Add("tif", "image/tiff");
            ct.Add("tiff", "image/tiff");
            ct.Add("txt", "text/plain");
            ct.Add("wav", "audio/x-wav");
            ct.Add("xhtml", "application/xhtml+xml");
            ct.Add("xls", "application/vnd.ms-excel");
            ct.Add("zip", "application/zip");

            string contentType;
            if (ct.TryGetValue(ext, out contentType))
            {
                return contentType;
            }
            else
            {
                return "application/octet-stream";
            }
        }

        public static int GetCustomOfficeId(string Code)
        {
            string sql = @"select Id from CustomOffice where Code = @Code";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Code", DbType.String);
            param.Value = Code;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetFreightIndicatorId(string Code)
        {
            string sql = @"select Id from FreightIndicator where Code = @Code";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Code", DbType.String);
            param.Value = Code;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 0;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetBLTypeId(string Code)
        {
            string sql = @"select Id from BLTypes where CODE = @Code";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Code", DbType.String);
            param.Value = Code;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetContainerTypeId(string Name)
        {
            string sql = @"select Id from ContainerType where Name = @Name";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = Name;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetPackageTypeId(string Code)
        {
            string sql = @"select Id from TypeofPackage where CODE = @Code";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Code", DbType.String);
            param.Value = Code;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetContainerIndicatorId(string Name)
        {
            string sql = @"select Id from ContainerIndicator where Name = @Name";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = Name;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            return Id;
        }
        public static int GetShippingAgentId(int ShipmentId)
        {
            string sql = @"select ShippingAgentId from Shipment where Id = @ShipmentId AND ShippingAgentId IS NOT NULL";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 0;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["ShippingAgentId"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetCustomerId(int HBLid)
        {
            string sql = @"select CustomerId from HouseBL where Id = @HBLid";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLid", DbType.Int32);
            param.Value = HBLid;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["CustomerId"]);
            }
            reader.Close();
            return Id;
        }

        public static int GetShipmentId(int HBLid)
        {
            string sql = @"select ShipmentId from HouseBL where Id = @HBLid";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HBLid", DbType.Int32);
            param.Value = HBLid;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 1;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["ShipmentId"]);
            }
            reader.Close();
            return Id;
        }

        public static string GetPartyEmail(int PartyId)
        {
            string sql = @"select Email 
                            from Party AS P
                            INNER JOIN Contact AS C ON C.Id = P.ContactId
                            where P.Id = @PartyId";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter("@PartyId", DbType.Int32) { Value = PartyId };
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            string Email = "";
            while (reader.Read())
            {
                Email = (reader["Email"].ToString());
            }
            reader.Close();
            return Email;
        }

        public static decimal GetMVRExchangeRate()
        {
            string sql = @"select ExchangeRate from Currency where Id = 2";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            decimal Mvr = new decimal();
            while (reader.Read())
            {
                Mvr = Convert.ToDecimal(reader["ExchangeRate"]);
            }
            reader.Close();
            return Mvr;
        }

        public static int GetShipmentLastStatus(int ShipmentId)
        {
            string sql = @"select ShipmentStatusId from ShipmentLastStatus where ShipmentId = @ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = ShipmentId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            int Id = 0;
            while (reader.Read())
            {
                Id = Convert.ToInt32(reader["ShipmentStatusId"]);
            }
            reader.Close();
            return Id;
        }

        public static string ArrivalNoticeTemplate(int HBL, string subject, int EmployeeId)
        {
            //Get Arrival Notice info and bind Data to Template
            Dictionary<object, object> ArrivalNoticeInfo = ManifestModel.GetArrivalNoticeInfo(HBL);
            string ContainerNo = ManifestModel.GetContainerNo(HBL);
            string TypeofPackage = ManifestModel.GetContainerTypeofPackage(HBL);
            string EmployeeName = EmployeeModel.GetEmployee(EmployeeId);

            return @"<div>
                        <table style='width: 100%; margin: 0px; padding: 0px; background-color: rgb(238, 238, 238);' cellspacing='0' cellpadding='0' align='center'>
                        <tbody>
                        <tr>
                        <td style='padding: 20px;'>
	                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); margin: 0px auto;' width='720' cellspacing='0' cellpadding='0' align='center'>
	                        <tbody>
		                        <tr>
			                        <td style='padding: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30);' bgcolor='#FFFFFF'>
				                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 13pt; color: rgb(30, 30, 30); margin: 0px auto;' width='100%' cellspacing='0' cellpadding='0' align='center'>
					                        <tbody>
						                        <tr>
							                        <td style='padding: 0px;' width='190' valign='top' height='90' bgcolor='#FFFFFF'><img src='https://static.wixstatic.com/media/c0d565_bcbf26f106d349b691b4b66eb301d889~mv2.png/v1/fill/w_281,h_108,al_c,q_85,usm_0.66_1.00_0.01/Asia%20Forwarding.png' style='background-color:#FFFFFF;'></td>
							                        <td style='padding: 10px; color: rgb(51, 51, 51); font-size: 12px;' valign='top'>
								                        <strong style='font-size: 18px;'>ASIA FORWARDING (PVT) LTD</strong><br>
								                        <font style='font-size: 11px;'>Omadu Fannu, Level-3, Haveeree Hingun, Maafannu, 20280&nbsp; MALE, MALDIVES</font><br>
								                        <font style='font-size: 11px;'>Company Reg No: C-125/2005</font><br>
								                        <font style='font-size: 11px;'>Hotline: (960) 7983041</font><br>
								                        <font style='font-size: 11px;'>Tel: (960) 3343041, 3343042, 3343043 Fax: (960) 3343040</font><br>
								                        <font style='font-size: 11px;'>Email: info@theasiaforwarding.com, Website: www.theasiaforwarding.com</font>
							                        </td>
						                        </tr>
					                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 6px 240px;' bgcolor='#f99500'>
				                        <div style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 20px; line-height: 14pt; font-weight: bold; margin: 0px; display: block; color: rgb(255, 255, 255);'>
				                        Arrival Notice"+subject+@"</div>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 15px 20px; font-family: Arial, Helvetica, sans-serif;' bgcolor='#FFFFFF'>
				                        <h2 style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 14px; line-height: 16pt; color: rgb(30, 30, 30); font-weight: bold; margin: 0px;'>
					                        <span style='color: rgb(42, 143, 189);'><strong>ETA</strong></span><strong> " + ArrivalNoticeInfo["DateArrival"] + @"</strong>
				                        </h2>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' valign='top' bgcolor='#FFFFFF'>
				                        <table width='100%' cellspacing='0' cellpadding='3' border='0'>
				                        <tbody>
					                        <tr>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>SHIPPER</strong></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["Shipper"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>CONSIGNEE</strong></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["CustomerName"] + @"</font><br>
							                        <font style='font-size: 11px;'>" + ArrivalNoticeInfo["CustomerAddress"] + @"</font><br>
							                        <font style='font-size: 11px;'>" + ArrivalNoticeInfo["CustomerNation"] + @"</font><br>
							                        <font style='font-size: 11px;'>" + ArrivalNoticeInfo["CustomerRegNo"] + @"</font>
						                        </td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' valign='top' bgcolor='#FFFFFF'>
				                        <table width='100%' cellspacing='0' cellpadding='3' border='0'>
				                        <tbody>
					                        <tr>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>JOB #</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["JobNo"] + @"</font></td>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VESSEL</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["Vessel"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>HBL #</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["HouseBL"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VOYAGE</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["VoyageNo"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>SERIAL #</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>-</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>POL</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["PortofLoading"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>MBL #</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["MasterBL"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>POD</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["PortofDelivery"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>FREIGHT TERM</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["FreightIndicator"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>COMMODITY</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["TypeofCommodity"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>COMMERCIAL TERM</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'>&nbsp;</td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>NO. OF PKGS</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["NoOfPackage"] + " " + TypeofPackage + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VOLUME (M3)</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["Measurement"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>GROSS WT. (KG)</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + ArrivalNoticeInfo["Weight"] + @"</font></td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' bgcolor='#FFFFFF'>
			                        <span style='color: rgb(42, 143, 189);'><strong>CONTAINER # : </strong></span><font style='font-size: 11px;'> " + ContainerNo + @"</font</td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 17px 20px 12px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px dashed rgb(238, 238, 238);' bgcolor='#f4f4f4'>
				                        <table style='border-collapse: collapse; border-spacing: 0px; border-width: 0px;' width='720' cellspacing='0' cellpadding='0'>
				                        <tbody>
					                        <tr>
						                        <td style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 12pt; color: rgb(30, 30, 30); text-align: left;' valign='top'>
							                        <table width='100%' cellspacing='0' cellpadding='3' border='0' align='center'>
							                        <tbody>
							                        <tr>
								                        <td style='font-size: 11px;' width='90' align='left'>SENT BY</td>
								                        <td style='font-size: 11px;' width='300' align='left'>:&nbsp;&nbsp;" + EmployeeName + @"</td>
								                        <td>&nbsp;</td>
								                        <td>&nbsp;</td>
							                        </tr>
							                        <tr>
								                        <td style='font-size: 11px;' align='left'>DATE / TIME</td>
								                        <td style='font-size: 11px;' align='left'>:&nbsp;&nbsp;" +DateTime.Now+@"</td>
								                        <td colspan='2' style='font-size: 10px; color: rgb(51, 51, 51);' align='right'>&nbsp;</td>
							                        </tr>
							                        </tbody>
							                        </table>
						                        </td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
	                        </tbody>
	                        </table>
                        </td>
                        </tr>
                        </tbody>
                        </table>
                        </div>";
        }

        public static string DebitNoteTemplate(int SPId, int EmployeeId)
        {
            //Get Debit Note info and bind Data to Template
            Dictionary<object, object> DebitNoteInfo = ManifestModel.GetDebitNoteInfo(SPId);
            string HBLTable = ManifestModel.GetHBLList(SPId);
            string EmployeeName = EmployeeModel.GetEmployee(EmployeeId);
            return @"<div>
                        <table style='width: 100%; margin: 0px; padding: 0px; background-color: rgb(238, 238, 238);' cellspacing='0' cellpadding='0' align='center'>
                        <tbody>
                        <tr>
                        <td style='padding: 20px;'>
	                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); margin: 0px auto;' width='720' cellspacing='0' cellpadding='0' align='center'>
	                        <tbody>
		                        <tr>
			                        <td style='padding: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30);' bgcolor='#FFFFFF'>
				                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 13pt; color: rgb(30, 30, 30); margin: 0px auto;' width='100%' cellspacing='0' cellpadding='0' align='center'>
					                        <tbody>
						                        <tr>
							                        <td style='padding: 0px;' width='190' valign='top' height='90' bgcolor='#FFFFFF'><img src='https://static.wixstatic.com/media/c0d565_bcbf26f106d349b691b4b66eb301d889~mv2.png/v1/fill/w_281,h_108,al_c,q_85,usm_0.66_1.00_0.01/Asia%20Forwarding.png' style='background-color:#FFFFFF;'></td>
							                        <td style='padding: 10px; color: rgb(51, 51, 51); font-size: 12px;' valign='top'>
								                        <strong style='font-size: 18px;'>ASIA FORWARDING (PVT) LTD</strong><br>
								                        <font style='font-size: 11px;'>Omadu Fannu, Level-3, Haveeree Hingun, Maafannu, 20280&nbsp; MALE, MALDIVES</font><br>
								                        <font style='font-size: 11px;'>Company Reg No: C-125/2005</font><br>
								                        <font style='font-size: 11px;'>Hotline: (960) 7983041</font><br>
								                        <font style='font-size: 11px;'>Tel: (960) 3343041, 3343042, 3343043 Fax: (960) 3343040</font><br>
								                        <font style='font-size: 11px;'>Email: info@theasiaforwarding.com, Website: www.theasiaforwarding.com</font>
							                        </td>
						                        </tr>
					                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 6px 180px;' bgcolor='#f99500'>
				                        <div style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 20px; line-height: 14pt; font-weight: bold; margin: 0px; display: block; color: rgb(255, 255, 255);'>
				                        Request For Debit Note / Freight Invoice</div>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' valign='top' bgcolor='#FFFFFF'>
				                        <table width='100%' cellspacing='0' cellpadding='3' border='0'>
				                        <tbody>
					                        <tr>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>MANIFEST #</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + DebitNoteInfo["ManifestNo"] + @"</font></td>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VESSEL</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + DebitNoteInfo["VesselName"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>MBL #</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + DebitNoteInfo["MasterBL"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VOYAGE</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + DebitNoteInfo["VoyageNo"] + @"</font></td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' bgcolor='#FFFFFF'>
										<table width='100%' cellspacing='0' cellpadding='3' border='1'>
										<caption>HBL Information</caption>
				                        <tbody>
					                        <tr>
												<td width='10' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>#</strong></span></td>
						                        <td width='70' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>HBL</strong></span></td>
						                        <td width='180' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>CONSIGNEE</strong></span></td>
						                        <td width='50' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>FREIGHT INDICATOR</strong></span></td>
												<td width='80' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>JOB #</strong></span></td>
					                        </tr>"
	                                            + HBLTable +
				                        @"</tbody>
				                        </table>
									</td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 17px 20px 12px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px dashed rgb(238, 238, 238);' bgcolor='#f4f4f4'>
				                        <table style='border-collapse: collapse; border-spacing: 0px; border-width: 0px;' width='720' cellspacing='0' cellpadding='0'>
				                        <tbody>
					                        <tr>
						                        <td style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 12pt; color: rgb(30, 30, 30); text-align: left;' valign='top'>
							                        <table width='100%' cellspacing='0' cellpadding='3' border='0' align='center'>
							                        <tbody>
							                        <tr>
								                        <td style='font-size: 11px;' width='90' align='left'>SENT BY</td>
								                        <td style='font-size: 11px;' width='300' align='left'>:&nbsp;&nbsp;" + EmployeeName + @"</td>
								                        <td>&nbsp;</td>
								                        <td>&nbsp;</td>
							                        </tr>
							                        <tr>
								                        <td style='font-size: 11px;' align='left'>DATE / TIME</td>
								                        <td style='font-size: 11px;' align='left'>:&nbsp;&nbsp;" + DateTime.Now + @"</td>
								                        <td colspan='2' style='font-size: 10px; color: rgb(51, 51, 51);' align='right'>&nbsp;</td>
							                        </tr>
							                        </tbody>
							                        </table>
						                        </td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
	                        </tbody>
	                        </table>
                        </td>
                        </tr>
                        </tbody>
                        </table>
                        </div>";
        }

        public static string PODTemplate(int HBL, int EmployeeId)
        {
            //Get POD info and bind Data to Template
            Dictionary<object, object> PODinfo = ManifestModel.GetPODInfo(HBL);
            string EmployeeName = EmployeeModel.GetEmployee(EmployeeId);
            return @"<div>
                        <table style='width: 100%; margin: 0px; padding: 0px; background-color: rgb(238, 238, 238);' cellspacing='0' cellpadding='0' align='center'>
                        <tbody>
                        <tr>
                        <td style='padding: 20px;'>
	                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); margin: 0px auto;' width='720' cellspacing='0' cellpadding='0' align='center'>
	                        <tbody>
		                        <tr>
			                        <td style='padding: 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30);' bgcolor='#FFFFFF'>
				                        <table style='border-collapse: collapse; text-align: left; font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 12px; line-height: 13pt; color: rgb(30, 30, 30); margin: 0px auto;' width='100%' cellspacing='0' cellpadding='0' align='center'>
					                        <tbody>
						                        <tr>
							                        <td style='padding: 0px;' width='190' valign='top' height='90' bgcolor='#FFFFFF'><img src='https://static.wixstatic.com/media/c0d565_bcbf26f106d349b691b4b66eb301d889~mv2.png/v1/fill/w_281,h_108,al_c,q_85,usm_0.66_1.00_0.01/Asia%20Forwarding.png' style='background-color:#FFFFFF;'></td>
							                        <td style='padding: 10px; color: rgb(51, 51, 51); font-size: 12px;' valign='top'>
								                        <strong style='font-size: 18px;'>ASIA FORWARDING (PVT) LTD</strong><br>
								                        <font style='font-size: 11px;'>Omadu Fannu, Level-3, Haveeree Hingun, Maafannu, 20280&nbsp; MALE, MALDIVES</font><br>
								                        <font style='font-size: 11px;'>Company Reg No: C-125/2005</font><br>
								                        <font style='font-size: 11px;'>Hotline: (960) 7983041</font><br>
								                        <font style='font-size: 11px;'>Tel: (960) 3343041, 3343042, 3343043 Fax: (960) 3343040</font><br>
								                        <font style='font-size: 11px;'>Email: info@theasiaforwarding.com, Website: www.theasiaforwarding.com</font>
							                        </td>
						                        </tr>
					                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 6px 240px;' bgcolor='#f99500'>
				                        <div style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 20px; line-height: 14pt; font-weight: bold; margin: 0px; display: block; color: rgb(255, 255, 255);'>
				                        DOCUMENTS RELEASE</div>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' valign='top' bgcolor='#FFFFFF'>
				                        <table width='100%' cellspacing='0' cellpadding='3' border='0'>
				                        <tbody>
					                        <tr>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>MANIFEST #</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + PODinfo["ManifestNo"] + @"</font></td>
						                        <td width='120' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VESSEL</strong></span></td>
						                        <td width='5' valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + PODinfo["VesselName"] + @"</font></td>
					                        </tr>
					                        <tr>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>MBL #</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + PODinfo["MasterBL"] + @"</font></td>
						                        <td valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>VOYAGE</strong></span></td>
						                        <td valign='top'><font style='font-size: 11px;'>:</font></td>
						                        <td valign='top'><font style='font-size: 11px;'>" + PODinfo["VoyageNo"] + @"</font></td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 10px 20px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px solid rgb(238, 238, 238);' bgcolor='#FFFFFF'>
										<table width='100%' cellspacing='0' cellpadding='3' border='1'>
										<caption>HBL Information</caption>
				                        <tbody>
					                        <tr>
						                        <td width='70' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>HBL</strong></span></td>
						                        <td width='180' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>CONSIGNEE</strong></span></td>
						                        <td width='50' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>FREIGHT INDICATOR</strong></span></td>
												<td width='80' valign='top'><span style='color: rgb(42, 143, 189);'><strong style='font-size: 12px;'>JOB #</strong></span></td>
					                        </tr>
											<tr>
												<td width='80' valign='top'><font style='font-size: 12px;'>" + PODinfo["HouseBL"] + @"</font></td>
												<td width='180' valign='top'><font style='font-size: 12px;'>" + PODinfo["Consignee"] + @"</font></td>
												<td width='60' valign='top'><font style='font-size: 12px;'>" + PODinfo["FreightIndicator"] + @"</font></td>
												<td width='70' valign='top'><font style='font-size: 12px;'>" + PODinfo["JobNo"] + @"</font></td>
											</tr>
										</tbody>
				                        </table>
									</td>
		                        </tr>
		                        <tr>
			                        <td style='padding: 17px 20px 12px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 15pt; color: rgb(30, 30, 30); border-top: 1px dashed rgb(238, 238, 238);' bgcolor='#f4f4f4'>
				                        <table style='border-collapse: collapse; border-spacing: 0px; border-width: 0px;' width='720' cellspacing='0' cellpadding='0'>
				                        <tbody>
					                        <tr>
						                        <td style='padding: 0px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; line-height: 12pt; color: rgb(30, 30, 30); text-align: left;' valign='top'>
							                        <table width='100%' cellspacing='0' cellpadding='3' border='0' align='center'>
							                        <tbody>
							                        <tr>
								                        <td style='font-size: 11px;' width='90' align='left'>SENT BY</td>
								                        <td style='font-size: 11px;' width='300' align='left'>:&nbsp;&nbsp;" + EmployeeName + @"</td>
								                        <td>&nbsp;</td>
								                        <td>&nbsp;</td>
							                        </tr>
							                        <tr>
								                        <td style='font-size: 11px;' align='left'>DATE / TIME</td>
								                        <td style='font-size: 11px;' align='left'>:&nbsp;&nbsp;" + DateTime.Now + @"</td>
								                        <td colspan='2' style='font-size: 10px; color: rgb(51, 51, 51);' align='right'>&nbsp;</td>
							                        </tr>
							                        </tbody>
							                        </table>
						                        </td>
					                        </tr>
				                        </tbody>
				                        </table>
			                        </td>
		                        </tr>
	                        </tbody>
	                        </table>
                        </td>
                        </tr>
                        </tbody>
                        </table>
                        </div>";
        }

    }
}