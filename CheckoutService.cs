using Sabio.Data.Providers;
using Sabio.Models.Requests.Checkout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class CheckoutService : ICheckoutService
    {
        private IDataProvider _data = null;

        public CheckoutService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(CheckoutAddRequest model, int userId)
        {
            int id = 0;

            Guid guid = Guid.NewGuid();

            string procName = "[dbo].[Checkout_Orders_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);
                    col.AddWithValue("@TrackingUrl", guid);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    Int32.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        private static void AddCommonParams(CheckoutAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Total", model.Total);
            col.AddWithValue("@TrackingCode", model.TrackingCode);
            col.AddWithValue("@ShippingAddressId", model.ShippingAddressId);
            col.AddWithValue("@ChargeId", model.ChargeId);
            col.AddWithValue("@PaymentAccountId", model.PaymentAccountId);
            col.AddWithValue("@InventoryId", model.InventoryId);
            col.AddWithValue("@Quantity", model.Quantity);
        }
    }
}