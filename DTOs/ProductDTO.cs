namespace ShopWebApi.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public IFormFile ImageFile { get; set; }
}