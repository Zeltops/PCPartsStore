using Microsoft.AspNetCore.Http.HttpResults;

namespace PCPartsStore.Paging;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; }

    public int TotalPages { get; }

    public int CurrentRange { get; private set; }

    public int InitialRange { get; set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int count, int pageIndex, int productsPerPage)
    {
        this.PageIndex = pageIndex;
        this.TotalPages = (int)Math.Ceiling(count / (double)productsPerPage);
        this.InitialRange = 10;
        this.CurrentRange = (PageIndex - 1) / InitialRange * InitialRange + InitialRange;

        AddRange(items);
    }


    public static PaginatedList<T> Create(List<T> source, int pageIndex, int productsPerPage)
    {
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * productsPerPage).Take(productsPerPage).ToList();

        return new PaginatedList<T>(items, count, pageIndex, productsPerPage);
    }
}