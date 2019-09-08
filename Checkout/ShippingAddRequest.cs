using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests.Checkout
{
    public class ShippingAddRequest
    {
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string afName { get; set; }
        public string afStreet1 { get; set; }
        public string afCity { get; set; }
        public string afState { get; set; }
        public string afZip { get; set; }
        public string afCountry { get; set; }
        public string afPhone { get; set; }
        public string afEmail { get; set; }


        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Distance_unit { get; set; }
        public string Weight { get; set; }
        public string Mass_unit { get; set; }

    }
}
