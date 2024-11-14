using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MyDBAccess;
using System.Data;


namespace AYLogistics.Models.Account
{
    public class ACSUsers
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ACSUsers()
        {
            //default constructor
        }
    }

    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Employee()
        {
            //default constructor
        }

        public static IQueryable<Employee> GetEmployees()
        {
            string sql = @"SELECT 
		                    SU.Id,
                            E.NIC,
		                    E.EmpId,
		                    E.FirstName,
		                    E.MiddleName,
		                    E.LastName
		                    FROM ACSUser SU
	                    INNER JOIN Employee E ON E.Id = SU.ACSUserId AND ACSUserType=2
                        WHERE E.EmployeeStatusId =1";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<Employee> list = new List<Employee>();
            while (reader.Read())
            {
                Employee employee = new Employee
                {
                    Id = reader["Id"].ToString(),
                    Name = reader["NIC"].ToString() + "  " + reader["EmpId"].ToString() + "  " + reader["FirstName"].ToString() + " " + reader["MiddleName"].ToString() + " " + reader["LastName"].ToString()
                };
                list.Add(employee);
            }
            reader.Close();
            return list.AsQueryable<Employee>();
        }
    }
}