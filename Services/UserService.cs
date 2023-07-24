using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopWebApi.Entities;
using ShopWebApi.Helpers;
using ShopWebApi.Models.Users;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ShopWebApi.DTOs;
using ShopWebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
    
        Task<User> RegisterUserAsync(UserDTO userDto);
        Task<(string token, User user)> Login(UserLoginDto loginDto);
        
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(DataContext context, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

     
        public async Task<User> RegisterUserAsync(UserDTO userDto)
        {
            var user = new User
            {
                UserName = userDto.firstname,
                firstname = userDto.firstname,
                lastname = userDto.lastname,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
              
                return null;
            }
            
            return user;
        }
        
        public async Task<(string token, User user)> Login(UserLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                // Create ClaimsIdentity and add claims
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Return the user details
                return (null, user);
            }
            else
            {
                return ("Invalid UserName or Password", null);
            }
        }
        
      
    }
}