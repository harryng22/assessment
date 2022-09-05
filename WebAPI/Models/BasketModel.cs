namespace WebAPI.Models;

public class BasketModel
{
    public Guid Id { get; set; }
    public string Customer { get; set; }
    public bool PaysVAT { get; set; }
    public List<BasketItemModel> Items { get; set; }
    public decimal TotalNet
    { 
        get {
            decimal totalNet = 0;
            foreach (var item in Items)
            {
                totalNet += item.Price;
            }
            return totalNet;
        }
    }
    public decimal TotalGross 
    { 
        get {
            decimal totalGross = 0;
            foreach (var item in Items)
            {
                totalGross += item.Price + item.Price * 10 / 100;
            }
            return totalGross;
        }
    }
}