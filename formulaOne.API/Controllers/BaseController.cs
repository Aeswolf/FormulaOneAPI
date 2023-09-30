using AutoMapper;
using formulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace formulaOne.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    protected readonly ILogger _logger;

    protected readonly IDistributedCache _distributedCache;

    public BaseController(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IDistributedCache distributedCache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _distributedCache = distributedCache;
    }
}