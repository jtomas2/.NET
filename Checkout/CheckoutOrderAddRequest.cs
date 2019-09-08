using Sabio.Models.Requests.CheckoutFinalOrder;
using Sabio.Models.Requests.Location;
using Sabio.Models.Requests.ShoppingCart;
using Sabio.Models.Requests.Stripe;
using System.Collections.Generic;

namespace Sabio.Models.Requests.Checkout
{
    public class CheckoutOrderAddRequest
    {
        public CheckoutFinalOrderAddRequest Shipping { get; set; }

        public CheckoutFinalOrderAddRequest Billing { get; set; }

        public OrderAddRequest Order { get; set; }

        public List<ShoppingCartAddRequestV2> ShoppingCartItems { get; set; }

        public PaymentAddRequest Payment { get; set; }

    }
}