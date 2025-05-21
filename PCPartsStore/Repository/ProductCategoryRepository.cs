using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductCategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddProductCategory(ProductCategory productCategory)
    {
        _dbContext.ProductsCategory.Add(productCategory);
        _dbContext.SaveChanges();
    }

    public List<ProductCategory> GetProductCategories()
    {
        return _dbContext.ProductsCategory.ToList();
    }

    public ProductCategory? GetProductCategoryById(int id)
    {
        return _dbContext.ProductsCategory.FirstOrDefault(c => c.Id == id);
    }

    public void UpdateProductCategory(ProductCategory productCategory)
    {
        _dbContext.Entry(productCategory).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void DeleteProductCategory(ProductCategory productCategory)
    {
        _dbContext.ProductsCategory.Remove(productCategory);
        _dbContext.SaveChanges();
    }
}