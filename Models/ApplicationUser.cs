using Microsoft.AspNetCore.Identity;

namespace SmartPark.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName  { get; set; }
    public string? RegistrskaStevilka { get; set; }

}
