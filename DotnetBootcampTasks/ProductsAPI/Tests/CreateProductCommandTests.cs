using com.sun.org.apache.xpath.@internal.operations;
using AutoMapper;
using com.sun.tools.corba.se.idl.constExpr;
using Moq;
using ProductsAPI.Models;
using ProductsAPI.ProductOperations.CreateProduct;
using Xunit;


namespace ProductsAPI.Tests
{

    public class CreateProductCommandTests
    {
        private readonly Mock<ProductContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateProductCommand _command;

        public CreateProductCommandTests()
        {
            _mockContext = new Mock<ProductContext>();
            _mockMapper = new Mock<IMapper>();
            _command = new CreateProductCommand(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public void Handle_ShouldAddProductToContext()
        {
            // Arrange
            var model = new CreateProductCommand.CreateProductModel { Id = 1, ProductName = "Product1", ProductPrice = 100 };
            var product = new Product { Id = 1, ProductName = "Product1", ProductPrice = 100 };
            _mockMapper.Setup(m => m.Map<Product>(model)).Returns(product);
            _command.Model = model;

            // Act
            _command.Handle();

            // Assert
            _mockContext.Verify(c => c.Products.Add(It.Is<Product>(p => p.Id == product.Id)), Moq.Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Moq.Times.Once);
        }
    }

}
