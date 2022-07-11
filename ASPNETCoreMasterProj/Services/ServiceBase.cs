using AutoMapper;
using DomainModels.Extensions;
using Microsoft.Extensions.Logging;

namespace Services
{
    public abstract class ServiceBase<TService>
    {
        protected readonly ILogger<TService> _logger;
        protected readonly IMapper _mapper;

        protected ServiceBase(IMapper mapper, ILogger<TService> logger)
        {
            _logger = logger.MustBeImplemented();
            _mapper = mapper.MustBeImplemented();
        }
    }
}
