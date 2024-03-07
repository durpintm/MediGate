using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.Data;
using MediGate.DataService.IConfiguration;
using MediGate.Entities.DbSet;
using MediGate.Entities.DTOs.Incoming;
using Microsoft.AspNetCore.Mvc;

namespace MediGate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork) // ApplicationDbContext context
        {
            _unitOfWork = unitOfWork;
        }

        //Get
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(users);
        }

        //Post
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserDTO user)
        {
            var _user = new User();
            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;
            _user.Email = user.Email;
            _user.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
            _user.Phone = user.Phone;
            _user.Country = user.Country;
            _user.Status = 1;

            await _unitOfWork.Users.Add(_user);
            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetUser", new { id = _user.Id }, user); // return a 201
        }

        [HttpGet]
        [Route("GetUser/{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid Id)
        {
            var user = await _unitOfWork.Users.GetById(Id);
            return Ok(user);
        }

    }
}