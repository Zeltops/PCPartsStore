using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddOrder(Order order)
    {
        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();
    }

    public List<Order> GetOrders()
    {
        return _dbContext.Orders.ToList();
    }

    public List<Order> GetOrdersByCustomerId(string customerId)
    {
        return _dbContext.Orders
            .Where(o => o.User.Id == customerId)
            .ToList();
    }

    public Order? GetOrderId(int id)
    {
        return _dbContext.Orders.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateOrder(Order order)
    {
        _dbContext.Entry(order).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        _dbContext.Orders.Remove(order);
        _dbContext.SaveChanges();
    }
}