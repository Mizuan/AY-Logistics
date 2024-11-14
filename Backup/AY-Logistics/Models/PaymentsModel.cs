using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class PaymentsModel
    {
    }


    public class DOPayment
    {
        public int shipmentId { get; set; }
        public string DebitNoteNo { get; set; }
        public double Ammount { get; set; }
        public string DOcollectVoucherNo { get; set; }
        public int RequestedPaymentType { get; set; }
        public string CurrencyType { get; set; }

        public bool SaveDOPayement()
        {
            string sql = @"Update Shipment
                            SET DebitNoteNo = @DebitNoteNo
                               ,DebitNoteAmount = @Ammount
                               ,CurrencyType = @CurrencyType
                            WHERE Id = @ShipmentId

                        INSERT INTO [dbo].[ShipmentStatusHistory]
		                        ([ShipmentId]
		                        ,[ShipmentStatusId]
		                        ,[DateCreated]
		                        ,[EmployeeId]
                                ,[RequestedPayment])
                        VALUES
		                        (@ShipmentId
		                        ,3
		                        ,GETDATE()
		                        ,@EmployeeId
                                ,@RequestedPayment)";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@DebitNoteNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(DebitNoteNo) ? (object)DBNull.Value : DebitNoteNo.ToUpper();
            list.Add(param);
            param = new SqlParameter("@Ammount", DbType.Decimal);
            param.Value = Ammount;
            list.Add(param);
            param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            list.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            list.Add(param);
            param = new SqlParameter("@RequestedPayment", DbType.Int32);
            param.Value = DBNull.Value;
            if (RequestedPaymentType > 0)
            {
                param.Value = RequestedPaymentType;
            }
            list.Add(param);
            param = new SqlParameter("@CurrencyType", DbType.String);
            param.Value = CurrencyType;
            list.Add(param);

            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool ApproveDOPayement()
        {
            string sql = @"Update Shipment
                            SET DOcollectVoucherNo = @DOcollectVoucherNo
                            WHERE Id = @ShipmentId

                        INSERT INTO [dbo].[ShipmentStatusHistory]
		                        ([ShipmentId]
		                        ,[ShipmentStatusId]
		                        ,[DateCreated]
		                        ,[EmployeeId])
                        VALUES
		                        (@ShipmentId
		                        ,4
		                        ,GETDATE()
		                        ,@EmployeeId)";
            List<SqlParameter> list = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@DOcollectVoucherNo", DbType.String);
            param.Value = string.IsNullOrWhiteSpace(DOcollectVoucherNo) ? (object)DBNull.Value : DOcollectVoucherNo.ToUpper();
            list.Add(param);
            param = new SqlParameter("@ShipmentId", DbType.Int32);
            param.Value = shipmentId;
            list.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            list.Add(param);
            return DBAccess.Insert(sql, list, ConnectionString.DEFAULT);
        }

        public bool SavePaymentStat(int HouseBLId)
        {
            String sql = @"INSERT INTO [dbo].[Payments]
                                       ([HouseBLId]
                                       ,[PaymentTypesId]
                                       ,[UpdatedDate]
                                       ,[PaidUpdatedBy]
                                       ,[PaymentStat])
                                 VALUES
                                       (@HouseBLId
                                       ,@PaymentTypesId
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
											                           (@HouseBLId
											                           ,5
											                           ,GETDATE()
											                           ,@EmployeeId
                                                                       ,1)";
            List<SqlParameter> List = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@HouseBLId", DbType.Int32);
            param.Value = HouseBLId;
            List.Add(param);
            param = new SqlParameter("@PaymentTypesId", DbType.Int32);
            if (RequestedPaymentType == 1)
            {
                param.Value = 1; // Freight Invoice
            }
            else
            {
                param.Value = 2; // Clearance Invoice
            }
            List.Add(param);
            param = new SqlParameter("@EmployeeId", DbType.Int32);
            param.Value = Convert.ToInt32(Profile.GetProfile(HttpContext.Current.User.Identity.Name).GetPropertyValue("ACSUserId"));
            List.Add(param);
            return DBAccess.Insert(sql, List, ConnectionString.DEFAULT);
        }

    }
}