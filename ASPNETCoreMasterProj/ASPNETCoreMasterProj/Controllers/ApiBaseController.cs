using DomainModels.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreMasterProj.Controllers
{
    public class ApiBaseController<TController> : ControllerBase
    {
        protected readonly ILogger<TController> _logger;

        public ApiBaseController(ILogger<TController> logger)
        {
            _logger = logger.MustBeImplemented();
        }
    }
}
