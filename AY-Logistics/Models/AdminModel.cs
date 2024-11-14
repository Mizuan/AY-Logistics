using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using MyDBAccess;
using System.ComponentModel.DataAnnotations;

namespace AYLogistics.Models
{
    public class Designation_Settings
    {
        [Required(ErrorMessage = "cannot leave as blank")]
        public string EmpDesignation { get; set; }
        public int DesignationId { get; set; }
        public string UpdateDesignation { get; set; }

        public bool AddDesignation()
        {
            string sql = @"INSERT INTO [dbo].[Designation]
                               (Designation)
                            VALUES
                               (@Designation)";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Designation", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(EmpDesignation) ? (object)DBNull.Value : EmpDesignation;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static IQueryable<Designation_Settings> GetAllDesignations()
        {
            string sql = "SELECT Id,Designation FROM Designation;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Designation_Settings> list = new List<Designation_Settings>();
            while (reader.Read())
            {
                Designation_Settings Designations = new Designation_Settings { DesignationId = (int)reader[0], EmpDesignation = (string)reader[1] };
                list.Add(Designations);
            }
            reader.Close();
            return list.AsQueryable<Designation_Settings>();
        }

        public bool EditDesignation()
        {
            string sql = @"UPDATE [dbo].[Designation]
                               SET [Designation] = @Designation
                             WHERE Id= @DesignationId";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@DesignationId", DbType.Int32);
            param.Value = DesignationId;
            list.Add(param);

            param = new SqlParameter("@Designation", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(UpdateDesignation) ? (object)DBNull.Value : UpdateDesignation;
            list.Add(param);

            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }
    }
}