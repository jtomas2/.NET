using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Checkout
{
    public class CheckoutAddRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string TrackingCode { get; set; }

        [Required]
        public int ShippingAddressId { get; set; }

        [Required]
        public string ChargeId { get; set; }

        [Required]
        public string PaymentAccountId { get; set; }

        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}