using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediGate.Authentication.Configuration;
using MediGate.Authentication.Models.DTO.Generic;
using MediGate.Authentication.Models.DTO.Incoming;
using MediGate.Authentication.Models.DTO.Outgoing;
using MediGate.DataService.IConfiguration;
using MediGate.Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediGate.Api.Controllers.v1
{
    public class AccountsController : BaseController
    {
        // Class provided by AspNetCore Identity Framework
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtConfiguration _jwtConfig;
        public AccountsController(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        TokenValidationParameters tokenValidationParameters,
        IOptionsMonitor<JwtConfiguration> optionsMonitor) : base(unitOfWork)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _unitOfWork = unitOfWork;
            _tokenValidationParameters = tokenValidationParameters;
        }

        // Register Action
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO registrationDto)
        {
            // Check the model or object we are receiving is valid
            if (ModelState.IsValid)
            {
                // Check if the email already exist
                var userExists = await _userManager.FindByEmailAsync(registrationDto.Email);

                if (userExists is not null)
                {
                    return BadRequest(new UserRegistrationResponseDTO()
                    {
                        IsSuccess = false,
                        Errors = new List<string>(){
                        "Email already in use"
                        }
                    });
                }

                // Add the user
                var newUser = new IdentityUser()
                {
                    Email = registrationDto.Email,
                    UserName = registrationDto.Email,
                    EmailConfirmed = true // ToDO build email confirmation functionality
                };

                // Adding the user to the table
                var isCreated = await _userManager.CreateAsync(newUser, registrationDto.Password);

                if (!isCreated.Succeeded) // When the registration has failed
                {
                    return BadRequest(new UserRegistrationResponseDTO()
                    {
                        IsSuccess = isCreated.Succeeded,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }

                // Adding user to the database
                var _user = new User();
                _user.UserId = new Guid(newUser.Id);
                _user.FirstName = registrationDto.FirstName;
                _user.LastName = registrationDto.LastName;
                _user.Email = registrationDto.Email;
                _user.DateOfBirth = DateTime.UtcNow; //Convert.ToDateTime(registrationDto.DateOfBirth);
                _user.Phone = "1234567890";
                _user.Country = "Nepal";
                _user.Status = 1;

                await _unitOfWork.Users.Add(_user);
                await _unitOfWork.CompleteAsync();

                // Create a Jwt Token
                var token = await GenerateJwtToken(newUser);

                // Return it back to the user
                return Ok(new UserRegistrationResponseDTO()
                {
                    IsSuccess = true,
                    Token = token.JwtToken,
                    RefreshToken = token.RefreshToken
                });
            }
            else // Invalid object
            {
                return BadRequest(new UserRegistrationResponseDTO
                {
                    IsSuccess = false,
                    Errors = new List<string>(){
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                // 1 - Check if email exist
                var userExist = await _userManager.FindByEmailAsync(loginDto.Email);

                if (userExist is null)
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        IsSuccess = false,
                        Errors = new List<string>(){
                            "Invalid Authentication Request"
                        }
                    });
                }

                // 2 - Check if the user has valid password
                var isCorrect = await _userManager.CheckPasswordAsync(userExist, loginDto.Password);

                if (isCorrect)
                {
                    // We need to generate the JWT Token
                    var token = await GenerateJwtToken(userExist);
                    return Ok(new UserLoginResponseDTO()
                    {
                        IsSuccess = true,
                        Token = token.JwtToken,
                        RefreshToken = token.RefreshToken
                    });
                }
                else
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        IsSuccess = false,
                        Errors = new List<string>(){
                            "Password doesn't match"
                        }
                    });
                }

            }
            else // Invalid Object
            {
                return BadRequest(new UserRegistrationResponseDTO()
                {
                    IsSuccess = false,
                    Errors = new List<string>(){
                        "Invalid Payload"
                        }
                });
            }

        }
        private async Task<TokenData> GenerateJwtToken(IdentityUser user)
        {
            // The handler is going to be responsible for creating the token
            var jwtHandler = new JwtSecurityTokenHandler();

            // Get the security key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email), // sub refers to the unique ID
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Jti is used by refresh token
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame), // Update the expiration time to minutes
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature // ToDo review the algorithm down the road
                )
            };

            // Generates the security object token
            var token = jwtHandler.CreateToken(tokenDescriptor);

            // Convert the security object token into the string
            var jwtToken = jwtHandler.WriteToken(token);

            // Generate Refresh token
            var refreshToken = new RefreshToken
            {
                AddedDate = DateTime.UtcNow,
                Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}", // 
                UserId = user.Id,
                IsRevoked = false,
                IsUsed = false,
                Status = 1,
                JwtId = token.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync();

            var tokenData = new TokenData
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private string RandomStringGenerator(int length)
        {

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}