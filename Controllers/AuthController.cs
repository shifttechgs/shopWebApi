using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWebApi.DTOs;
using ShopWebApi.Entities;
using ShopWebApi.Helpers;
using ShopWebApi.Models;
using ShopWebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;

    public AuthController(IUserService userService, IMapper mapper, UserManager<User> userManager,SignInManager<User> signInManager)
    {
        _userService = userService;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
    }

  

    [HttpPost("register")]
   
    public async Task<IActionResult> Register(UserDTO userDto)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(500, new { Error = "An error occurred during authentication" });
        }

        var registeredUser = await _userService.RegisterUserAsync(userDto);
        if (registeredUser == null)
        {
            return BadRequest(new { Error = "User registration failed." });
        }

        return Ok(registeredUser);
    }
    
    
   [HttpPost("login")]
    
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Error = "An error occurred during authentication" });
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            // Create ClaimsIdentity and add claims
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            // Sign in the user
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));

            // Return the user details
            return Ok(user);
        }
        else
        {
            return BadRequest(new { Error = "Invalid UserName or Password" });
        }
    }
    
   
    
    
  
}