using AutoMapper;
using ShopWebApi.DTOs;
using ShopWebApi.Entities;
using ShopWebApi.Helpers;

namespace WebApi.Services
{
    // Product Service Interface
    public interface IProductService
    {
        Task CreateProduct(ProductDto productDto );
        Product GetProductById(int productId, string userId);
        void UpdateProduct(ProductDto productDto, string userId);
        void DeleteProduct(int productId, string userId);
    }

    // Product Service Implementation
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateProduct(ProductDto productDto)
        {
           
            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productDto.ImageFile.CopyToAsync(memoryStream);
                    var base64Image = "data:image/jpeg;base64," + Convert.ToBase64String(memoryStream.ToArray());
                    var product = new Product()
                    {
                        Name = productDto.Name,
                        Description = productDto.Description,
                        UserId = productDto.UserId,
                        ImageUrl = base64Image // Use the base64 image as the ImageUrl property of the Product entity
                    };
                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
            }
            else
            {
                throw new ArgumentNullException("Image file is required.");
            }
        }

        public Product GetProductById(int productId, string userId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId && p.UserId == userId);
            return product;
        }

        public void UpdateProduct(ProductDto productDto, string userId)
        {
            var existingProduct = _context.Products
                .FirstOrDefault(p => p.Id == productDto.Id && p.UserId == userId);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized to update.");
            }

            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    productDto.ImageFile.CopyTo(memoryStream);
                    var base64Image = "data:image/jpeg;base64," + Convert.ToBase64String(memoryStream.ToArray());
                    existingProduct.ImageUrl = base64Image;
                }
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            // Add other properties to update as needed

            _context.SaveChanges();
        }

        public void DeleteProduct(int productId, string userId)
        {
            var existingProduct = _context.Products
                .FirstOrDefault(p => p.Id == productId && p.UserId == userId);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized to delete.");
            }

            _context.Products.Remove(existingProduct);
            _context.SaveChanges();
        }
    }
}
