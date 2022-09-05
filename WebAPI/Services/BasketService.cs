using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DataAccess.Entities;
using WebAPI.Models;

namespace WebAPI.Services;
public interface IBasketService
{
    Task<Guid> CreateBasket(CreateBasketRequest request);
    Task<bool> AddItem(Guid basketId, BasketItemModel item);
    Task<BasketModel?> GetById(Guid id);
    Task<bool> UpdateBasket(Guid basketId, bool close, bool paid);
}

public class BasketService : IBasketService
{
    private readonly DataContext context;
    private readonly ILogger<BasketService> logger;
    public BasketService(DataContext context, ILogger<BasketService> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    public async Task<Guid> CreateBasket(CreateBasketRequest request)
    {
        try
        {
            var basket = new Basket
            {
                Id = Guid.NewGuid(),
                Customer = request.Customer,
                PaysVAT = request.PaysVAT,
                IsClosed = false,
                IsPaid = false
            };
            await context.Baskets.AddAsync(basket);
            await context.SaveChangesAsync();
            return basket.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Guid.Empty;
        }
    }

    public async Task<bool> AddItem(Guid basketId, BasketItemModel item)
    {
        try
        {
            var basketItem = new BasketItem
            {
                Id = Guid.NewGuid(),
                BasketId = basketId,
                Name = item.Name,
                Price = item.Price
            };

            await context.BasketItems.AddAsync(basketItem);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<BasketModel?> GetById(Guid id)
    {
        try
        {
            var basket = await context.Baskets.FirstOrDefaultAsync(x => x.Id == id && (!x.IsClosed || !x.IsPaid));

            if (basket == null) return null;

            var basketItems = context.BasketItems
                .Where(x => x.BasketId == id)
                .Select(item => new BasketItemModel
                {
                    Name = item.Name,
                    Price = item.Price
                }).ToList();
            var basketModel = new BasketModel
            {
                Id = basket.Id,
                Customer = basket.Customer,
                PaysVAT = basket.PaysVAT,
                Items = basketItems
            };
            return basketModel;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<bool> UpdateBasket(Guid basketId, bool close, bool paid)
    {
        try
        {
            var basket = await context.Baskets.FindAsync(basketId);

            if (basket == null) return false;

            basket.IsClosed = close;
            basket.IsPaid = paid;

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }
    }
}