using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class SalesModel
    {
        public int QuotationId { get; set; }
        public AutoComplete PartyId { get; set; }
        public AutoComplete ProjectId { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime ValidThrough { get; set; }
        public int DiscountRate { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public List<SalesCategory> SalesCategoryItems { get; set; }
        public Party PartyModel { get; set; }
        public Project ProjectModel { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }
        public decimal NetTotal { get; set; }
        public int CurrencyType { get; set; }
        //Constructor
        public SalesModel()
        {
            SalesCategoryItems = new List<SalesCategory>();
            AutoComplete PartyId = new AutoComplete();
            AutoComplete ProjectId = new AutoComplete();
            PartyModel = new Party();
            ProjectModel = new Project();
        }
        public SalesModel(int Id)
        {
            this.QuotationId = Id;
            PartyModel = new Party();
            ProjectModel = new Project();
        }
       /* public static string FormatQuotationNumber()
        {
            string FormatQuotationNumber = GetQuotationNumberFormat();
            return FormatQuotationNumber.Replace("%S%", GetQuotationSequence().ToString()).Replace("%YYYY%", Convert.ToString(DateTime.Today.Year));
        }*/

        /*public static string GetQuotationNumberFormat()
        {
            string sql = @"SELECT [Format] FROM  Numbering WHERE Id=5";
            string format = null;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                format = (string)reader[0];
            }
            reader.Close();
            return format;
        }*/

       /* public static int GetQuotationSequence()
        {
            string sql = @"SELECT [Sequence] FROM  Numbering WHERE Id=5";
            int sequence = 0;
            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                sequence = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            return sequence;
        }*/

        public bool SaveQuotation()
        {
            string QuotationNumber = MySettings.FormatNumbering(5);// FormatQuotationNumber();
            String sql = @"INSERT INTO [dbo].[Quotation]
                                       ([PartyId]
                                       ,[ProjectId]
                                       ,[QuotationNumber]
                                       ,[DateCreated]
                                       ,[CreatedBy]
                                       ,[ValidThrough]
                                       ,[Type]
,[Mode]
                                       ,[CurrencyId])
                                 VALUES
                                       (@PartyId
                                       ,@ProjectId
                                       ,@QuotationNumber
                                       ,GETDATE()
                                       ,@EmployeeId
                                       ,@ValidThrough
                                       ,@Type
 ,@Mode
                                       ,@CurrencyId)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@PartyId", DbType.Int32);
            param.Value = DBNull.Value;
            if (PartyId != null)
            {
                param.Value = PartyId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ProjectId", DbType.Int32);
            param.Value = DBNull.Value;
            if (ProjectId != null)
            {
                param.Value = ProjectId.id;
            }
            List.Add(param);
            param = new SqlParameter("@QuotationNumber", DbType.String);
            param.Value = QuotationNumber;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@ValidThrough", DbType.DateTime);
            DateTime today = DateTime.Now;
            DateTime ValidThrough = today.AddDays(30);
            param.Value = ValidThrough;
            List.Add(param);
            param = new SqlParameter("@Type", DbType.String);
            param.Value = Type;
            List.Add(param);
            param = new SqlParameter("@Mode", DbType.String);
            param.Value = Mode;
            List.Add(param);
            param = new SqlParameter("@CurrencyId", DbType.Int32);
            param.Value = CurrencyType;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool UpdateQuotation()
        {
            string QuotationNumber = MySettings.FormatNumbering(5);// FormatQuotationNumber();
            String sql = @"UPDATE [dbo].[Quotation]
                           SET [PartyId] = @PartyId
                              ,[ProjectId] = @ProjectId
                              ,[ValidThrough] = @ValidThrough
                              ,[Type] = @Type
,[Mode] = @Mode
                              ,[CurrencyId] = @CurrencyId
                         WHERE Id = @Id";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@PartyId", DbType.Int32);
            param.Value = DBNull.Value;
            if (PartyId != null)
            {
                param.Value = PartyId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ProjectId", DbType.Int32);
            param.Value = DBNull.Value;
            if (ProjectId != null)
            {
                param.Value = ProjectId.id;
            }
            List.Add(param);
            param = new SqlParameter("@ValidThrough", DbType.DateTime);
            DateTime today = DateTime.Now;
            DateTime ValidThrough = today.AddDays(30);
            param.Value = ValidThrough;
            List.Add(param);
            param = new SqlParameter("@Type", DbType.String);
            param.Value = Type;
            List.Add(param);
            param = new SqlParameter("@Mode", DbType.String);
            param.Value = Mode;
            List.Add(param);
            param = new SqlParameter("@Id", DbType.Int32);
            param.Value = QuotationId;
            List.Add(param);
            param = new SqlParameter("@CurrencyId", DbType.Int32);
            param.Value = CurrencyType;
            List.Add(param);

            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

        public bool SaveQuotationLog(int LastQuotationId)
        {
            String sql = @"INSERT INTO [dbo].[QuotationLog]
                                   ([QuotationId]
                                   ,[DateUpdate]
                                   ,[EmployeeId]
                                   ,[SubTotal]
                                   ,[GST]
                                   ,[NetTotal])
                             VALUES
                                   (@QuotationId
                                   ,GETDATE()
                                   ,@EmployeeId
                                   ,@SubTotal
                                   ,@GST
                                   ,@NetTotal)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@QuotationId", DbType.String);
            param.Value = LastQuotationId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            param = new SqlParameter("@SubTotal", DbType.Decimal);
            param.Value = SubTotal;
            List.Add(param);
            param = new SqlParameter("@GST", DbType.Decimal);
            param.Value = GST;
            List.Add(param);
            param = new SqlParameter("@NetTotal", DbType.Decimal);
            param.Value = NetTotal;
            List.Add(param);

            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static int GetLastQuotationId()
        {
            string sql = @"SELECT IDENT_CURRENT('Quotation')";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            int QuotationId = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    QuotationId = Convert.ToInt32(reader.GetValue(i));
                }
            }
            reader.Close();
            return QuotationId;
        }

      /*  public bool UpdateSequence()
        {
            String sql = @"UPDATE [dbo].[Numbering]
                                SET     [Sequence] = @Sequence
                                WHERE Id= 5";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Sequence", DbType.Int32);
            param.Value = 1 + GetQuotationSequence();
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }*/

        public bool SaveSalesSelectedItems(List<SalesCategory> SalesCategoryItems, int LastQuotationId)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;

            for (int j = 0; j < SalesCategoryItems.Count; j++) // Sales Category Saving
            {
                sql = @"INSERT INTO [dbo].[SelectedSalesCategory]
                                   ([QuotationId]
                                   ,[SalesCategoryId]
                                   ,[DateAdded]
                                   ,[AddedBy])
                             VALUES
                                   (@LastQuotationId
                                   ,@SalesCategoryId
                                   ,GETDATE()
                                   ,@EmployeeId)";

                List = new List<SqlParameter>();
                param = new SqlParameter("@LastQuotationId", DbType.Int32);
                param.Value = LastQuotationId;
                List.Add(param);

                param = new SqlParameter("@SalesCategoryId", DbType.Int32);
                param.Value = DBNull.Value;
               if (SalesCategoryItems[j].Id != null)
                {
                    param.Value = SalesCategoryItems[j].Id.id;
                }
                /* if (SalesCategoryItems[j].IdDD > 0)
                {
                    param.Value = SalesCategoryItems[j].IdDD;
                }*/
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);

                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // Sales item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < SalesCategoryItems[j].SalesItems.Count; k++)
                    {
                        sql = @"INSERT INTO [dbo].[SelectedSalesItem]
                                           ([SelectedSalesCategoryId]
                                           ,[SalesItemId]
                                           ,[DateAdded]
                                           ,[Quantity]
                                           ,[AddedBy]
,[UnitPrice])
                                     VALUES
                                           ((SELECT IDENT_CURRENT('SelectedSalesCategory'))
                                           ,@SalesItemId
                                           ,GETDATE()
                                           ,@Quantity
                                           ,@EmployeeId
,@UnitPrice)";
                        sqlList.Add(sql);
                    }
                    for (int i = 0; i < SalesCategoryItems[j].SalesItems.Count; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@SalesItemId", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (SalesCategoryItems[j].SalesItems[i].ItemId != null)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].ItemId.id;
                        }
                       /* if (SalesCategoryItems[j].SalesItems[i].ItemIdDD > 0)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].ItemIdDD;
                        }*/
                        List.Add(param);
                        param = new SqlParameter("@Quantity", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (SalesCategoryItems[j].SalesItems[i].Quantity > 0)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].Quantity;
                        }
                        List.Add(param);
                        param = new SqlParameter("@UnitPrice", DbType.Decimal);
                        param.Value = SalesCategoryItems[j].SalesItems[i].UnitPrice;
                        List.Add(param);
                        param = new SqlParameter("@EmployeeId", DbType.Int32);
                        param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
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

        public bool UpdateSalesSelectedItems(List<SalesCategory> SalesCategoryItems, int QuotationId)
        {
            List<SqlParameter> List;
            SqlParameter param;

            string sql;

            for (int j = 0; j < SalesCategoryItems.Count; j++) // Sales Category Saving
            {
                sql = @"
                    IF NOT EXISTS(select Id from SelectedSalesCategory where Id = @CatPrimaryId)
                        BEGIN
                            INSERT INTO [dbo].[SelectedSalesCategory]
                                   ([QuotationId]
                                   ,[SalesCategoryId]
                                   ,[DateAdded]
                                   ,[AddedBy])
                             VALUES
                                   (@QuotationId
                                   ,@SalesCategoryId
                                   ,GETDATE()
                                   ,@EmployeeId)
                        END";

                List = new List<SqlParameter>();
                param = new SqlParameter("@QuotationId", DbType.Int32);
                param.Value = QuotationId;
                List.Add(param);

                param = new SqlParameter("@CatPrimaryId", DbType.Int32);
                param.Value = SalesCategoryItems[j].SelectedCATid;
                List.Add(param);

                param = new SqlParameter("@SalesCategoryId", DbType.Int32);
                param.Value = DBNull.Value;
                if (SalesCategoryItems[j].Id != null)
                {
                    param.Value = SalesCategoryItems[j].Id.id;
                }
                /*if (SalesCategoryItems[j].IdDD > 0)
                {
                    param.Value = SalesCategoryItems[j].IdDD;
                }*/
                List.Add(param);
                param = new SqlParameter("@EmployeeId", DbType.Int32);
                param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
                List.Add(param);

                if (DBAccess.Insert(sql, List, ConnectionString.DEFAULT)) // Sales item saving start
                {
                    List<string> sqlList;
                    List<List<SqlParameter>> paramlist;
                    sqlList = new List<string>();
                    paramlist = new List<List<SqlParameter>>();
                    for (int k = 0; k < SalesCategoryItems[j].SalesItems.Count; k++)
                    {
                        sql = @"
                               IF EXISTS(select Id from SelectedSalesItem where Id = @ItemPrimaryId)
                                BEGIN
                                    UPDATE SelectedSalesItem
                                    SET Quantity = @Quantity
                                    WHERE Id = @ItemPrimaryId
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO [dbo].[SelectedSalesItem]
                                           ([SelectedSalesCategoryId]
                                           ,[SalesItemId]
                                           ,[DateAdded]
                                           ,[Quantity]
                                           ,[AddedBy]
,[UnitPrice])
                                     VALUES
                                           (--%selectedCatId%
                                           ,@SalesItemId
                                           ,GETDATE()
                                           ,@Quantity
                                           ,@EmployeeId
,@UnitPrice)
                                END";
                        if (SalesCategoryItems[j].EnableStatus == false)
                        {
                            sql = sql.Replace("--%selectedCatId%", "@CatPrimaryId");
                        }
                        else
                        {
                            sql = sql.Replace("--%selectedCatId%", "(SELECT IDENT_CURRENT('SelectedSalesCategory'))");
                        }
                        sqlList.Add(sql);
                    }
                    for (int i = 0; i < SalesCategoryItems[j].SalesItems.Count; i++)
                    {
                        List = new List<SqlParameter>();

                        param = new SqlParameter("@ItemPrimaryId", DbType.Int32);
                        param.Value = SalesCategoryItems[j].SalesItems[i].selectedITEMid;
                        List.Add(param);

                        if (SalesCategoryItems[j].EnableStatus == false)
                        {
                            param = new SqlParameter("@CatPrimaryId", DbType.Int32);
                            param.Value = SalesCategoryItems[j].SelectedCATid;
                            List.Add(param);
                        }
                        param = new SqlParameter("@SalesItemId", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (SalesCategoryItems[j].SalesItems[i].ItemId != null)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].ItemId.id;
                        }
                        if (SalesCategoryItems[j].SalesItems[i].ItemIdDD > 0)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].ItemIdDD;
                        }
                        List.Add(param);
                        param = new SqlParameter("@Quantity", DbType.Int32);
                        param.Value = DBNull.Value;
                        if (SalesCategoryItems[j].SalesItems[i].Quantity > 0)
                        {
                            param.Value = SalesCategoryItems[j].SalesItems[i].Quantity;
                        }
                        List.Add(param);
                        param = new SqlParameter("@UnitPrice", DbType.Decimal);
                        param.Value = SalesCategoryItems[j].SalesItems[i].UnitPrice;
                        List.Add(param);
                        param = new SqlParameter("@EmployeeId", DbType.Int32);
                        param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
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

        public static Dictionary<Object, Object> getLastQuotationInfo(int LastQuotationId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"select Id, QuotationNumber from Quotation where Id = @LastQuotationId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@LastQuotationId", DbType.Int32);
            param.Value = LastQuotationId;
            list.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("QuotationNumber", reader["QuotationNumber"].ToString());
                dictionary.Add("Id", reader["Id"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<object, object> QuotationFilterByDate(DateTime StartDate, DateTime EndDate)
        {

            List<Dictionary<object, object>> QTList = new List<Dictionary<object, object>>();

            string sql = @"SELECT QT.Id,
	                           QT.QuotationNumber,
	                           PT.Name,
	                           Convert(nvarchar(10),QT.DateCreated,103) AS DateCreated,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee
                        FROM Quotation AS QT
                        INNER JOIN Party AS PT ON PT.Id = QT.PartyId
INNER JOIN Employee AS EMP ON EMP.Id = QT.CreatedBy
                        WHERE QT.DateCreated Between @StartDate AND @EndDate";

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
                Dictionary<object, object> QT = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    QT.Add(reader.GetName(i), reader.GetValue(i));
                }
                QTList.Add(QT);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", QTList);
            return dataTable;
        }

        public static Dictionary<object, object> QuotationFilterByQuery(string query)
        {

            List<Dictionary<object, object>> QTList = new List<Dictionary<object, object>>();

            string sql = @"SELECT QT.Id,
	                           QT.QuotationNumber,
	                           PT.Name,
	                           Convert(nvarchar(10),QT.DateCreated,103) AS DateCreated,
CONCAT(ISNULL(EMP.MiddleName,EMP.FirstName),' ',EMP.EMPId) AS Employee
                            FROM Quotation AS QT
                            INNER JOIN Party AS PT ON PT.Id = QT.PartyId
INNER JOIN Employee AS EMP ON EMP.Id = QT.CreatedBy
                            WHERE 
                                    (
                                    QT.QuotationNumber like concat('%',@query,'%')
                                    OR
									PT.Name like concat('%',@query,'%')
									)";

            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            spList.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> QT = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    QT.Add(reader.GetName(i), reader.GetValue(i));
                }
                QTList.Add(QT);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", QTList);
            return dataTable;
        }

        public static Dictionary<Object, Object> GetQT(int QTId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT QT.PartyId,
                                PT.Name AS PartyName,
                                QT.ProjectId,
                                PRJ.Name AS ProjectName,
                                QuotationNumber,
                                QT.[Type],
QT.[Mode],
								QT.Id,
								QT.CurrencyId  
                            FROM Quotation AS QT
                            LEFT JOIN Party AS PT ON PT.Id = QT.PartyId
                            LEFT JOIN Project AS PRJ ON PRJ.Id = QT.ProjectId
                            WHERE QT.Id = @QTId";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@QTId", DbType.Int32);
            param.Value = QTId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("PartyId", reader["PartyId"].ToString());
                dictionary.Add("PartyName", reader["PartyName"].ToString());
                dictionary.Add("ProjectId", reader["ProjectId"].ToString());
                dictionary.Add("ProjectName", reader["ProjectName"].ToString());
                dictionary.Add("QuotationNumber", reader["QuotationNumber"].ToString());
                string Type = reader["Type"].ToString();
                if (Type == "SEA")
                {
                    dictionary.Add("Type", 1);
                }
                if (Type == "AIR")
                {
                    dictionary.Add("Type", 2);
                }
                string Mode = reader["Mode"].ToString();
                if (Mode == "IMPORT")
                {
                    dictionary.Add("Mode", 1);
                }
                if (Mode == "EXPORT")
                {
                    dictionary.Add("Mode", 2);
                }
                dictionary.Add("QuotationId", reader["Id"].ToString());
                dictionary.Add("CurrencyId", reader["CurrencyId"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static List<Dictionary<object, object>> GetCAT(int QuotId)
        {
            List<Dictionary<object, object>> SalesCATList = new List<Dictionary<object, object>>();

            string sql = @"SELECT SSCAT.Id, SSCAT.SalesCategoryId, SCAT.Name AS SalesCATName 
                            FROM SelectedSalesCategory AS SSCAT
                            INNER JOIN SalesCategory AS SCAT ON SCAT.Id = SSCAT.SalesCategoryId
                            WHERE SSCAT.QuotationId = @QuotId AND SSCAT.DateRemoved IS NULL";
            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@QuotId", DbType.Int32);
            param.Value = QuotId;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> CAT = new Dictionary<object, object>();
                CAT.Add("Id", (reader["Id"].ToString()));
                CAT.Add("SalesCategoryId", (reader["SalesCategoryId"].ToString()));
                CAT.Add("SalesCATName", (reader["SalesCATName"].ToString()));
                SalesCATList.Add(CAT);
            }
            reader.Close();
            return SalesCATList;
        }

        public static List<Dictionary<object, object>> GetSITEMS(int SCATid)
        {
            List<Dictionary<object, object>> SalesItemList = new List<Dictionary<object, object>>();

            string sql = @"SELECT SSI.SalesItemId,
		                            SI.Name AS SalesItemName,
		                            U.Name AS UnitName,
		                            SSI.Quantity,
		                            --SI.UnitPrice,
SSI.UnitPrice,
                                    Curr.Name AS CurrencyName,
									TAX.Rate,
									SSI.Id
                            FROM SelectedSalesItem AS SSI
                            INNER JOIN SalesItem AS SI ON SI.Id = SSI.SalesItemId
                            INNER JOIN Unit AS U ON U.Id = SI.UnitId
                            INNER JOIN Currency AS Curr ON Curr.Id = SI.CurrencyId
							INNER JOIN TAX AS TAX ON TAX.Id = SI.GST
                            WHERE SSI.SelectedSalesCategoryId = @SCATid AND SSI.DateRemoved IS NULL";
            List<SqlParameter> spList = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SCATid", DbType.Int32);
            param.Value = SCATid;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Item = new Dictionary<object, object>();
                Item.Add("SalesItemId", (reader["SalesItemId"].ToString()));
                Item.Add("SalesItemName", (reader["SalesItemName"].ToString()));
                Item.Add("UnitName", (reader["UnitName"].ToString()));
                Item.Add("Quantity", (reader["Quantity"].ToString()));
                Item.Add("UnitPrice", (reader["UnitPrice"].ToString()));
                Item.Add("CurrencyName", (reader["CurrencyName"].ToString()));
                Item.Add("IsGST", (reader["Rate"].ToString()));
                Item.Add("selectedITEMid", (reader["Id"].ToString()));
                SalesItemList.Add(Item);
            }
            reader.Close();
            return SalesItemList;
        }

        public static bool UpdateUnitPrice(decimal UnitPrice, int ItemId)
        {
            String sql = @"UPDATE SalesItem
                            SET UnitPrice = @UnitPrice,
	                            UpdatedBy = @UpdatedBy,
	                            UpdatedDate = GETDATE()
                            Where Id = @ItemId";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ItemId", DbType.Int32);
            param.Value = ItemId;
            List.Add(param);
            param = new SqlParameter("@UnitPrice", DbType.Decimal);
            param.Value = UnitPrice;
            List.Add(param);
            param = new SqlParameter("@UpdatedBy", DbType.String);
            param.Value = HttpContext.Current.User.Identity.Name;
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static Dictionary<Object, Object> GetQuotationBasicInfo(int QuotationId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"SELECT QT.QuotationNumber,
		                            PT.Name AS CustomerName,
		                            CONCAT(Addr.Address1,', ',addr.Island,', ', NT.Name) AS CustomerAddress,
		                            CONCAT('Tel:',CT.Tel,', Fax:',CT.Fax,', Mobile:',CT.Mobile) AS CustomerContact,
		                            PRJ.Name AS ProjectName,
		                            Convert(nvarchar(10),QT.ValidThrough,103) AS ValidThrough,
									QT.[Type],
QT.[Mode],
									QT.CurrencyId
                            FROM Quotation AS QT
                            INNER JOIN Party AS PT ON PT.Id = QT.PartyId
                            LEFT JOIN Project AS PRJ ON PRJ.Id = QT.ProjectId
                            LEFT JOIN [Address] AS Addr ON Addr.Id = PT.AddressId
                            LEFT JOIN Nationality AS NT ON NT.Id = Addr.NationalityId
                            LEFT JOIN Contact AS CT ON CT.Id = PT.ContactId
                            WHERE QT.Id = @QuotationId";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@QuotationId", DbType.Int32);
            param.Value = QuotationId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("QuotationNumber", reader["QuotationNumber"].ToString());
                dictionary.Add("CustomerName", reader["CustomerName"].ToString());
                dictionary.Add("CustomerAddress", reader["CustomerAddress"].ToString());
                dictionary.Add("CustomerContact", reader["CustomerContact"].ToString());
                dictionary.Add("ProjectName", reader["ProjectName"].ToString());
                dictionary.Add("ValidThrough", reader["ValidThrough"].ToString());
                dictionary.Add("Type", reader["Type"].ToString());
                dictionary.Add("Mode", reader["Mode"].ToString());
                dictionary.Add("CurrencyId", reader["CurrencyId"].ToString());
            }
            reader.Close();
            return dictionary;
        }

        public static Dictionary<Object, Object> GetTotalCalculation(int QuotationId)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();

            string sql = @"select TOP 1 SubTotal,GST,NetTotal from QuotationLog WHERE QuotationId = @QuotationId order by DateUpdate DESC";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@QuotationId", DbType.Int32);
            param.Value = QuotationId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("SubTotal", reader["SubTotal"].ToString());
                dictionary.Add("GST", reader["GST"].ToString());
                dictionary.Add("NetTotal", reader["NetTotal"].ToString());
            }
            reader.Close();
            return dictionary;
        }
    }

    public class SalesCategory
    {
        public AutoComplete Id { get; set; }
        public int IdDD { get; set; }
        [Required]
        [Display(Name = "Category Name")]   
        public string SalesCatName { get; set; }
        public int SalesCatId { get; set; }
        public int SelectedCATid { get; set; }
        public bool EnableStatus { get; set; }
        public List<SalesItems> SalesItems { get; set; }

        public SalesCategory()
        {
            SalesItems = new List<SalesItems>();
            AutoComplete Id = new AutoComplete();
        }


        public bool AddCategory()
        {
            string sql = @"INSERT INTO [dbo].[SalesCategory]
                                       ([Name])
                                 VALUES
                                       (@Name)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = SalesCatName.ToUpper();
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetCategories()
        {

            List<Dictionary<object, object>> CategoryList = new List<Dictionary<object, object>>();

            string sql = @"select Id,Name,INACTIVE from SalesCategory where INACTIVE = 0";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Category = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Category.Add(reader.GetName(i), reader.GetValue(i));
                }
                CategoryList.Add(Category);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", CategoryList);
            return dataTable;
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null)
        {
            string sql = @"SELECT Id,Name FROM 
                                    SalesCategory
                                    WHERE 
                                    (
                                    Name like concat('%',@query,'%') AND INACTIVE = 0
                                    ) ";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> Categorylist = new List<Dictionary<object, object>>();

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
                Categorylist.Add(dictionary);
            }
            reader.Close();
            return Categorylist;
        }

        public static Dictionary<String, String> GetCategoriesKO()
        {
            String sql = @"SELECT Id,Name FROM SalesCategory WHERE INACTIVE = 0";
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

        public static Dictionary<String, String> GetCategoriesKO_ALL()
        {
            String sql = @"SELECT Id,Name FROM SalesCategory";
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

        public static bool UptadeCatStatus(int CATid, int StatusId)
        {
            string sql = @"UPDATE SalesCategory
                        SET INACTIVE = @StatusId
                        WHERE Id = @Id

                        UPDATE SalesItem
                        SET INACTIVE = @StatusId
                        WHERE SalesCategoryId = @Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = CATid;
            list.Add(param);
            param = new SqlParameter("@StatusId", DbType.Int32);
            param.Value = StatusId;
            list.Add(param);
            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }

        public static bool UptadeItemStatus(int ITEMid, int StatusId)
        {
            string sql = @"UPDATE SalesItem
                        SET INACTIVE = @StatusId
                        WHERE Id = @Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.Int32);
            param.Value = ITEMid;
            list.Add(param);
            param = new SqlParameter("@StatusId", DbType.Int32);
            param.Value = StatusId;
            list.Add(param);
            return DBAccess.Update(sql, list, ConnectionString.DEFAULT);
        }

        public static IQueryable<SalesCategory> GetCategoryDropD()
        {
            string sql = "SELECT Id,Name FROM SalesCategory WHERE INACTIVE = 0";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SalesCategory> list = new List<SalesCategory>();
            while (reader.Read())
            {
                SalesCategory SCAT = new SalesCategory { SalesCatId = (int)reader[0], SalesCatName = (string)reader[1] };
                list.Add(SCAT);
            }
            reader.Close();
            return list.AsQueryable<SalesCategory>();
        }

        public static bool DeleteCAT(int SelectedCATid)
        {
            String sql = @"UPDATE SelectedSalesCategory
                            SET DateRemoved = GETDATE()
		                       ,RemovedBy = @EmployeeId
                          WHERE Id = @SelectedCATid";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SelectedCATid", DbType.Int32);
            param.Value = SelectedCATid;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }
    }

    public class SalesUnit
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Unit Name")]
        public string UnitName {get; set;}
        [Display(Name="Description")]
        public string UnitDescription { get; set; }

        public bool AddUnit()
        {
            string sql = @"INSERT INTO [dbo].[Unit]
                                       ([Name]
                                       ,[Description])
                                 VALUES
                                       (@Name
                                       ,@Description)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = UnitName;
            list.Add(param);
            param = new SqlParameter("@Description", DbType.String);
            param.Value = UnitDescription;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetUnits()
        {

            List<Dictionary<object, object>> UnitList = new List<Dictionary<object, object>>();

            string sql = @"select Id,Name,Description from Unit";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Unit = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Unit.Add(reader.GetName(i), reader.GetValue(i));
                }
                UnitList.Add(Unit);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", UnitList);
            return dataTable;
        }

        public static IQueryable<SalesUnit> GetUnitList()
        {
            string sql = "SELECT Id,Name FROM Unit;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SalesUnit> list = new List<SalesUnit>();
            while (reader.Read())
            {
                SalesUnit Unit = new SalesUnit { Id = (int)reader[0], UnitName = (string)reader[1] };
                list.Add(Unit);
            }
            reader.Close();
            return list.AsQueryable<SalesUnit>();
        }
    }

    public class SalesDiscount
    {
        public int Id { get; set; }
        [Required]
        public decimal Rate { get; set; }

        public bool AddDiscount()
        {
            string sql = @"INSERT INTO [dbo].[DiscountRate]
                                       ([Rate])
                                 VALUES
                                       (@Rate)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Rate", DbType.Decimal);
            param.Value = Rate;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetDiscounts()
        {

            List<Dictionary<object, object>> DiscountList = new List<Dictionary<object, object>>();

            string sql = @"select Id,Rate from DiscountRate";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Discount = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Discount.Add(reader.GetName(i), reader.GetValue(i));
                }
                DiscountList.Add(Discount);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", DiscountList);
            return dataTable;
        }
    }

    public class SalesCurrency
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Option { get; set; }
        public decimal ExchangeRate { get; set; }

        //constructor
        public SalesCurrency()
        {
        }
        public SalesCurrency(int Id)
        {
            this.Id = Id;
            GetSalesCurrencyById(Id);
        }

        public bool AddCurrency()
        {
            string sql = @"INSERT INTO [dbo].[Currency]
                           ([Name]
                            ,[ExchangeRate])
                     VALUES
                           (@Name
                            ,@ExchangeRate)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = Currency;
            list.Add(param);
            param = new SqlParameter("@ExchangeRate", DbType.Decimal);
            param.Value = ExchangeRate;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetCurrencies()
        {

            List<Dictionary<object, object>> CurrencyList = new List<Dictionary<object, object>>();

            string sql = @"select Id,Name, ExchangeRate from Currency";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Currency = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Currency.Add(reader.GetName(i), reader.GetValue(i));
                }
                CurrencyList.Add(Currency);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", CurrencyList);
            return dataTable;
        }

        public static IQueryable<SalesCurrency> GetCurrencyList()
        {
            string sql = "SELECT Id,Name FROM Currency;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SalesCurrency> list = new List<SalesCurrency>();
            while (reader.Read())
            {
                SalesCurrency Currency = new SalesCurrency { Id = (int)reader[0], Currency = (string)reader[1] };
                list.Add(Currency);
            }
            reader.Close();
            return list.AsQueryable<SalesCurrency>();
        }

        public static IQueryable<SalesCurrency> GSTOption()
        {
            string sql = "SELECT Id,Name FROM TAX Order by Id";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<SalesCurrency> list = new List<SalesCurrency>();
            while (reader.Read())
            {
                SalesCurrency Option = new SalesCurrency { Id = (int)reader[0], Option = (string)reader[1] };
                list.Add(Option);
            }
            reader.Close();
            return list.AsQueryable<SalesCurrency>();
        }

        public void GetSalesCurrencyById(int Id)
        {
            string sql = @"select Id,Name, ExchangeRate from Currency where Id = @Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", System.Data.SqlDbType.Int);
            param.Value = Id;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            string ER = "";
            while (reader.Read())
            {
                Id = (Convert.ToInt32(reader["Id"]));
                Currency = (reader["Name"].ToString());
                ER = reader["ExchangeRate"].ToString();
                if (ER != "")
                {
                    ExchangeRate = (Convert.ToDecimal(reader["ExchangeRate"]));
                }
            }
            reader.Close();
        }

        public bool ModifyCurrency()
        {
            string sql = @"UPDATE Currency
                    SET Name =@Name
	                    ,ExchangeRate = @ExchangeRate
	                    Where Id = @Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", DbType.String);
            param.Value = Id;
            list.Add(param);
            param = new SqlParameter("@Name", DbType.Int32);
            param.Value = Currency;
            list.Add(param);
            param = new SqlParameter("@ExchangeRate", DbType.Decimal);
            param.Value = ExchangeRate;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<String, String> GetKOCurrencies()
        {
            String sql = @"select Id, CONCAT(Name,' ',ExchangeRate) Name from Currency";
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

        public static Dictionary<Object, Object> getExchangeRate()
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            string sql = @"select ExchangeRate  from Currency WHERE Id = 2";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            if (reader.Read())
            {
                dictionary.Add("ExchangeRate", reader["ExchangeRate"].ToString());
            }
            reader.Close();
            return dictionary;
        }
    }

    public class SalesItems
    {
        public int Id { get; set; }
        public AutoComplete ItemId { get; set; }
        public int ItemIdDD { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemTotal { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int SalesCAT { get; set; }
        [Required]
        [Display(Name = "GST")]
        public int GST { get; set; }
        public int selectedITEMid { get; set; }
        //constructor
        public SalesItems()
        {
        }
        public SalesItems(int Id)
        {
            this.Id = Id;
            GetSalesItemById(Id);
        }

        public bool AddItems()
        {
            string sql = @"INSERT INTO [dbo].[SalesItem]
                                       ([Name]
                                       ,[UnitId]
                                       ,[UnitPrice]
                                       ,[DateCreated]
                                       ,[CreatedBy]
                                       ,[CurrencyId]
                                       ,[SalesCategoryId]
                                       ,[GST])
                                 VALUES
                                       (@Name
                                       ,@UnitId
                                       ,@UnitPrice
                                       ,GETDATE()
                                       ,@EmployeeId
                                       ,@CurrencyId
                                       ,@SalesCategoryId
                                       ,@GST)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Name", DbType.String);
            param.Value = ItemName;
            list.Add(param);
            param = new SqlParameter("@UnitId", DbType.Int32);
            param.Value = UnitId;
            list.Add(param);
            param = new SqlParameter("@UnitPrice", DbType.Decimal);
            param.Value = UnitPrice;
            list.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            list.Add(param);
            param = new SqlParameter("@CurrencyId", DbType.Decimal);
            param.Value = CurrencyId;
            list.Add(param);
            param = new SqlParameter("@SalesCategoryId", DbType.Int32);
            param.Value = SalesCAT;
            list.Add(param);
            param = new SqlParameter("@GST", DbType.Int32);
            param.Value = GST;
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static Dictionary<object, object> GetSalesItems()
        {

            List<Dictionary<object, object>> ItemList = new List<Dictionary<object, object>>();

            string sql = @"SELECT SI.[Id]
                                  ,SI.[Name] ItemName
                                  ,U.Name AS Unit
                                  ,SI.[UnitPrice]
                                  ,C.Name AS CurrencyName
,SI.[INACTIVE]
                              FROM [dbo].[SalesItem] AS SI
                              INNER JOIN Unit AS U ON U.Id = SI.UnitId
                              INNER JOIN Currency AS C ON C.Id = SI.CurrencyId
WHERE SI.[INACTIVE] = 0";

            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                Dictionary<object, object> Item = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Item.Add(reader.GetName(i), reader.GetValue(i));
                }
                ItemList.Add(Item);
            }
            reader.Close();

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", ItemList);
            return dataTable;
        }

        public void GetSalesItemById(int Id)
        {
            string sql = @"select Name, UnitId,UnitPrice,CurrencyId, SalesCategoryId,GST from SalesItem where Id = @Id";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id", System.Data.SqlDbType.Int);
            param.Value = Id;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                ItemName = (reader["Name"].ToString());
                UnitId = (Convert.ToInt32(reader["UnitId"]));
                UnitPrice = (Convert.ToDecimal(reader["UnitPrice"]));
                CurrencyId = (Convert.ToInt32(reader["CurrencyId"]));
                SalesCAT = (Convert.ToInt32(reader["SalesCategoryId"]));
                GST = (Convert.ToInt32(reader["GST"]));
            }
            reader.Close();
        }

        public bool ModifyItems()
        {
            string sql = @"UPDATE [dbo].[SalesItem]
                               SET [Name] = @Name
                                  ,[UnitId] = @UnitId
                                  ,[UnitPrice] = @UnitPrice
                                  ,[CurrencyId] = @CurrencyId
                                  ,[SalesCategoryId] = @SalesCategoryId
                                  ,[GST] = @GST
                             WHERE Id = @SalesItemId


                            INSERT INTO [dbo].[SalesItemLog]
                                       ([SalesItemId]
                                       ,[DateUpdate]
                                       ,[EmployeeId])
                                 VALUES
                                       (@SalesItemId
                                       ,GETDATE()
                                       ,@EmployeeId)";
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@SalesItemId", DbType.String);
            param.Value = Id;
            list.Add(param);
            param = new SqlParameter("@Name", DbType.String);
            param.Value = ItemName;
            list.Add(param);
            param = new SqlParameter("@UnitId", DbType.Int32);
            param.Value = UnitId;
            list.Add(param);
            param = new SqlParameter("@UnitPrice", DbType.Decimal);
            param.Value = UnitPrice;
            list.Add(param);
            param = new SqlParameter("@CurrencyId", DbType.Int32);
            param.Value = CurrencyId;
            list.Add(param);
            param = new SqlParameter("@SalesCategoryId", DbType.Int32);
            param.Value = SalesCAT;
            list.Add(param);
            param = new SqlParameter("@GST", DbType.Int32);
            param.Value = GST;
            list.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public static List<Dictionary<object, object>> Search(string query, int? typeId = null, int? subtype = null, int? SCATid = null)
        {
           /* string sql = @"SELECT Id,Name FROM 
                                    SalesItem
                                    WHERE 
                                    (
                                    Name like concat('%',@query,'%') AND SalesCategoryId = @SCATid
                                    ) ";*/
            string sql = @"SELECT SI.Id,SI.Name FROM 
                                    SalesItem AS SI
									INNER JOIN SalesCategory AS SC ON SC.Id = SI.SalesCategoryId AND SC.INACTIVE = 0
                                    WHERE 
                                    (
                                    SI.Name like concat('%',@query,'%') AND SI.SalesCategoryId = @SCATid AND SI.INACTIVE = 0
									OR SC.Name like concat('%',@query,'%') AND SI.SalesCategoryId = @SCATid AND SI.INACTIVE = 0
                                    )";

            List<SqlParameter> list = new List<SqlParameter>();
            List<Dictionary<object, object>> SalesItemlist = new List<Dictionary<object, object>>();

            SqlParameter param;

            param = new SqlParameter("@query", DbType.String);
            param.Value = query;
            list.Add(param);
            param = new SqlParameter("@SCATid", DbType.String);
            param.Value = SCATid;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            while (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                dictionary.Add("Id", Convert.ToInt32(reader["Id"].ToString()));
                dictionary.Add("Name", (reader["Name"].ToString()));
                SalesItemlist.Add(dictionary);
            }
            reader.Close();
            return SalesItemlist;
        }
        public static List<Dictionary<String, String>> GetSalesItemKO()
        {
            Dictionary<string,string> SalesCAT = SalesCategory.GetCategoriesKO(); //Get All Category
            List<Dictionary<string, string>> AllItem = new List<Dictionary<string, string>>();
            int count = 1;
            foreach (var val in SalesCAT) // get Item by Category
            {
         
                String sql = @"SELECT Id,Name FROM SalesItem WHERE INACTIVE = 0 AND SalesCategoryId = @SCATid";
                List<SqlParameter> list = new List<SqlParameter>();
                SqlParameter param = new SqlParameter("@SCATid", DbType.Int32);
                param.Value = Convert.ToInt32(val.Key);
                list.Add(param);
                SqlDataReader reader = DBAccess.FetchResult(sql, list);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                while (reader.Read())
                {
                    if (dictionary.ContainsKey("0")) // check if Category is repeated
                    {
                    }
                    else
                    {
                        dictionary.Add("0", "CAT: "+count+"----"+val.Value.ToString()+"------------------"); // add Category to the list top
                    }
                    dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString()); // add Items
                }
                if (dictionary.Count > 0)
                {
                    AllItem.Add(dictionary);
                    count = count + 1;
                }
                reader.Close();
            }
            return AllItem;
        }

        public static List<Dictionary<String, String>> GetSalesItemKO_ALL()
        {
            Dictionary<string, string> SalesCAT = SalesCategory.GetCategoriesKO_ALL(); //Get All Category
            List<Dictionary<string, string>> AllItem = new List<Dictionary<string, string>>();
            int count = 1;
            foreach (var val in SalesCAT) // get Item by Category
            {
                String sql = @"SELECT Id,Name FROM SalesItem WHERE SalesCategoryId = @SCATid";
                List<SqlParameter> list = new List<SqlParameter>();
                SqlParameter param = new SqlParameter("@SCATid", DbType.Int32);
                param.Value = Convert.ToInt32(val.Key);
                list.Add(param);
                SqlDataReader reader = DBAccess.FetchResult(sql, list);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                while (reader.Read())
                {
                    if (dictionary.ContainsKey("0")) // check if Category is repeated
                    {
                    }
                    else
                    {
                        dictionary.Add("0", "CAT: " + count + "----" + val.Value.ToString() + "------------------"); // add Category to the list top
                    }
                    dictionary.Add(reader["Id"].ToString(), reader["Name"].ToString()); // add Items
                }
                if (dictionary.Count > 0)
                {
                    AllItem.Add(dictionary);
                    count = count + 1;
                }
                reader.Close();
            }
            return AllItem;
        }

        public static Dictionary<object, object> GetItemsRates(int ItemId)
        {
            Dictionary<object, object> List = new Dictionary<object, object>();
            string sql = @"SELECT  UT.Name AS UnitName,
		                            SI.UnitPrice,
		                            C.Name AS Currency,
									TAX.Rate,
									SI.SalesCategoryId
                            FROM SalesItem AS SI
                            INNER JOIN Unit AS UT ON UT.Id = SI.UnitId
                            INNER JOIN Currency AS C ON C.Id = SI.CurrencyId
							INNER JOIN TAX AS TAX ON TAX.Id = SI.GST
                            WHERE SI.Id = @ItemId";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ItemId", DbType.Int32);
            param.Value = ItemId;
            spList.Add(param);
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            while (reader.Read())
            {
                List.Add("UnitName", (reader["UnitName"].ToString()));
                //List.Add("UnitPrice", Convert.ToDecimal((reader["UnitPrice"])));
                List.Add("UnitPrice", (reader["UnitPrice"]));
                List.Add("Currency", (reader["Currency"].ToString()));
                List.Add("IsGST", (reader["Rate"].ToString()));
                List.Add("SalesCategoryId", (reader["SalesCategoryId"].ToString()));
            }
            reader.Close();
            return List;
        }

        public static bool DeleteITEM(int SelectedCATid)
        {
            String sql = @"UPDATE SelectedSalesItem
                            SET DateRemoved = GETDATE()
		                       ,RemovedBy = @EmployeeId
                          WHERE SelectedSalesCategoryId = @SelectedCATid";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@SelectedCATid", DbType.Int32);
            param.Value = SelectedCATid;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }

        public static bool DeleteSpecificITEM(int ItemPriId)
        {
            String sql = @"UPDATE SelectedSalesItem
                            SET DateRemoved = GETDATE()
		                       ,RemovedBy = @EmployeeId
                          WHERE Id = @ItemPriId";
            List<SqlParameter> List = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@ItemPriId", DbType.Int32);
            param.Value = ItemPriId;
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Update(sql, List, ConnectionString.DEFAULT);
        }
    }
}