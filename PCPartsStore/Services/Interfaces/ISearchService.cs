using PCPartsStore.Entities;
using PCPartsStore.Paging;

namespace PCPartsStore.Services.Interfaces;

public interface ISearchService
{
    Task<PaginatedList<Product>> Search(string searchString, int? page);

}