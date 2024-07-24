using Moq;
using ProductsAPI.Models;
using ProductsAPI.ProductOperations.DeleteProduct;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class DeleteProductCommandTests
{
    private readonly Mock<ProductContext> _mockContext;
    private readonly DeleteProductCommand _command;

    public DeleteProductCommandTests()
    {
        _mockContext = new Mock<ProductContext>();
        _command = new DeleteProductCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_ShouldRemoveProductFromContext()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Product1", ProductPrice = 100 };
        var dbSet = new Mock<DbSet<Product>>();
        dbSet.Setup(d => d.Find(It.IsAny<object[]>())).Returns(product);
        _mockContext.Setup(c => c.Products).Returns(dbSet.Object);
        _command.Id = 1;

        // Act
        _command.Handle();

        // Assert
        dbSet.Verify(d => d.Remove(It.Is<Product>(p => p.Id == product.Id)), Moq.Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Moq.Times.Once);
    }
}
