using PCPartsStore.Entities;

namespace PCPartsStore.ViewModels;

public class OrderHistoryDetailsViewModel
{
    public string DeliveryAddress { get; set; }
    public string Recipient { get; set; }
    public string PhoneNumber { get; set; }
    public string OrderDate { get; set; }
    public List<Product> Products { get; set; }
    public List<int> ProductQuantities { get; set; }
    
    public List<decimal?> ProductPrices { get; set; }
}