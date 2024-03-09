using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace MediGate.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}