using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MediGate.Entities.DbSet;

namespace MediGate.DataService.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {

    public new DbSet<User> Users { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
  }
}