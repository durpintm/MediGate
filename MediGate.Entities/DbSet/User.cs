using System;

namespace MediGate.Entities.DbSet
{
  public class User : BaseEntity
  {
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Country { get; set; }
  }
}
