namespace WebAPI.DataAccess.Entities;

public class Basket
{
    public Guid Id { get; set; }
    public string Customer { get; set; }
    public bool PaysVAT { get; set; }
    public bool IsClosed { get; set; }
    public bool IsPaid { get; set; }
}