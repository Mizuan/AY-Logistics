using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MyDBAccess;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AYLogistics.Models
{
    public class JobDocumentVM
    {
        public int? Id { get; set; }

      //  [Required(ErrorMessage = "*")]
        [Display(Name = "Document Type")]
        public int DocumentType { get; set; }

       // [Required(ErrorMessage = "*")]
        [Display(Name = "Document Name")]
        public string Name { get; set; }

        [Display(Name = "File Name")]
        public string Filename { get; set; }
        public string edit { get; set; }
        public string backup { get; set; }
        public string OriginalFilename { get; set; }

        [Required(ErrorMessage = "*")]
        public int ShipmentId { get; set; }

      //  [Required(ErrorMessage = "*")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string TemplateName { get; set; }
        public int? HouseBLId { get; set; }
        public int specificBLid { get; set; }

       // [Required(ErrorMessage = "Please select file.")]
       // [Display(Name = "Browse File")]
       // [DataType(DataType.Upload)]
        public HttpPostedFileBase[] files { get; set; }


        public static Dictionary<String, String> GetJobDocumentTypes()
        {
            String sql = @"SELECT Id,Name FROM DocumentTypes;";

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

        public static Dictionary<String, String> GetBLList(int shipmentId, int? HBLId)
        {
            String sql = @"select Id,HouseBL from HouseBL where DateRemoved IS NULL AND ShipmentId = @shipmentId AND HouseBL IS NOT NULL --%onlyBL%";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@shipmentId", SqlDbType.Int);
            param.Value = shipmentId;
            list.Add(param);

            if (HBLId.HasValue)
            {
                sql = sql.Replace("--%onlyBL%", "AND Id = @HBLId");
                param = new SqlParameter("@HBLId", SqlDbType.Int);
                param.Value = HBLId.Value;
                list.Add(param);
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, list);

            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            while (reader.Read())
            {
                dictionary.Add(reader["Id"].ToString(), reader["HouseBL"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<JobDocumentVM> GetJobDocuments(int shipmentId, int? HBLId)
        {
            
            string sql = @"SELECT JD.Id,
	                               JD.DocumentTypesId,
	                               DT.Name AS DocumentType,
	                               JD.Name,
	                               JD.Filename,
	                               JD.ShipmentId,
	                               JD.Description,
                                   JD.HouseBLId
                            FROM JobDocuments AS JD 
                            INNER JOIN DocumentTypes AS DT ON JD.DocumentTypesId = DT.Id
                            WHERE JD.ShipmentId= @shipmentId AND JD.DateRemoved IS NULL --%onlyBL%";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@shipmentId", SqlDbType.Int);
            param.Value = shipmentId;
            list.Add(param);

            if (HBLId.HasValue)
            {
                sql = sql.Replace("--%onlyBL%", "OR JD.HouseBLId IS NULL AND JD.HouseBLId = @HBLId"); // Display All including Shipment DOcs to BL
                param = new SqlParameter("@HBLId", SqlDbType.Int);
                param.Value = HBLId.Value;
                list.Add(param);
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            List<JobDocumentVM> result = new List<JobDocumentVM>();
            while (reader.Read())
            {
                JobDocumentVM cdvm = new JobDocumentVM();
                cdvm.Id = Convert.ToInt32(reader["Id"]);
                cdvm.DocumentType = Convert.ToInt32(reader["DocumentTypesId"]);
                if (reader["HouseBLId"] == DBNull.Value)
                {
                    cdvm.HouseBLId = 0;
                }
                else
                {
                    cdvm.HouseBLId = Convert.ToInt32(reader["HouseBLId"]);
                }
                cdvm.Name = reader["Name"].ToString();
                cdvm.Filename = ""; // TODO: redo the model without this. reader["FileName"].ToString();
                cdvm.ShipmentId = Convert.ToInt32(reader["ShipmentId"]);
                cdvm.Description = reader["Description"].ToString();
                result.Add(cdvm);
            }
            reader.Close();
            return result;
        }

        public static List<JobDocumentVM> GetRemovedDocuments(int shipmentId, int? HBLId)
        {
            string sql = @"SELECT JD.Id,
	                               JD.DocumentTypesId,
	                               DT.Name AS DocumentType,
	                               JD.Name,
	                               JD.Filename,
	                               JD.ShipmentId,
	                               JD.Description,
                                   JD.HouseBLId
                            FROM JobDocuments AS JD 
                            INNER JOIN DocumentTypes AS DT ON JD.DocumentTypesId = DT.Id
                            WHERE JD.ShipmentId= @shipmentId AND JD.DateRemoved IS NOT NULL --%onlyBL%;";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@shipmentId", SqlDbType.Int);
            param.Value = shipmentId;
            list.Add(param);

            if (HBLId.HasValue)
            {
                sql = sql.Replace("--%onlyBL%", "AND HouseBLId IS NULL OR HouseBLId = @HBLId");
                param = new SqlParameter("@HBLId", SqlDbType.Int);
                param.Value = HBLId.Value;
                list.Add(param);
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            List<JobDocumentVM> result = new List<JobDocumentVM>();
            while (reader.Read())
            {
                JobDocumentVM cdvm = new JobDocumentVM();
                cdvm.Id = Convert.ToInt32(reader["Id"]);
                cdvm.DocumentType = Convert.ToInt32(reader["DocumentTypesId"]);
                if (reader["HouseBLId"] == DBNull.Value)
                {
                    cdvm.HouseBLId = 0;
                }
                else
                {
                    cdvm.HouseBLId = Convert.ToInt32(reader["HouseBLId"]);
                }
                cdvm.Name = reader["Name"].ToString();
                cdvm.Filename = ""; // TODO: redo the model without this. reader["FileName"].ToString();
                cdvm.ShipmentId = Convert.ToInt32(reader["ShipmentId"]);
                cdvm.Description = reader["Description"].ToString();
                result.Add(cdvm);
            }
            reader.Close();
            return result;
        }

        public Dictionary<String, String> AddDocument(SqlTransaction Transaction = null)
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"
            INSERT INTO [dbo].[JobDocuments]
                   ([DocumentTypesId]
                   ,[Name]
                   ,[Filename]
                   ,[ShipmentId]
                   ,[Description]
                   ,[DateAdded]
                   ,[HouseBLId])
             OUTPUT inserted.Id
	         VALUES
                   (@DocumentTypesId
                   ,@Name
                   ,@Filename
                   ,@ShipmentId
                   ,@Description
                   ,GETDATE()
                   ,@HouseBLId)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;

            param = new SqlParameter("@DocumentTypesId", SqlDbType.Int);
            param.Value = this.DocumentType;
            list.Add(param);
            param = new SqlParameter("@Name", SqlDbType.NVarChar);
            //param.Value = this.Name;
            param.Value = this.OriginalFilename;
            list.Add(param);
            param = new SqlParameter("@Filename", SqlDbType.NVarChar);
            param.Value = this.Filename;
            list.Add(param);
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);
            param = new SqlParameter("@Description", SqlDbType.NVarChar);
            param.Value = this.Description;
            list.Add(param);
            param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = DBNull.Value;
            if (HouseBLId > 0)
            {
                param.Value = HouseBLId;
            }
            list.Add(param);

            if (Transaction != null)
            {
                if (DBAccess.ExecuteSQLInTransaction(Transaction, sql, list) > 0)
                {
                    dictionary.Add("Status", "success");
                    return dictionary;
                }
                else
                {
                    dictionary.Add("Status", "error");
                    return dictionary;
                }
            }
            else
            {
                int id = DBAccess.Insert2(sql, list);
                if (id > 0)
                {
                    dictionary.Add("Id", id.ToString());
                    dictionary.Add("Message", "Document has been uploaded successfully");
                    dictionary.Add("Status", "success");
                }
                else
                {
                    dictionary.Add("Message", "Failed!");
                    dictionary.Add("Status", "error");
                }
                return dictionary;
            }
        }

        public Dictionary<String, String> SaveDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DocumentTypesId] = @DocumentTypesId,
                            [HouseBLId] = @HouseBLId,
                            [Name] = @Name,
                            [Description] = @Description
                    WHERE Id=@Id AND ShipmentId=@ShipmentId AND DateRemoved IS NULL";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);
            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);
            param = new SqlParameter("@DocumentTypesId", SqlDbType.Int);
            param.Value = this.DocumentType;
            list.Add(param);
            param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = DBNull.Value;
            if (HouseBLId > 0)
            {
                param.Value = HouseBLId;
            }
            list.Add(param);
            param = new SqlParameter("@Name", SqlDbType.NVarChar);
            param.Value = this.Name;
            list.Add(param);
            param = new SqlParameter("@Description", SqlDbType.NVarChar);
            param.Value = this.Description;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been Modified successfully");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Upload Fail!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }

        public Dictionary<String, String> RemoveDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DateRemoved] = GETDATE()
                    WHERE Id=@Id AND ShipmentId=@ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);

            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been removed");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Failed!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }

        public Dictionary<String, String> RestoreDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DateRemoved] = NULL
                    WHERE Id=@Id AND ShipmentId=@ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);

            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been accepted");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Failed!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }

        public static bool GetDocumentFilename(int shipmentId, int id, out string filename, out string name)
        {
            filename = "";
            name = "";
            string sql = @"SELECT Filename,Name FROM [JobDocuments]
                           WHERE Id=@Id AND ShipmentId=@ShipmentId ;";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = shipmentId;
            list.Add(param);

            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = id;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            if (reader.Read())
            {
                filename = reader["Filename"].ToString();
                name = reader["Name"].ToString();
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        public static bool GetDocumentFilePath(int HBLId, int DocTypeId, out string filename, out string name)
        {
            filename = "";
            name = "";
            string sql = @"select Top 1 [Filename],Name from JobDocuments where HouseBLId = @HBLId AND DocumentTypesId = @DocTypeId order by DateAdded DESC";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@HBLId", SqlDbType.Int);
            param.Value = HBLId;
            list.Add(param);

            param = new SqlParameter("@DocTypeId", SqlDbType.Int);
            param.Value = DocTypeId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list);
            if (reader.Read())
            {
                filename = reader["Filename"].ToString();
                name = reader["Name"].ToString();
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
    }

    public class JobDocument // edit Documets
    {
        public int? Id { get; set; }
        public int DocumentType { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string edit { get; set; }
        public string backup { get; set; }
        public int ShipmentId { get; set; }
        public string Description { get; set; }
        public string TemplateName { get; set; }
        public int? HouseBLId { get; set; }
        public int specificBLid { get; set; }

        public Dictionary<String, String> RemoveDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DateRemoved] = GETDATE()
                    WHERE Id=@Id AND ShipmentId=@ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);

            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been removed");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Failed!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }
        public Dictionary<String, String> SaveDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DocumentTypesId] = @DocumentTypesId,
                            [HouseBLId] = @HouseBLId,
                            [Name] = @Name,
                            [Description] = @Description
                    WHERE Id=@Id AND ShipmentId=@ShipmentId AND DateRemoved IS NULL";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);
            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);
            param = new SqlParameter("@DocumentTypesId", SqlDbType.Int);
            param.Value = this.DocumentType;
            list.Add(param);
            param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = DBNull.Value;
            if (HouseBLId > 0)
            {
                param.Value = HouseBLId;
            }
            list.Add(param);
            param = new SqlParameter("@Name", SqlDbType.NVarChar);
            param.Value = this.Name;
            list.Add(param);
            param = new SqlParameter("@Description", SqlDbType.NVarChar);
            param.Value = this.Description;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been Modified successfully");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Upload Fail!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }
        public Dictionary<String, String> RestoreDocument()
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();
            string sql = @"UPDATE [JobDocuments]
                        SET
	                        [DateRemoved] = NULL
                    WHERE Id=@Id AND ShipmentId=@ShipmentId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param;
            param = new SqlParameter("@ShipmentId", SqlDbType.Int);
            param.Value = this.ShipmentId;
            list.Add(param);

            param = new SqlParameter("@Id", SqlDbType.Int);
            param.Value = this.Id;
            list.Add(param);

            if (DBAccess.Update(sql, list))
            {
                dictionary.Add("Message", "Document has been accepted");
                dictionary.Add("Status", "success");
            }
            else
            {
                dictionary.Add("Message", "Failed!");
                dictionary.Add("Status", "error");
            }
            return dictionary;
        }


    }

}