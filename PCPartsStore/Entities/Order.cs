using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PCPartsStore.Entities;

public class Order
{
    
    public int Id { get; set; }
    
    public string OrderDetails { get; set; }

    public string OrderDate { get; set; }
    
    public string DeliveryAddress { get; set; }
    
    public string Recipient { get; set; }
    
    public string PhoneNumber { get; set; }

    [Precision(9,2)]
    public decimal? TotalPrice { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}