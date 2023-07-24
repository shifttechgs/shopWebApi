using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ShopWebApi.DTOs;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
    
    public ClaimsPrincipal Principal { get; set; }
}