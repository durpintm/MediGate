using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediGate.Entities.DTOs.Incoming.Profile
{
    public class UpdateProfileDTO
    {
        public string Country { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
    }
}