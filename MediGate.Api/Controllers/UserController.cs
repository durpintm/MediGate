using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.Data;
using MediGate.Entities.DbSet;
using MediGate.Entities.DTOs.Incoming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediGate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Where(x => x.Status == 1).ToList();
            return Ok(users);
        }

        //Post
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(UserDTO user)
        {
            var _user = new User();
            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;
            _user.Email = user.Email;
            _user.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
            _user.Phone = user.Phone;
            _user.Country = user.Country;
            _user.Status = 1;

            _context.Users.Add(_user);
            _context.SaveChanges();

            return Ok(); // return a 201
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public IActionResult GetUser(Guid Id)
        {
            var user = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            return Ok(user);
        }

    }
}