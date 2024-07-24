using AutoMapper;
using Moq;
using ProductsAPI.Models;
using ProductsAPI.ProductOperations.GetProducts;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace ProductsAPI.Tests
{
    public class GetProductsQueryTests
    {
        private readonly Mock<ProductContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProductsQuery _query;

        public GetProductsQueryTests()
        {
            _mockContext = new Mock<ProductContext>();
            _mockMapper = new Mock<IMapper>();
            _query = new GetProductsQuery(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public void Handle_ShouldReturnListOfProductsViewModel()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, ProductName = "Product1", ProductPrice = 100 },
            new Product { Id = 2, ProductName = "Product2", ProductPrice = 200 }
        }.AsQueryable();
            var dbSet = new Mock<DbSet<Product>>();
            dbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            dbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            dbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            dbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            _mockContext.Setup(c => c.Products).Returns(dbSet.Object);
            var productsViewModel = new List<GetProductsQuery.ProductsViewModel>
        {
            new GetProductsQuery.ProductsViewModel { ProductName = "Product1", ProductPrice = 100 },
            new GetProductsQuery.ProductsViewModel { ProductName = "Product2", ProductPrice = 200 }
        };
            _mockMapper.Setup(m => m.Map<List<GetProductsQuery.ProductsViewModel>>(products)).Returns(productsViewModel);

            // Act
            var result = _query.Handle();

            // Assert
            Assert.Equal(productsViewModel.Count, result.Count);
            Assert.Equal(productsViewModel[0].ProductName, result[0].ProductName);
            Assert.Equal(productsViewModel[0].ProductPrice, result[0].ProductPrice);
        }
    }
}