namespace PCPartsStore.Services.Interfaces;

public interface ICartService
{
    void AddToCart(int id);
    void SubtractQuantity(int id);
    void AddQuantity(int id);
    void RemoveProduct(int id);
    void PlaceOrder(int addressId);
}