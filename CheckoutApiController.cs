using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Checkout;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Shippo;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutApiController : BaseApiController
    {
        private ICheckoutService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CheckoutApiController(ICheckoutService service
            , ILogger<CheckoutApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CheckoutAddRequest model)
        {
            ObjectResult result = null;
            int userId = _authService.GetCurrentUserId();

            try
            {
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse($"Generic Error: {ex.Message}");
                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPost("ship")]
        public ActionResult<object> ShipAsync(ShippoShipping model)
        {
            APIResource resource = new APIResource("shippo_test_4b21ff87309dbb951077d18f554f1ffc1d1f9988");

            Hashtable parameters = new Hashtable();

            parameters.Add("address_to",  model.address_to);
            parameters.Add("address_from", model.address_from);
            parameters.Add("parcels", model.parcels);
            parameters.Add("async", false);

            var shipmentInfo = resource.CreateShipment(parameters);

            return shipmentInfo;
        }

        [HttpPost("shippingInfo")]
        public ActionResult<object> ShippingLabelAsync(ShippoShippingLabel model)
        {
            APIResource resource = new APIResource("shippo_test_4b21ff87309dbb951077d18f554f1ffc1d1f9988");
           
            Hashtable parameters = new Hashtable();


            parameters.Add("shipment", model.shipment );
            parameters.Add("carrier_account", model.carrier_account);
            parameters.Add("servicelevel_token", model.servicelevel_token);

            var shippingLabel = resource.CreateTransaction(parameters);

            return shippingLabel;
        }


    }
}
