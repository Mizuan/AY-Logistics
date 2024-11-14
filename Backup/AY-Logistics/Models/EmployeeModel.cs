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
    public class EmployeeModel
    {
        public int Id { get; set; }
        
        [Display(Name = "NIC/PassPort No")]        
        public string NIC { get; set; }

        [Display(Name = "Employee Id")]
        public string EmpId { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Required format is .(960)-7994181")]
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is required (we promise not to spam you!).")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Employee Status")]
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatusName { get; set; }

        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public string Designation { get; set; }


               //Constructor
        public EmployeeModel()
        {

        }

        public static IQueryable<EmployeeModel> GetEmployeeStatus()
        {
            string sql = "SELECT Id,Status FROM EmployeeStatus;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<EmployeeModel> list = new List<EmployeeModel>();
            while (reader.Read())
            {
                EmployeeModel EmployeeStatus = new EmployeeModel { EmployeeStatusId = (int)reader[0], EmployeeStatusName = (string)reader[1] };
                list.Add(EmployeeStatus);
            }
            reader.Close();
            return list.AsQueryable<EmployeeModel>();
        }

        public static IQueryable<EmployeeModel> GetEmployeeDesignation()
        {
            string sql = "SELECT Id,Designation FROM Designation;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<EmployeeModel> list = new List<EmployeeModel>();
            while (reader.Read())
            {
                EmployeeModel EmployeeDesignation = new EmployeeModel { DesignationId = (int)reader[0], Designation = (string)reader[1] };
                list.Add(EmployeeDesignation);
            }
            reader.Close();
            return list.AsQueryable<EmployeeModel>();
        }

        public static Dictionary<object, object> getEmployeeInfo(int EmployeeId)
        {
            string sql = @"SELECT EMP.FirstName,
	                               EMP.MiddleName,
	                               EMP.LastName,
	                               D.Designation
                            FROM Employee AS EMP
                            INNER JOIN Designation AS D ON D.Id = EMP.DesignationId
                            WHERE EMP.Id = @EmployeeId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = EmployeeId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> Dictionary = new Dictionary<object, object>();

            while (reader.Read())
            {
                string nameFmt = "";

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("FirstName"))
                    {
                        nameFmt = MySettings.FormatPersonName(Convert.ToString(reader["FirstName"]), Convert.ToString(reader["MiddleName"]), Convert.ToString(reader["LastName"]));
                        Dictionary.Add("EmployeeName", nameFmt);
                    }
                    else
                    {
                        Dictionary.Add(reader.GetName(i), reader.GetValue(i));
                    }
                }
                reader.Close();
                return Dictionary;
            }
            reader.Close();
            return null;
        }


        public static Dictionary<object, object> GetAllEmployee()
        {

            List<Dictionary<object, object>> EmployeeList = new List<Dictionary<object, object>>();

            string sql = @" SELECT	EM.Id,
		                            EM.NIC,
		                            EM.EmpId,
                                    EM.FirstName+' '+ISNULL(EM.MiddleName,'')+' '+ISNULL(EM.LastName,'') AS FullName,
		                            EM.Contact,
		                            EM.PermanentAddress,
		                            G.Name AS Gender,
		                            ES.Status AS EmployeeStatus
                             FROM Employee AS EM
                             INNER JOIN Gender AS G ON EM.GenderId = G.Id
                             INNER JOIN EmployeeStatus AS ES ON EM.EmployeeStatusId = ES.Id";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Employee = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Employee.Add(reader.GetName(i), reader.GetValue(i));
                }
                EmployeeList.Add(Employee);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", EmployeeList);
            return dataTable;
        }

        public static EmployeeModel GetEmployeeDetail(int id)
        {
            string sql = @"SELECT   EM.NIC,
		                            EM.EmpId,
                                    EM.FirstName,
		                            EM.MiddleName,
		                            EM.LastName,
		                            EM.Contact,
		                            EM.Email,
		                            EM.PermanentAddress,
						            EM.DesignationId,
		                            G.Name AS Gender,
		                            ES.Status AS EmployeeStatus
                                FROM Employee AS EM
                                INNER JOIN Gender AS G ON EM.GenderId = G.Id
                                INNER JOIN EmployeeStatus AS ES ON EM.EmployeeStatusId = ES.Id
					            LEFT JOIN Designation AS DE ON EM.DesignationId = DE.Id
	                            WHERE EM.Id=@id";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@id", DbType.Int32);
            param.Value = id;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            //List<StudentModel> Studentlist = new List<StudentModel>();
            EmployeeModel Employee = new EmployeeModel();
            while (reader.Read())
            {
                Employee = new EmployeeModel
                {
                    NIC = (reader == null) ? string.Empty : Convert.ToString(reader[0]),
                    EmpId = (reader == null) ? string.Empty : Convert.ToString(reader[1]),
                    FirstName = (reader == null) ? string.Empty : Convert.ToString(reader[2]),
                    MiddleName = (reader == null) ? string.Empty : Convert.ToString(reader[3]),
                    LastName = (reader == null) ? string.Empty : Convert.ToString(reader[4]),
                    Contact = (reader == null) ? string.Empty : Convert.ToString(reader[5]),
                    Email = (reader == null) ? string.Empty : Convert.ToString(reader[6]),
                    PermanentAddress = (reader == null) ? string.Empty : Convert.ToString(reader[7]),
                   // Designation = (reader == null) ? string.Empty : Convert.ToString(reader[8]),
                    DesignationId = reader[8] == DBNull.Value ? 0 : Convert.ToInt32(reader[8]), // This is how to convert DBNull from Database if integer value
                    Gender = (reader == null) ? string.Empty : Convert.ToString(reader[9]),
                    EmployeeStatusName = (reader == null) ? string.Empty : Convert.ToString(reader[10]),
                };

            }
            reader.Close();
            return Employee;
        }

        public static string GetEmployeeNIC(string newNIC)
        {
            string sql = @"SELECT NIC FROM Employee WHERE NIC =@newNIC ";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@newNIC", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(newNIC) ? (object)DBNull.Value : newNIC.ToUpper();
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            string getEmpNIC = null;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    getEmpNIC = Convert.ToString(reader.GetValue(i));
                }
            }
            reader.Close();
            return getEmpNIC;
        }

        public bool AddNewEmployee()
        {
            string sql = @"INSERT INTO [dbo].[Employee]
                               (NIC,
                                EmpId,
                                FirstName,
                                MiddleName,
                                LastName,
                                Contact,
                                Email,
                                PermanentAddress,
                                DesignationId,
                                GenderId,
                                EmployeeStatusId)
                           VALUES
                               (@NIC,
                                @EmpId,
                                @FirstName,
                                @MiddleName,
                                @LastName,
                                @Contact,
                                @Email,
                                @PermanentAddress,
                                @DesignationId,
                                @GenderId,
                                @EmployeeStatusId)";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@NIC", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(NIC) ? (object)DBNull.Value : NIC.ToUpper();
            list.Add(param);

            param = new SqlParameter("@EmpId", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(EmpId) ? (object)DBNull.Value : EmpId.ToUpper();
            list.Add(param);

            param = new SqlParameter("@FirstName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(FirstName) ? (object)DBNull.Value : FirstName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@MiddleName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(MiddleName) ? (object)DBNull.Value : MiddleName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@LastName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(LastName) ? (object)DBNull.Value : LastName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@Contact", DbType.Int32);
            param.Value = DBNull.Value;
            if (Contact != null)
            {
                param.Value = Contact;
            }
            list.Add(param);

            param = new SqlParameter("@Email", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Email) ? (object)DBNull.Value : Email;
            list.Add(param);

            param = new SqlParameter("@PermanentAddress", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(PermanentAddress) ? (object)DBNull.Value : PermanentAddress.ToUpper();
            list.Add(param);

            param = new SqlParameter("@DesignationId", DbType.Int32);
            param.Value = DesignationId;
            list.Add(param);

            param = new SqlParameter("@GenderId", DbType.Int32);
            param.Value = GenderId;
            list.Add(param);

            param = new SqlParameter("@EmployeeStatusId", DbType.Int32);
            param.Value = EmployeeStatusId;
            list.Add(param);

            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static int GetLastEmployeeId()
        {
            string sql = @"SELECT IDENT_CURRENT('Employee')";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            int EmployeeId = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    EmployeeId = Convert.ToInt32(reader.GetValue(i));
                }
            }
            reader.Close();
            return EmployeeId;
        }

        public static string GetEmployee(int empId)
        {
            string sql = @"SELECT FirstName+' '+ISNULL(MiddleName,'')+' '+ISNULL(LastName,'') AS FullName
                            FROM Employee
                            WHERE Id = @empId";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@empId", DbType.Int32);
            param.Value = empId;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            string Employee = null;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Employee = Convert.ToString(reader.GetValue(i));
                }
            }
            reader.Close();
            return Employee;
        }

        public bool AddEmployeeToACSUser(int LastEmployeeId)
        {
            string sql = @"INSERT INTO [dbo].[ACSUser]
                               (ACSUserId,
                                ACSUserType)
                            VALUES
                               (@ACSUserId,
                                @ACSUserType)";
            
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ACSUserId", DbType.Int32);
            param.Value = LastEmployeeId;
            list.Add(param);

            param = new SqlParameter("@ACSUserType", DbType.Int32);
            param.Value = 2;    //Id 2 is known as Employee
            list.Add(param);

            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static EmployeeModel GetEmployeeDetailforEdit(int id)
        {
            string sql = @"SELECT 
	                            NIC,
	                            EmpId,
	                            FirstName,
	                            MiddleName,
	                            LastName,
	                            Contact,
	                            Email,
	                            PermanentAddress,
	                            DesignationId,
	                            GenderId,
	                            EmployeeStatusId
                            FROM Employee
                            WHERE Id= @id";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@id", DbType.Int32);
            param.Value = id;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);

            //List<StudentModel> Studentlist = new List<StudentModel>();
            EmployeeModel Employee = new EmployeeModel();
            while (reader.Read())
            {
                Employee = new EmployeeModel
                {
                    NIC = (reader == null) ? string.Empty : Convert.ToString(reader[0]),
                    EmpId = (reader == null) ? string.Empty : Convert.ToString(reader[1]),
                    FirstName = (reader == null) ? string.Empty : Convert.ToString(reader[2]),
                    MiddleName = (reader == null) ? string.Empty : Convert.ToString(reader[3]),
                    LastName = (reader == null) ? string.Empty : Convert.ToString(reader[4]),
                    Contact = (reader == null) ? string.Empty : Convert.ToString(reader[5]),
                    Email = (reader == null) ? string.Empty : Convert.ToString(reader[6]),
                    PermanentAddress = (reader == null) ? string.Empty : Convert.ToString(reader[7]),
                    DesignationId = reader[8] == DBNull.Value ? 0 : Convert.ToInt32(reader[8]), // This is how to convert DBNull from Database if integer value
                    Gender = (reader == null) ? string.Empty : Convert.ToString(reader[9]),
                    EmployeeStatusName = (reader == null) ? string.Empty : Convert.ToString(reader[10]),
                };

            }
            reader.Close();
            return Employee;
        }

        public bool UpdateEmployee()
        {
            string sql = @"UPDATE [dbo].[Employee]
                               SET [NIC] = @NIC
                                  ,[EmpId] = @EmpId
                                  ,[FirstName] = @FirstName
                                  ,[MiddleName] = @MiddleName
                                  ,[LastName] = @LastName
                                  ,[Contact] = @Contact
                                  ,[Email] = @Email
                                  ,[PermanentAddress] = @PermanentAddress
                                  ,[DesignationId] = @DesignationId
                                  ,[GenderId] = @GenderId
                                  ,[EmployeeStatusId] = @EmployeeStatusId
                             WHERE Id= @id";

            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = Id;
            list.Add(param);

            param = new SqlParameter("@NIC", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(NIC) ? (object)DBNull.Value : NIC.ToUpper();
            list.Add(param);

            param = new SqlParameter("@EmpId", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(EmpId) ? (object)DBNull.Value : EmpId.ToUpper();
            list.Add(param);

            param = new SqlParameter("@FirstName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(FirstName) ? (object)DBNull.Value : FirstName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@MiddleName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(MiddleName) ? (object)DBNull.Value : MiddleName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@LastName", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(LastName) ? (object)DBNull.Value : LastName.ToUpper();
            list.Add(param);

            param = new SqlParameter("@Contact", DbType.Int32);
            param.Value = DBNull.Value;
            if (Contact != null)
            {
                param.Value = Contact;
            }
            list.Add(param);

            param = new SqlParameter("@Email", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(Email) ? (object)DBNull.Value : Email;
            list.Add(param);

            param = new SqlParameter("@PermanentAddress", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(PermanentAddress) ? (object)DBNull.Value : PermanentAddress.ToUpper();
            list.Add(param);

            param = new SqlParameter("@DesignationId", DbType.Int32);
            param.Value = DesignationId;
            list.Add(param);

            param = new SqlParameter("@GenderId", DbType.Int32);
            param.Value = DBNull.Value;
            if (Gender != null)
            {
                param.Value = Gender;
            }
            list.Add(param);

            param = new SqlParameter("@EmployeeStatusId", DbType.Int32);
            param.Value = DBNull.Value;
            if (EmployeeStatusName != null)
            {
                param.Value = EmployeeStatusName;
            }
            list.Add(param);

            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }

    }
}