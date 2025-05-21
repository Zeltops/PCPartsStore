using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductImageRepository
{
    void AddProductImage(ProductImage productImage);

    List<ProductImage> GetProductImages();
    
    ProductImage? GetProductImageById(int? productId);
    
    ProductImage? GetProductImageByName(string fileName);
    
    bool ProductImageExists(string fileName);

    Task UpdateProductImage(ProductImage productImage);

    Task DeleteProductImage(ProductImage productImage);
}