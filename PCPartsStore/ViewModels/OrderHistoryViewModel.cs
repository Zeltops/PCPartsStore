namespace PCPartsStore.ViewModels;

public class OrderHistoryViewModel
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string OrderDate { get; set; }
    public decimal? TotalPrice { get; set; }
}