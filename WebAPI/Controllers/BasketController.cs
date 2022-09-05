using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    private readonly IBasketService service;
    private readonly ILogger<BasketsController> logger;
    public BasketsController(IBasketService service, ILogger<BasketsController> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBasket(CreateBasketRequest request)
    {
        try
        {
            var basketId = await service.CreateBasket(request);
            return Ok(basketId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id}/article-line")]
    public async Task<IActionResult> AddItem([FromRoute] Guid id, BasketItemModel item)
    {
        try
        {
            var result = await service.AddItem(id, item);
            if (result)
            {
                return Ok("Added item successfully");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Failed to add item to the basket");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var item = await service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PayAndCloseBasket([FromRoute] Guid id, bool close, bool paid)
    {
        try
        {
            var result = await service.UpdateBasket(id, close, paid);
            if (result)
            {
                return Ok("Paid and closed successfully");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Failed to closed the basket");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}