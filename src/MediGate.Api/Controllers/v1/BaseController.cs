using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediGate.DataService.IConfiguration;
using MediGate.Entities.DTOs.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediGate.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        public UserManager<IdentityUser> _userManager;
        public readonly IMapper _mapper;
        public BaseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        internal Error PopulateError(int code, string type, string message)
        {
            return new Error()
            {
                Code = code,
                Message = message,
                Type = type
            };
        }
    }
}