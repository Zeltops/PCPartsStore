using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }

    public List<Product> GetProducts()
    {
        return _dbContext.Products.ToList();
    }

    public bool ContainsProductWithId(int? id)
    {
        return _dbContext.Products.Any(p => p.Id == id);
    }

    public List<Product> GetLatestProducts(int count)
    {
        return _dbContext.Products
            .OrderByDescending(p => p.Id)
            .Take(count)
            .ToList();
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        return _dbContext.Products.Where(p => p.ProductCategory.Id == categoryId).ToList();
    }

    public Product? GetProductById(int id)
    {
        return _dbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public void UpdateProduct(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
    }
}