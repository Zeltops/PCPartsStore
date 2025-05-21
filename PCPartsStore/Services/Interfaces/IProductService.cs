using PCPartsStore.Entities;

namespace PCPartsStore.Services.Interfaces;

public interface IProductService
{
    Task Add(Product product);
    Task<bool> UploadImage(IFormFile file, int? oldId);
    Task<bool> Delete(int id);
    Task<bool> Edit(Product product, IFormFile image, int id);
    List<Product> GetProductsByCategory(int categoryId);
}