using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Entities;
using PCPartsStore.Models;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Controllers;

[Authorize(Policy = "FirstTimeSetupComplete")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly ISearchService _searchService;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository,
        IProductImageRepository productImageRepository, IProductCategoryRepository productCategoryRepository, ISearchService searchService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _productCategoryRepository = productCategoryRepository;
        _searchService = searchService;
    }

    public async Task<IActionResult> Index()
    {
        var latestProducts = _productRepository.GetLatestProducts(5).ToList();

        ViewData["actions"] = _productCategoryRepository.GetProductCategories()
            .Select(pc => pc.Name)
            .ToList();

        foreach (var product in latestProducts)
        {
            product.ProductImage = _productImageRepository.GetProductImageById(product.ProductImageId);
        }

        return View(latestProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Search(string searchString, int? page)
    {
        ViewData["actions"] = new List<string>() { "Cpu", "Gpu", "Ram", "Motherboard", "Other" };
        var paginatedList = await _searchService.Search(searchString, page);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View("../Search/Search", paginatedList);
        }
    }
}