using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Extensions;
using PCPartsStore.Services.Interfaces;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISession _session;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public CartService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
        SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _session = _httpContextAccessor.HttpContext.Session;
        if (_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User) &&
            _session.GetComplexData<HashSet<CartViewModel>>("Cart") == null)
            _session.SetComplexData("Cart", new HashSet<CartViewModel>());
    }

    public void AddToCart(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var cartItem = new CartViewModel
            { Product = product, Quantity = 1, Price = product.Price, InitialPrice = product.Price };
        if (cart.Any(i => i.Product.Id == cartItem.Product.Id))
        {
            var duplicateItem = cart.FirstOrDefault(i => i.Product.Id == cartItem.Product.Id);
            duplicateItem.InitialPrice = cartItem.InitialPrice;
            duplicateItem.Quantity += 1;
            duplicateItem.Price = cartItem.Price * duplicateItem.Quantity;
            cart.Add(duplicateItem);
        }
        else
        {
            cart.Add(cartItem);
        }

        _session.SetComplexData("Cart", cart);
    }

    public void SubtractQuantity(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        if (model.Quantity > 1)
        {
            var edittedItem = cart.FirstOrDefault(i => i.Product.Id == model.Product.Id);
            edittedItem.Quantity -= 1;
            edittedItem.Price = model.InitialPrice * edittedItem.Quantity;
            cart.Add(edittedItem);
        }
        else
        {
            cart.Remove(model);
        }

        _session.SetComplexData("Cart", cart);
    }

    public void AddQuantity(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        var edittedItem = cart.FirstOrDefault(i => i.Product.Id == model.Product.Id);
        edittedItem.Quantity += 1;
        edittedItem.Price = model.InitialPrice * edittedItem.Quantity;
        cart.Add(edittedItem);
        _session.SetComplexData("Cart", cart);
    }

    public void RemoveProduct(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        cart.Remove(model);
        _session.SetComplexData("Cart", cart);
    }

    public void PlaceOrder(int addressId)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var orderDetails = JsonConvert.SerializeObject(cart.Select(i => new
        {
            i.Quantity, ProductId = i.Product.Id, i.Price
        }));
        var addressModel = _dbContext.UserAddress.FirstOrDefault(i => i.Id == addressId);
        var order = new Order
        {
            OrderDetails = orderDetails,
            OrderDate = DateTime.Now.ToString("dd/MM/yyyy"),
            DeliveryAddress = addressModel.Street,
            Recipient = addressModel.Recipient,
            PhoneNumber = addressModel.PhoneNumber,
            TotalPrice = cart.Sum(i => i.Price),
            UserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User)
        };
        _dbContext.Orders.Add(order);
        
        foreach (CartViewModel cartViewModel in cart)
        {
            int? productId = cartViewModel.Product.Id;

            Product? product = _dbContext.Products
                .Include(p => p.ProductImage)
                .Include(p => p.ProductCategory)
                .FirstOrDefault(p => p.Id == productId);
            
            if (product is not null)
            {
                int newQuantity = product.Quantity - cartViewModel.Quantity;
                if (newQuantity < 0)
                {
                    newQuantity = 0;
                }

                product.Quantity = newQuantity;

                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
        }
        
        _dbContext.SaveChanges();
    }
}