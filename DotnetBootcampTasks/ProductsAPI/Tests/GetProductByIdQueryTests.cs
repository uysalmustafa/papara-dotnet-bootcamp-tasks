using AutoMapper;
using Moq;
using ProductsAPI.Models;
using ProductsAPI.ProductOperations.GetProductById;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace ProductsAPI.Tests
{
    public class GetProductByIdQueryTests
    {
        private readonly Mock<ProductContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProductByIdQuery _query;

        public GetProductByIdQueryTests()
        {
            _mockContext = new Mock<ProductContext>();
            _mockMapper = new Mock<IMapper>();
            _query = new GetProductByIdQuery(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public void Handle_ShouldReturnProductViewIdModel()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Product1", ProductPrice = 100 };
            var dbSet = new Mock<DbSet<Product>>();
            dbSet.Setup(d => d.Find(It.IsAny<object[]>())).Returns(product);
            _mockContext.Setup(c => c.Products).Returns(dbSet.Object);
            var productViewModel = new GetProductByIdQuery.ProductViewIdModel { ProductName = "Product1", ProductPrice = 100 };
            _mockMapper.Setup(m => m.Map<GetProductByIdQuery.ProductViewIdModel>(product)).Returns(productViewModel);
            _query.Id = 1;

            // Act
            var result = _query.Handle();

            // Assert
            Assert.Equal(productViewModel.ProductName, result.ProductName);
            Assert.Equal(productViewModel.ProductPrice, result.ProductPrice);
        }
    }

}