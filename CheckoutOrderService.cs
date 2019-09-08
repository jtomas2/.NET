using Sabio.Data.Providers;
using Sabio.Models.Requests.Checkout;
using Sabio.Models.Requests.ShoppingCart;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class CheckoutOrderService : ICheckoutOrderService
    {
        private IDataProvider _dataProvider = null;

        public CheckoutOrderService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public int Insert(CheckoutOrderAddRequest model, int userId, string chargeId)
        {
            int OrderId = 0;

            int oTotal = Decimal.ToInt32(model.Payment.Total);

            _dataProvider.ExecuteNonQuery("[dbo].[Checkout_Orders_Insert_V3]", inputParamMapper: delegate (SqlParameterCollection col)
            {
                //Current User
                col.AddWithValue("@CreatedBy", userId);

                //Shipping Address

                col.AddWithValue("@saLineOne", model.Shipping.LineOne);
                col.AddWithValue("@saLineTwo", model.Shipping.LineTwo);
                col.AddWithValue("@saCity", model.Shipping.City);
                col.AddWithValue("@saZip", model.Shipping.Zip);
                col.AddWithValue("@saStateId", model.Shipping.StateId);
                
                //Billing Address

                col.AddWithValue("@baLineOne", model.Billing.LineOne);
                col.AddWithValue("@baLineTwo", model.Billing.LineTwo);
                col.AddWithValue("@baCity", model.Billing.City);
                col.AddWithValue("@baZip", model.Billing.Zip);
                col.AddWithValue("@baStateId", model.Billing.StateId);

                //Order

                col.AddWithValue("@Total", oTotal);
                col.AddWithValue("@TrackingCode", model.Order.TrackingCode);
                col.AddWithValue("@TrackingUrl", model.Order.TrackingUrl);
                col.AddWithValue("@ChargeId", chargeId);

                //orderitems

                DataTable OrderItems = null;
                if (model.ShoppingCartItems != null)
                {
                    OrderItems = new DataTable();
                    OrderItems.Columns.Add("InventoryId", typeof(int));
                    OrderItems.Columns.Add("Quantity", typeof(int));
                    OrderItems.Columns.Add("DateCreated", typeof(DateTime));
                    OrderItems.Columns.Add("DateModified", typeof(DateTime));
                    OrderItems.Columns.Add("CreatedBy", typeof(int));
                    OrderItems.Columns.Add("ModifiedBy", typeof(int));

                    foreach (ShoppingCartAddRequestV2 item in model.ShoppingCartItems)
                    {
                        OrderItems.Rows.Add(item.InventoryId, item.Quantity, item.DateCreated, item.DateModified, item.CreatedBy, item.ModifiedBy);
                    }

                    col.AddWithValue("@OrderItems", OrderItems);
                }
                SqlParameter idOut = new SqlParameter("@OrderId", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@OrderId"].Value;

                int.TryParse(oId.ToString(), out OrderId);
            });
            return OrderId;
        }

        public void VoidOrder(int userId)
        {
            string procName = "[dbo].[Checkout_Orders_EmptyCart]";
            _dataProvider.ExecuteNonQuery(procName,

                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@CreatedBy", userId);
                },
            returnParameters: null);
        }
    }
}