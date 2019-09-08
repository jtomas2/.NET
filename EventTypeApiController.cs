using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Event;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventTypeApiController : BaseApiController
    {
        private IEventTypeService _service = null;
        private IAuthenticationService<int> _authService = null;

        public EventTypeApiController(IEventTypeService service
            , ILogger<EventTypeApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("type"), AllowAnonymous]
        public ActionResult<ItemResponse<EventType>> Get()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<EventType> eventType = _service.Get();

                if (eventType == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<List<EventType>> { Item = eventType };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}