using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Data;
using PCPartsStore.Extensions;
using PCPartsStore.Services.Interfaces;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(ApplicationDbContext dbContext, ICartService cartService,
        UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _cartService = cartService;
        _userManager = userManager;
    }

    [Authorize]
    [Route("/Cart")]
    public IActionResult Cart()
    {
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);

        return View(model);
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var redirectLink = Request.GetTypedHeaders().Referer.ToString();
        if (User.Identity.IsAuthenticated == false)
        {
            redirectLink = Url.Action("Login", "Account",
                new { returnUrl = Request.GetTypedHeaders().Referer.AbsolutePath });
            return Json(new { url = redirectLink });
        }

        _cartService.AddToCart(id);
        return Json(new { url = redirectLink });
    }

    [HttpPost]
    public IActionResult SubtractProductQuantity(int id)
    {
        _cartService.SubtractQuantity(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);

        return PartialView("Cart", model);
    }

    [HttpPost]
    public IActionResult AddProductQuantity(int id)
    {
        _cartService.AddQuantity(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);

        return PartialView("Cart", model);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveProductFromCart(int id)
    {
        _cartService.RemoveProduct(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);

        var partialView = await this.RenderViewAsync("Cart", model, true);
        return Json(new { PartialView = partialView, Quantity = model.Sum(i => i.Quantity) });
    }

    [Authorize]
    [Route("/Cart/Checkout")]
    public IActionResult Checkout()
    {
        if (HttpContext.User.Identity.IsAuthenticated &&
            HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart").Count == 0)
            return RedirectToAction("Cart");

        var model = new CheckoutViewModel
        {
            UserAddress = _dbContext.UserAddress.Where(i => i.UserId == _userManager.GetUserId(User)).ToList(),
            Cart = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart")
        };
        foreach (var entity in model.Cart)
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        return View(model);
    }

    [Route("/Cart/Checkout")]
    [HttpPost]
    public IActionResult Checkout(int addressId)
    {
        var address = _dbContext.UserAddress.FirstOrDefault(a => a.Id == addressId);
        var userId = _userManager.GetUserId(User);
        if (address == null)
        {
            return Redirect("/Account/User/Address/New");
        }
        if (address.UserId != userId) return Unauthorized();
        _cartService.PlaceOrder(addressId);
       
        return RedirectToAction("OrderPlaced");
    }

    [Authorize]
    [Route("/Cart/Order/Successful")]
    public IActionResult OrderPlaced()
    {
        if (HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart").Count == 0)
            return RedirectToAction("Cart");
        HttpContext.Response.Cookies.Delete("SessionData");
        return View();
    }
}