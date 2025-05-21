using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Paging;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Services;

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _context;

    public SearchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Product>> Search(string searchString, int? page)
    {
        var products = await _context.Products.Where(p => p.Name.Contains(searchString)).ToListAsync();
        foreach (var product in products)
        {
            product.ProductImage = _context.ProductsImages.FirstOrDefault(i => i.Id == product.ProductImageId);
        }

        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        return paginatedList;
    }
}