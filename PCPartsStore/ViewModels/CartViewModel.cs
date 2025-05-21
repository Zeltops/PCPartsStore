using Microsoft.EntityFrameworkCore;
using PCPartsStore.Entities;

namespace PCPartsStore.ViewModels;

public class CartViewModel
{
    public int Quantity { get; set; }
    public Product Product { get; set; }

    [Precision(6, 2)]
    public decimal? InitialPrice { get; set; }

    [Precision(6, 2)]
    public decimal? Price { get; set; }

}