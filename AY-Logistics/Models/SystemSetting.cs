using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using MyDBAccess;
using System.Data;

namespace AYLogistics.Models
{
    public class SystemSetting
    {
        [Display(Name = "Manifest Number Format")]
        public string ManifestFormat { get; set; }
        [Display(Name = "Sequence")]
        public int ManifestSequence { get; set; }

        [Display(Name = "Job Number Format (SEA)")]
        public string JobFormatSEA { get; set; }
        [Display(Name = "Job Number Format (AIR)")]
        public string JobFormatAIR { get; set; }
        [Display(Name = "Job Number Format (POST)")]
        public string JobFormatPOST { get; set; }
        [Display(Name = "Sequence")]
        public int JobSequence { get; set; }

        [Display(Name = "House BL Number Format")]
        public string HouseBLFormat { get; set; }
        [Display(Name = "Sequence")]
        public int HBLSequence { get; set; }

        [Display(Name = "Quotation Number Format")]
        public string QuotFormat { get; set; }
        [Display(Name = "Sequence")]
        public int QuotSequence { get; set; }

        [Display(Name = "Daily Clearance Number Format")]
        public string DCFormat { get; set; }
        [Display(Name = "Sequence")]
        public int DCSequence { get; set; }


        [Display(Name = "New Type of Commodity")]
        public string CommodityName { get; set; }
        [Display(Name = "Available Types")]
        public int commodityId { get; set; }

        [Display(Name = "New Type of Document")]
        public string DocumentName { get; set; }
        [Display(Name = "Available Types")]
        public int documentId { get; set; }

        public SystemSetting()
        {
            GetManifestFormating();
            GetJobSEAFormating();
            GetJobAIRFormating();
            GetJobPOSTFormating();
            GetQuotationFormating();
            GetHBLFormating();
            GetDCFormating();
        }

        public void GetManifestFormating()
        {
            string sql = @"select [Format], Sequence from Numbering where Id = 1";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                ManifestFormat = reader["Format"].ToString();
                ManifestSequence = Convert.ToInt32(reader["Sequence"]);

            }
            reader.Close();
        }
        public void GetJobSEAFormating()
        {
            string sql = @"select [Format], Sequence from Numbering where Id = 2";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                JobFormatSEA = reader["Format"].ToString();
                JobSequence = Convert.ToInt32(reader["Sequence"]);

            }
            reader.Close();
        }

        public void GetQuotationFormating()
        {
            string sql = @"select [Format], Sequence from Numbering where Id = 5";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                QuotFormat = reader["Format"].ToString();
                QuotSequence = Convert.ToInt32(reader["Sequence"]);

            }
            reader.Close();
        }
        public void GetHBLFormating()
        {
            string sql = @"select [Format], Sequence from Numbering where Id = 6";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                HouseBLFormat = reader["Format"].ToString();
                HBLSequence = Convert.ToInt32(reader["Sequence"]);

            }
            reader.Close();
        }

        public void GetDCFormating()
        {
            string sql = @"select [Format], Sequence from Numbering where Id = 7";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                DCFormat = reader["Format"].ToString();
                DCSequence = Convert.ToInt32(reader["Sequence"]);

            }
            reader.Close();
        }

        public void GetJobAIRFormating()
        {
            string sql = @"select [Format] from Numbering where Id = 3";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                JobFormatAIR = reader["Format"].ToString();

            }
            reader.Close();
        }
        public void GetJobPOSTFormating()
        {
            string sql = @"select [Format] from Numbering where Id = 4";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                JobFormatPOST = reader["Format"].ToString();

            }
            reader.Close();
        }


        public bool UpdateManifestFormating()
        {
            string sql = @"UPDATE [dbo].[Numbering]
                           SET [Format] = @Format
                              ,[Sequence] = @sequence
                         WHERE Id = 1";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Format", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(ManifestFormat) ? (object)DBNull.Value : ManifestFormat;
            list.Add(param);

            param = new SqlParameter("@sequence", DbType.Int32);
            param.Value = ManifestSequence;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool UpdateQuotFormating()
        {
            string sql = @"UPDATE [dbo].[Numbering]
                           SET [Format] = @Format
                              ,[Sequence] = @sequence
                         WHERE Id = 5";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Format", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(QuotFormat) ? (object)DBNull.Value : QuotFormat;
            list.Add(param);

            param = new SqlParameter("@sequence", DbType.Int32);
            param.Value = QuotSequence;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool UpdateDCFormating()
        {
            string sql = @"UPDATE [dbo].[Numbering]
                           SET [Format] = @Format
                              ,[Sequence] = @sequence
                         WHERE Id = 7";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Format", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(QuotFormat) ? (object)DBNull.Value : DCFormat;
            list.Add(param);

            param = new SqlParameter("@sequence", DbType.Int32);
            param.Value = DCSequence;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool UpdateBLFormating()
        {
            string sql = @"UPDATE [dbo].[Numbering]
                           SET [Format] = @Format
                              ,[Sequence] = @sequence
                         WHERE Id = 6";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Format", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(HouseBLFormat) ? (object)DBNull.Value : HouseBLFormat;
            list.Add(param);

            param = new SqlParameter("@sequence", DbType.Int32);
            param.Value = HBLSequence;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool UpdateJobFormating()
        {
            string sql = @"UPDATE [dbo].[Numbering]
                           SET [Format] = @FormatSEA
                              ,[Sequence] = @sequence
                         WHERE Id = 2

                         UPDATE [dbo].[Numbering]
                           SET [Format] = @FormatAIR
                         WHERE Id = 3

                         UPDATE [dbo].[Numbering]
                           SET [Format] = @FormatPOST
                         WHERE Id = 4";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@FormatSEA", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(JobFormatSEA) ? (object)DBNull.Value : JobFormatSEA;
            list.Add(param);
            param = new SqlParameter("@FormatAIR", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(JobFormatAIR) ? (object)DBNull.Value : JobFormatAIR;
            list.Add(param);
            param = new SqlParameter("@FormatPOST", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(JobFormatPOST) ? (object)DBNull.Value : JobFormatPOST;
            list.Add(param);

            param = new SqlParameter("@sequence", DbType.Int32);
            param.Value = JobSequence;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static IQueryable<SystemSetting> GetCommodities()
        {
            string sql = "SELECT Id,Name FROM TypeOfCommodity;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SystemSetting> list = new List<SystemSetting>();
            while (reader.Read())
            {
                SystemSetting commodities = new SystemSetting { commodityId = (int)reader[0], CommodityName = (string)reader[1] };
                list.Add(commodities);
            }
            reader.Close();
            return list.AsQueryable<SystemSetting>();
        }

        public bool AddCommodity()
        {
            string sql = @"INSERT INTO [dbo].[TypeOfCommodity]
                                   ([Name])
                             VALUES
                                   (@Name)";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(CommodityName) ? (object)DBNull.Value : CommodityName;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static IQueryable<SystemSetting> GetDocAvailableTypes()
        {
            string sql = "SELECT Id,Name FROM DocumentTypes;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SystemSetting> list = new List<SystemSetting>();
            while (reader.Read())
            {
                SystemSetting availableDocsType = new SystemSetting { documentId = (int)reader[0], DocumentName = (string)reader[1] };
                list.Add(availableDocsType);
            }
            reader.Close();
            return list.AsQueryable<SystemSetting>();
        }
        public bool AddDocType()
        {
            string sql = @"INSERT INTO [dbo].[DocumentTypes]
                                   ([Name])
                             VALUES
                                   (@Name)";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(DocumentName) ? (object)DBNull.Value : DocumentName;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }
    }

}