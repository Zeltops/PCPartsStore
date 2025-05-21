using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IOrderRepository
{
    void AddOrder(Order order);
    
    List<Order> GetOrders();
    
    List<Order> GetOrdersByCustomerId(string customerId);
    
    Order? GetOrderId(int id);
    
    void UpdateOrder(Order order);
    
    void DeleteOrder(Order order);
}