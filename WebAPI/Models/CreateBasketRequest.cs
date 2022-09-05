namespace WebAPI.Models;

public class CreateBasketRequest
{
    public string Customer { get; set; }
    public bool PaysVAT { get; set; }
}