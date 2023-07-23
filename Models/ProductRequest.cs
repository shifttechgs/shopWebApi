using System.ComponentModel.DataAnnotations;

namespace ShopWebApi.Models;

public class ProductRequest
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    
    public string ImageUrl { get; set; }
}