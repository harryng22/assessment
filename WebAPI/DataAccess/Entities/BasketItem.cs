using System.ComponentModel.DataAnnotations;

namespace WebAPI.DataAccess.Entities;
public class BasketItem
{
    public Guid Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public Guid BasketId { get; set; }
}