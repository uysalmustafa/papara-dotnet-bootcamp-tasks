// Import necessary namespaces
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Base;
using ProductsAPI.Controllers;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    // Define the ProductService class implementing IProductService interface
    public class ProductService : IProductService
    {
        // Dependency injection for the ProductContext
        private readonly ProductContext _context;

        // Constructor to initialize the context
        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public BaseResponse<Product> DeleteProduct([FromRoute] int id)
        {
            var product = _context.Products.Find(id);  
            _context.Products.Remove(product);        
            _context.SaveChanges();                  
            return BaseResponse<Product>.Success(200, product);  
        }

        // Method to get all products
        public BaseResponse<IEnumerable<Product>> GetProducts()
        {
            var products = from p in _context.Products
                           select p;
            return BaseResponse<IEnumerable<Product>>.Success(200, products.ToList());  
        }

        // Method to list products based on query parameters
        public BaseResponse<IEnumerable<Product>> List([FromQuery] QueryObject product)
        {
            var products = _context.Products.AsQueryable();  // Query products as IQueryable
            if (!string.IsNullOrEmpty(product.ProductName))
            {
                products = products.Where(e => e.ProductName.Contains(product.ProductName));  // Filter by product name
            }
            return BaseResponse<IEnumerable<Product>>.Success(200, products.ToList());  
        }

        // Method to add a new product
        public BaseResponse<Product> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product); 
            _context.SaveChanges();        
            return BaseResponse<Product>.Success(201, product);  
        }

        // Method to get a product by id
        public BaseResponse<Product> ProductById([FromRoute] int id)
        {
            var product = _context.Products.Find(id); 
            return BaseResponse<Product>.Success(200, product); 
        }

        // Method to update an existing product
        public BaseResponse<Product> UpdateProduct([FromRoute] int id, [FromBody] Product product)
        {
            _context.Entry(product).State = EntityState.Modified; 
            _context.SaveChanges();                               
            return BaseResponse<Product>.Success(200, product);   
        }
    }
}
