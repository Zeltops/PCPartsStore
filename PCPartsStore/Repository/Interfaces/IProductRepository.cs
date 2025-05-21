using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductRepository
{
    Task AddProduct(Product product);

    List<Product> GetProducts();
    
    bool ContainsProductWithId(int? id);
    
    List<Product> GetLatestProducts(int count);

   List<Product> GetProductsByCategory(int categoryId);

    Product? GetProductById(int id);

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);
}