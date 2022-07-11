using System;
using DomainModels.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreMasterProj.Controllers
{
    [ApiController]
    public class ErrorController : ApiBaseController<ErrorController>
    {
        public ErrorController(ILogger<ErrorController> logger)
            : base(logger) { }

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError($"Exception error for {exception.Path} Error: {exception.Error.Message}");

            return this.ExceptionResult(exception.Error);
        }

        private IActionResult ExceptionResult(Exception ex) => ex switch
        {
            NotFoundException e => NotFound(e.Message),
            BadRequestException e => BadRequest(e.Message),
            GuardException e => Problem(e.Message),
            _ => Problem(ex.Message)
        };
    }
}
