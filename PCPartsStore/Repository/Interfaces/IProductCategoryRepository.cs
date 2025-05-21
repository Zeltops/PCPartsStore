using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductCategoryRepository
{
    void AddProductCategory(ProductCategory productCategory);

    List<ProductCategory> GetProductCategories();

    ProductCategory? GetProductCategoryById(int id);

    void UpdateProductCategory(ProductCategory productCategory);

    void DeleteProductCategory(ProductCategory productCategory);
}