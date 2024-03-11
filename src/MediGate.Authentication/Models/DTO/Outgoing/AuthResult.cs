using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediGate.Authentication.Models.DTO.Outgoing
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
    }
}