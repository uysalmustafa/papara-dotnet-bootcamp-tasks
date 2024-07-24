using AutoMapper;
using Moq;
using ProductsAPI.Models;
using ProductsAPI.ProductOperations.UpdateProduct;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace ProductsAPI.Tests
{
    public class UpdateProductCommandTests
    {
        private readonly Mock<ProductContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProductCommand _command;

        public UpdateProductCommandTests()
        {
            _mockContext = new Mock<ProductContext>();
            _mockMapper = new Mock<IMapper>();
            _command = new UpdateProductCommand(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public void Handle_ShouldUpdateProductInContext()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "OldName", ProductPrice = 100 };
            var dbSet = new Mock<DbSet<Product>>();
            dbSet.Setup(d => d.SingleOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .Returns(product);
            _mockContext.Setup(c => c.Products).Returns(dbSet.Object);
            var model = new UpdateProductCommand.UpdateProductModel { ProductName = "NewName", ProductPrice = 200 };
            _command.Model = model;
            _command.Id = 1;

            // Act
            _command.Handle();

            // Assert
            Assert.Equal(model.ProductName, product.ProductName);
            Assert.Equal(model.ProductPrice, product.ProductPrice);
            _mockContext.Verify(c => c.SaveChanges(), Moq.Times.Once);
        }
    }
}