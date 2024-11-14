using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;
using System.Text;

namespace MyDBAccess
{
    public class ConnectionString
    {
        public const string DEFAULT = "CONNECTION_STRING";
    }

    public class DBAccess
    {
        public const string DEFAULT_CONNECTION_STRING = "CONNECTION_STRING";

        public static SqlDataReader FetchResult(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING, CommandType ct = CommandType.Text)
        {
            SqlConnection connection = default(SqlConnection);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = new SqlCommand(sql, connection);
            command.CommandType = ct;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static SqlDataReader FetchResultSP(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();

            command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static SqlDataReader FatchResultWithoutAgument(string sql, string connStr = DEFAULT_CONNECTION_STRING, CommandType ct = CommandType.Text)
        {
            SqlConnection connection = default(SqlConnection);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = new SqlCommand(sql, connection);
          

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static List<SelectListItem> FetchResultAsList(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            return GetAsList(FetchResult(sql, pc, connStr));
        }

        public static List<SelectListItem> GetAsList(SqlDataReader reader)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            while (reader.Read())
            {
                SelectListItem sli = new SelectListItem() { Value = reader[0].ToString(), Text = reader[1].ToString() };
                list.Add(sli);
            }
            return list;
        }

        public static bool Insert(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    // log error
                    return false;
                }
                //return false;
            }

            connection.Close();
            return true;
        }

        public static int Insert2(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);
            int result = 0;

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                result = Convert.ToInt32(command.ExecuteScalar());
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    return 0;
                }
                catch (Exception ex2)
                {
                    // log error
                    return 0;
                }
                //return false;
            }
            connection.Close();
            return result;
        }

        public static int InsertSP(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);
            int result = 0;

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                result = Convert.ToInt32(command.ExecuteScalar());

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                try
                {
                    transaction.Rollback();
                    return 0;
                }
                catch (SqlException ex2)
                {
                    // log error
                    return 0;
                }
                //return false;
            }
            connection.Close();
            return result;
        }

        public static bool InsertMany(List<string> sql, List<List<SqlParameter>> list, string connStr = DEFAULT_CONNECTION_STRING)
        {
            if (sql.Count != list.Count) return false;

            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.Text;

            for (int i = 0; i < sql.Count; i++)
            {
                command.CommandText = sql[i];
                command.Parameters.Clear(); // reset all parameters
                foreach (SqlParameter p in list[i])
                {
                    command.Parameters.Add(p);
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        // log error
                        return false;
                    }
                    //return false;
                }
            }
            transaction.Commit();
            
            connection.Close();
            return true;
        }

        public static bool Update(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    // log error
                    return false;
                }
            }
            connection.Close();
            return true;
        }

        public static bool Delete(string sql, List<SqlParameter> pc, string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);
            SqlTransaction transaction = default(SqlTransaction);
            SqlCommand command = default(SqlCommand);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    // log error
                    return false;
                }
            }
            connection.Close();
            return true;
        }

        public static SqlTransaction BeginTransaction(string connStr = DEFAULT_CONNECTION_STRING)
        {
            SqlConnection connection = default(SqlConnection);

            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString);
            connection.Open();

            return connection.BeginTransaction();
        }

        public static int ExecuteSQLInTransaction(SqlTransaction transaction, string sql, List<SqlParameter> pc, CommandType cmdType = CommandType.Text)
        {
            int result = 0;
            SqlCommand command = default(SqlCommand);

            command = transaction.Connection.CreateCommand();

            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.CommandType = cmdType;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                result = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    transaction.Connection.Close();
                    return -1;
                }
                catch (Exception ex2)
                {
                    // log error
                    return -1;
                }
            }
            return result;
        }

        public static SqlDataReader ExecuteReaderInTransaction(SqlTransaction transaction, string sql, List<SqlParameter> pc, CommandType cmdType = CommandType.Text)
        {
            SqlDataReader reader = null;
            SqlCommand command = default(SqlCommand);

            command = transaction.Connection.CreateCommand();

            command.Transaction = transaction;

            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.CommandType = cmdType;
            foreach (SqlParameter p in pc)
            {
                command.Parameters.Add(p);
            }

            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    transaction.Connection.Close();
                    return null;
                }
                catch (Exception ex2)
                {
                    // log error
                    return null;
                }
            }
            return reader;
        }


        public static int ExecuteSQLInTransaction(SqlTransaction transaction, List<string> sql, List<List<SqlParameter>> list, CommandType cmdType = CommandType.Text)
        {
            SqlCommand command = default(SqlCommand);

            command = transaction.Connection.CreateCommand();
            command.Transaction = transaction;

            if (sql.Count != list.Count) return -1;

            command.CommandType = CommandType.Text;

            for (int i = 0; i < sql.Count; i++)
            {
                command.CommandText = sql[i];
                command.Parameters.Clear(); // reset all parameters
                foreach (SqlParameter p in list[i])
                {
                    command.Parameters.Add(p);
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return -1;
                    }
                    catch (Exception ex2)
                    {
                        // log error
                        return -1;
                    }
                    //return false;
                }
            }

            return 0;

        }
        public static bool CommitTransaction(SqlTransaction transaction)
        {
            try
            {
                transaction.Commit();
                //transaction.Connection.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    // log error
                    return false;
                }
            }
            return true;
        }

        public static string GetJSONString(DataTable Dt)
        {

            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";

            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();

            Sb.Append("{\"" + "aaData" + "\":[");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;

                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString().Trim() + "¾", Dt.Rows[i][j].ToString().Trim());

                }
                Sb.Append(TempStr + "},");

            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            Sb.Append("]}");
            return Sb.ToString();

        }
    }
}