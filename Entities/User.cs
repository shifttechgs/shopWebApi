using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShopWebApi.Entities;

public class User: IdentityUser
{
  
     public string firstname { get; set; }

     public string lastname { get; set; }
}