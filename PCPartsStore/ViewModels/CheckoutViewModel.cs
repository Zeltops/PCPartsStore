using PCPartsStore.Entities;

namespace PCPartsStore.ViewModels;

public class CheckoutViewModel
{
    public List<Address> UserAddress { get; set; }
    public HashSet<CartViewModel> Cart { get; set; }
}