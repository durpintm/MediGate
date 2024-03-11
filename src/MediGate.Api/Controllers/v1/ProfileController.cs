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
using MediGate.Cofiguration.Messages;
using AutoMapper;
using MediGate.Entities.DTOs.Outgoing.Profiles;

namespace MediGate.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        public ProfileController(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            var result = new Result<ProfileDTO>();

            if (loggedInUser is null)
            {
                result.Error = PopulateError(400, ErrorMessages.Generic.TypeBadRequest, ErrorMessages.Profile.UserNotFound);
                return BadRequest(result);
            }

            var identityId = new Guid(loggedInUser.Id);
            var profile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if (profile is null)
            {
                result.Error = PopulateError(400, ErrorMessages.Generic.TypeBadRequest, ErrorMessages.Profile.UserNotFound);
                return BadRequest(result);
            }

            var mappedProfile = _mapper.Map<ProfileDTO>(profile);
            result.Content = mappedProfile;
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO profile)
        {
            var result = new Result<ProfileDTO>();

            // If the model is valid
            if (!ModelState.IsValid)
            {
                result.Error = PopulateError(400, ErrorMessages.Generic.TypeBadRequest, ErrorMessages.Generic.InvalidPayload);
                return BadRequest(result);
            }

            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser is null)
            {
                result.Error = PopulateError(400, ErrorMessages.Generic.TypeBadRequest, ErrorMessages.Profile.UserNotFound);
                return BadRequest(result);
            }

            var identityId = new Guid(loggedInUser.Id);
            var userProfile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if (userProfile is null)
            {
                result.Error = PopulateError(400, ErrorMessages.Generic.TypeBadRequest, ErrorMessages.Profile.UserNotFound);
                return BadRequest(result);
            }

            userProfile.Address = profile.Address;
            userProfile.Sex = profile.Sex;
            userProfile.MobileNumber = profile.MobileNumber;
            userProfile.Country = profile.Country;

            var isUpdated = await _unitOfWork.Users.UpdateUserProfile(userProfile);

            if (!isUpdated)
            {
                result.Error = PopulateError(500, ErrorMessages.Generic.UnableToProcess, ErrorMessages.Generic.SomethingWentWrong);

                return BadRequest(result);
            }
            await _unitOfWork.CompleteAsync();
            var mappedProfile = _mapper.Map<ProfileDTO>(userProfile);
            result.Content = mappedProfile;
            return Ok(result);

        }
    }
}