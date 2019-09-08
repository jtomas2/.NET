using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Checkout;
using Sabio.Models.Requests.ShoppingCart;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/checkoutorder")]
    [ApiController]
    public class CheckoutOrderApiController : BaseApiController
    {
        private ICheckoutOrderService _service = null;
        private IStripeService _stripe = null;
        private IShoppingCartService _cartService = null;
        private IAuthenticationService<int> _authService = null;

        public CheckoutOrderApiController(ICheckoutOrderService service
            , IStripeService stripe
            , IShoppingCartService cartService
            , ILogger<CheckoutOrderApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _stripe = stripe;
            _cartService = cartService;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Insert(CheckoutOrderAddRequest model)
        {
            ObjectResult result = null;

            int user = _authService.GetCurrentUserId();

            try
            {
                int userId = _authService.GetCurrentUserId();
                string chargeId = _stripe.Charge(model.Payment.Token, model.Payment.Total, model.Shipping.EmailAddress);
                model.ShoppingCartItems = _cartService.GetByCurrent(user);
                int id = _service.Insert(model, userId, chargeId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("void")]
        public ActionResult<ItemResponse<int>> Void()
        {
            int iCode = 200;
            BaseResponse response = null;
            int userId = _authService.GetCurrentUserId();
            try
            {
                _service.VoidOrder(userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }
    }
}