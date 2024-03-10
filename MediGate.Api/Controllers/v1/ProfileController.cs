using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.IConfiguration;
using MediGate.Entities.DbSet;
using MediGate.Entities.DTOs.Errors;
using MediGate.Entities.DTOs.Generic;
using MediGate.Entities.DTOs.Incoming.Profile;
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
            var result = new Result<User>();

            if (loggedInUser is null)
            {
                result.Error = new Error()
                {
                    Code = 400,
                    Type = "Bad Request",
                    Message = "User not found"

                };

                return BadRequest(result);
            }

            var identityId = new Guid(loggedInUser.Id);
            var profile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if (profile is null)
            {

                result.Error = new Error()
                {
                    Code = 400,
                    Type = "Bad Request",
                    Message = "User not found"

                };
                return BadRequest(result);
            }

            result.Content = profile;

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO profile)
        {
            // If the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Payload");
            }

            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser is null)
            {
                return BadRequest("User not found");
            }

            var identityId = new Guid(loggedInUser.Id);
            var userProfile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if (userProfile is null)
            {
                return BadRequest("User not found");
            }

            userProfile.Address = profile.Address;
            userProfile.Sex = profile.Sex;
            userProfile.MobileNumber = profile.MobileNumber;
            userProfile.Country = profile.Country;

            var isUpdated = await _unitOfWork.Users.UpdateUserProfile(userProfile);

            if (!isUpdated)
            {
                return BadRequest("Something went wrong, Please try again later!");
            }
            await _unitOfWork.CompleteAsync();
            return Ok(userProfile);

        }
    }
}