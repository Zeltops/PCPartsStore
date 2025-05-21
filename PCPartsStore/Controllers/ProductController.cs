using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Paging;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _dbContext;


    public ProductController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository,
        IProductService productService, ApplicationDbContext dbContext)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productService = productService;
        _dbContext = dbContext;
    }

    [Route("Product/Index")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("Product/ProductCategories")]
    public IActionResult ProductCategories()
    {
        return View();
    }


    [Route("Product/Products/{productCategory}")]
    public IActionResult Products(string productCategory, int? page)
    {

        ProductCategory? category = _dbContext.ProductsCategory.FirstOrDefault(p => p.Name == productCategory);

        if (category == null)
        {
            return NotFound();
        }
        
        var products = _productService.GetProductsByCategory(category.Id);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            ViewData["ProductCategory"] = productCategory;
            return View(paginatedList);
        }

        return View();
    }


    [Route("Product/Categories")]
    public IActionResult Categories(int productId)
    {
        return View();
    }


    [Route("Product/Add")]
    [Authorize(Roles = "Admin")]
    public IActionResult Add()
    {
        var categories = _productCategoryRepository.GetProductCategories();
        ViewData["ProductCategories"] = new SelectList(categories, "Id", "Name");
        return View();
    }

    [Route("/Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Add(string product)
    {
        if (product == "Cpu")
        {
            ViewData["AddText"] = "Add a new CPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 1).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }
        else if (product == "Gpu")
        {
            ViewData["AddText"] = "Add a new GPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 2).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Ram")
        {
            ViewData["AddText"] = "Add a new RAM product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 3).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Motherboard")
        {
            ViewData["AddText"] = "Add a new Motherboard product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 4).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }
        
        else if (product == "Other")
        {
            ViewData["AddText"] = "Add a new product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 5).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("/Product/Add")]
    [Route("Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(Product model)
    {
        var productsCategory = _dbContext.ProductsCategory.ToList();
        ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
        if (ModelState.IsValid)
        {
            if (_dbContext.Products.Any(id => id.Id == model.Id) == true)
            {
                ModelState.AddModelError("Id",
                    $"Product with id code - \"{model.Id}\" already exists in the database!");
                return View();
            }
            else
            {
                if (await _productService.UploadImage(model.Image, null) == false)
                {
                    ModelState.AddModelError("Image",
                        $"Image with the name - \"{model.Image.FileName}\" already exists in the database!");
                    return View();
                }
                else
                {
                    TempData["ShowProductSuccessfullyAddedToDb"] = true;
                    TempData["ProductId"] = model.Id;
                    await _productService.Add(model);
                    if (model.ProductCategoryId == 1)
                    {
                        return RedirectToAction("Cpu");
                    }
                    else if (model.ProductCategoryId == 2)
                    {
                        return RedirectToAction("Gpu");
                    }
                    else if (model.ProductCategoryId == 3)
                    {
                        return RedirectToAction("Ram");
                    }
                    else if (model.ProductCategoryId == 4)
                    {
                        return RedirectToAction("Motherboard");
                    }
                    else if (model.ProductCategoryId == 5)
                    {
                        return RedirectToAction("Other");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        return View();
    }

    public IActionResult Cpu(int? page)
    {
        var products = _productService.GetProductsByCategory(1);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Cpu/{id}")]
    public IActionResult Cpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 1);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Gpu(int? page)
    {
        var products = _productService.GetProductsByCategory(2);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Gpu/{id}")]
    public IActionResult Gpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 2);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Ram(int? page)
    {
        var products = _productService.GetProductsByCategory(3);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Ram/{id}")]
    public IActionResult Ram(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 3);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Motherboard(int? page)
    {
        var products = _productService.GetProductsByCategory(4);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Motherboard/{id}")]
    public IActionResult Motherboard(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 4);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }
    
    
    public IActionResult Other(int? page)
    {
        var products = _productService.GetProductsByCategory(5);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Other/{id}")]
    public IActionResult Other(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 5);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    [Route("/Product/Edit/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var productsCategory = _dbContext.ProductsCategory.ToList();
        ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
        if (TempData["model"] != null)
        {
            var oldImageName = TempData["oldImageName"];
            ModelState.AddModelError("Image",
                $"Image with the name - \"{oldImageName}\" already exists in the database!");
            var model = JsonConvert.DeserializeObject<Product>((string)TempData["model"]);
            TempData.Remove("model");
            return View(model);
        }

        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            product.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == product.ProductImageId);
            return View(product);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPatch]
    [Route("/Product/Edit/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string formData, IFormFile image, int id)
    {
        var data = HttpUtility.ParseQueryString(formData);
        var model = new Product()
        {
            Id = Convert.ToInt32(data["Id"]),
            Name = data["Name"],
            Description = data["Description"],
            Price = Convert.ToDecimal(data["Price"]),
            ProductCategoryId = Convert.ToInt32(data["ProductCategoryId"]),
            Quantity = Convert.ToInt32(data["Quantity"]),
        };
        //keep the existing image if the user hasn't uploaded a new one
        if (image == null)
        {
            model.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i =>
                i.Id == _dbContext.Products.FirstOrDefault(p => p.Id == id).ProductImageId);
            ModelState.Remove("Image");
        }

        if (ModelState.IsValid)
        {
            if (await _productService.Edit(model, image, id) == false)
            {
                model.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Name == image.FileName);
                Response.StatusCode = 400;
                TempData["oldImageName"] = image.FileName;
                TempData["model"] = JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Json(Url.Action("Edit", "Product", new { id = id }));
            }

            TempData["ShowProductSuccessfullyEdited"] = true;
            TempData["ProductId"] = model.Id;
            if (model.ProductCategoryId == 1)
            {
                return Json(Url.Action("Cpu"));
            }
            else if (model.ProductCategoryId == 2)
            {
                return Json(Url.Action("Gpu"));
            }
            else if (model.ProductCategoryId == 3)
            {
                return Json(Url.Action("Ram"));
            }
            else if (model.ProductCategoryId == 4)
            {
                return Json(Url.Action("Motherboard"));
            }
            else if (model.ProductCategoryId == 5)
            {
                return Json(Url.Action("Other"));
            }
        }

        return View(model);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var productCategoryId = _dbContext.Products.FirstOrDefault(p => p.Id == id).ProductCategoryId;
        if (await _productService.Delete(id) == true)
        {
            TempData["ShowProductSuccessfullyDeleted"] = true;
            TempData["ProductId"] = id;
            if (productCategoryId == 1)
            {
                return Json(Url.Action("Cpu"));
            }
            else if (productCategoryId == 2)
            {
                return Json(Url.Action("Gpu"));
            }
            else if (productCategoryId == 3)
            {
                return Json(Url.Action("Ram"));
            }
            else if (productCategoryId == 4)
            {
                return Json(Url.Action("Motherboard"));
            }
            else if (productCategoryId == 5)
            {
                return Json(Url.Action("Other"));
            }
        }

        return BadRequest();
    }
}