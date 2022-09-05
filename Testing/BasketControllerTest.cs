using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;

namespace Testing;

public class BasketControllerTest
{
    private Guid basketId = new Guid("5640fa66-8a4f-439e-8ccc-14502584f305");
    private readonly Mock<IBasketService> mockBasketService;
    private readonly BasketsController controller;
    private readonly BasketItemModel basketItemModel;
    private const string CREATEBASKETFAILED = "Failed to create the basket";

    public BasketControllerTest()
    {
        mockBasketService = new Mock<IBasketService>();
        var logger = Mock.Of<ILogger<BasketsController>>();
        controller = new BasketsController(mockBasketService.Object, logger);
        basketItemModel = new BasketItemModel
        {
            Name = "Test Item",
            Price = 20
        };
    }

    [Fact]
    public async Task CreateBasketSuccessfully()
    {
        // Arrange
        CreateBasketRequest request = GetCreateBasketRequest();
        mockBasketService.Setup(service => service.CreateBasket(request))
            .ReturnsAsync(basketId);

        // Act
        var actual = await controller.CreateBasket(request);
        var okResult = actual as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult?.StatusCode);
        Assert.Equal(basketId, okResult?.Value);
    }

    [Fact]
    public async Task CreateBasketFailed()
    {
        // Arrange
        CreateBasketRequest request = GetCreateBasketRequest();
        mockBasketService.Setup(service => service.CreateBasket(request))
            .ThrowsAsync(new Exception(CREATEBASKETFAILED));

        // Act
        var actual = await controller.CreateBasket(request);
        var okResult = actual as ObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(500, okResult?.StatusCode);
        Assert.Equal(CREATEBASKETFAILED, okResult?.Value);
    }

    [Fact]
    public async Task AddItemToBasketSuccessfully()
    {
        // Arrange
        CreateBasketRequest request = GetCreateBasketRequest();
        mockBasketService.Setup(service => service.AddItem(basketId, basketItemModel))
            .ReturnsAsync(true);

        // Act
        var actual = await controller.AddItem(basketId, basketItemModel);
        var okResult = actual as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult?.StatusCode);
        Assert.Equal("Added item successfully", okResult?.Value);
    }

    #region Prepare
    private CreateBasketRequest GetCreateBasketRequest()
    {
        return new CreateBasketRequest
        {
            Customer = "Test",
            PaysVAT = true
        };
    }
    #endregion
}