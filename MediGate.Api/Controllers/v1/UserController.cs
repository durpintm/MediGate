using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediGate.Cofiguration.Messages;
using MediGate.DataService.Data;
using MediGate.DataService.IConfiguration;
using MediGate.Entities.DbSet;
using MediGate.Entities.DTOs.Generic;
using MediGate.Entities.DTOs.Incoming;
using MediGate.Entities.DTOs.Outgoing.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediGate.Api.Controllers.v1
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        public UserController(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
        }

        //Get
        [HttpGet]
        // [Route("GetUsers")]
        [HttpHead]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            var result = new PagedResult<User>();
            result.Content = users.ToList();
            result.ResultCount = users.Count();
            return Ok(result);
        }

        //Post
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserDTO user)
        {
            var _mappedUser = _mapper.Map<User>(user);
            // var _user = new User();
            // _user.FirstName = user.FirstName;
            // _user.LastName = user.LastName;
            // _user.Email = user.Email;
            // _user.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
            // _user.Phone = user.Phone;
            // _user.Country = user.Country;
            // _user.Status = 1;

            await _unitOfWork.Users.Add(_mappedUser);
            await _unitOfWork.CompleteAsync();

            //Todo: Add the correct return to this action
            var result = new Result<UserDTO>();
            result.Content = user;
            return CreatedAtRoute("GetUser", new { id = _mappedUser.Id }, result); // return a 201
        }

        [HttpGet]
        [Route("GetUser/{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid Id)
        {
            var user = await _unitOfWork.Users.GetById(Id);
            var result = new Result<ProfileDTO>();

            if (user is not null)
            {
                var mappedProfile = _mapper.Map<ProfileDTO>(user);
                result.Content = mappedProfile;
                return Ok(result);
            }

            result.Error = PopulateError(404, ErrorMessages.Generic.ObjectNotFound, ErrorMessages.Users.UserNotFound);

            return BadRequest(result);

        }

    }
}