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
using System.Threading.Tasks;
using ShopWebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<(string token, User user)> AuthenticateUser(AuthenticateRequest model);
        void Register(RegisterRequest request);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<(string token, User user)> AuthenticateUser(AuthenticateRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var dbUser = await GetUserByUsername(model.Username);

                if (dbUser == null)
                {
                    throw new InvalidOperationException("User not found in the database.");
                }

                var token = await GenerateToken(model.Username, model.Password);

                return (token, dbUser);
            }

            return (null, null);
        }

        private async Task<string> GenerateToken(string username, string password)
        {
            var client = new AuthenticationApiClient(_configuration["Auth0:Domain"]);
            var response = await client.GetTokenAsync(new ResourceOwnerTokenRequest
            {
                Audience = _configuration["Auth0:Audience"],
                ClientId = _configuration["Auth0:ClientId"],
                ClientSecret = _configuration["Auth0:ClientSecret"],
                Scope = "openid",
                Realm = "Username-Password-Authentication",
                Username = username,
                Password = password
            });
            
           
            if (response.AccessToken != null)
            {
                return response.AccessToken;
            }
            else
            {
                throw new InvalidOperationException("Invalid credentials.");
            }

            return response.AccessToken;
        }

        private async Task<User> GetUserByUsername(string username)
        {
            return await Task.FromResult(_context.Users.SingleOrDefault(x => x.Username == username));
        }

        public void Register(RegisterRequest model)
        {
            
            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");


            var user = _mapper.Map<User>(model);
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}