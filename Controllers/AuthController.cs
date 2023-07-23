using Microsoft.AspNetCore.Mvc;
using ShopWebApi.Helpers;
using ShopWebApi.Models;
using ShopWebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthenticateRequest request)
    {
        try
        {
            var (token, user) = await _userService.AuthenticateUser(request);

            if (user != null && token != null)
            {
                return Ok(new { Token = token, User = user });
            }
            else
            {
                return Unauthorized(new { Error = "Invalid credentials" });
            }
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred during authentication" });
        }
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        try
        {
            _userService.Register(request);
            return Ok(new { message = "Registration successful" });
        }
        catch (AppException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred during registration" });
        }
    }
}