using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MediGate.Entities.DbSet
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; } // UserId when logged in
        public string Token { get; set; }
        public string JwtId { get; set; }  // The Id generated when a jwt id has been requested(JTI Id)
        public bool IsUsed { get; set; } // To make sure that the token is only used once
        public bool IsRevoked { get; set; } // Make sure they are valid

        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}