using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class ProductImageRepository : IProductImageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void AddProductImage(ProductImage productImage)
    {
        _dbContext.Add(productImage);
        _dbContext.SaveChanges();
    }

    public List<ProductImage> GetProductImages()
    {
        return _dbContext.ProductsImages.ToList();
    }

    public ProductImage? GetProductImageById(int? productId)
    {
        return _dbContext.ProductsImages.FirstOrDefault(p => p.Id == productId);
    }

    public ProductImage? GetProductImageByName(string fileName)
    {
        return _dbContext.ProductsImages.FirstOrDefault(p => p.Name == fileName);
    }

    public bool ProductImageExists(string fileName)
    {
        return _dbContext.ProductsImages.Any(p => p.Name == fileName);
    }

    public async Task UpdateProductImage(ProductImage productImage)
    {
        _dbContext.Entry(productImage).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductImage(ProductImage productImage)
    {
        _dbContext.ProductsImages.Remove(productImage);
        await _dbContext.SaveChangesAsync();
    }
}