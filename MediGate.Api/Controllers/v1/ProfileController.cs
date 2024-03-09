using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediGate.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        public ProfileController(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager) : base(unitOfWork, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser is null)
            {
                return BadRequest("User not found");
            }

            var identityId = new Guid(loggedInUser.Id);
            var profile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if (profile is null)
            {
                return BadRequest("User not found");
            }

            return Ok(profile);
        }
    }
}