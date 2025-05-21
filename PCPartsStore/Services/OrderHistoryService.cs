using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Paging;
using PCPartsStore.Services.Interfaces;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Services;

public class OrderHistoryService : IOrderHistoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public OrderHistoryService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public PaginatedList<OrderHistoryViewModel> OrderPage(int? page)
    {
        var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        var orders = _dbContext.Orders.Where(i => i.UserId == userId).ToList();
        var orderDetails = orders.Select(i => i.OrderDetails).ToJson();
        var totalPrices = orders.Select(i => i.TotalPrice).ToList();
        var orderDates = orders.Select(i => i.OrderDate).ToList();
        var orderIds = orders.Select(i => i.Id).ToList();
        var i = 0;
        var productId = 0;
        var parsedOuterArray = JArray.Parse(orderDetails);
        var productName = string.Empty;
        var products = new List<OrderHistoryViewModel>();
        foreach (var item in parsedOuterArray)
        {
            productName = string.Empty;
            var parsedInnerArray = JArray.Parse(item.ToString());
            foreach (var item2 in parsedInnerArray)
            {
                int id = (int)item2["ProductId"]!;
                if (parsedInnerArray.Count > 1)
                {
                    Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
                    if (product == null)
                    {
                        continue;
                    }
                    productName += product!.Name + " + ";
                }
                else
                {
                    Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
                    if (product == null)
                    {
                        continue;
                    }
                    productName += product!.Name;
                }
                
                productId = id;
            }

            if (parsedInnerArray.Count > 1)
            {
                productName = productName.Substring(0, productName.Length - 3);
                products.Add(new OrderHistoryViewModel
                {
                    OrderId = orderIds[i], ProductId = productId, Name = productName, OrderDate = orderDates[i],
                    TotalPrice = totalPrices[i]
                });
            }
            else
            {
                products.Add(new OrderHistoryViewModel
                {
                    OrderId = orderIds[i], ProductId = productId, Name = productName, OrderDate = orderDates[i],
                    TotalPrice = totalPrices[i]
                });
            }

            i++;
        }

        var paginatedList = PaginatedList<OrderHistoryViewModel>.Create(products, page ?? 1, 8);
        return paginatedList;
    }

    public OrderHistoryDetailsViewModel OrderDetailsPage(int id)
    {
        var order = _dbContext.Orders.FirstOrDefault(i => i.Id == id);
        var orderDetails = JArray.Parse(order.OrderDetails);
        var products = new List<Product>();
        var productQunatities = new List<int>();
        var productPrices = new List<decimal?>();
        foreach (var item in orderDetails)
        {
            var innerParsedJObject = JObject.Parse(item.ToString());
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == (int)innerParsedJObject["ProductId"]);
            product.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == product.ProductImageId);
            products.Add(product);
            productQunatities.Add((int)innerParsedJObject["Quantity"]);
            productPrices.Add(_dbContext.Products.FirstOrDefault(p => p.Id == (int)innerParsedJObject["ProductId"])
                .Price);
        }

        var model = new OrderHistoryDetailsViewModel
        {
            DeliveryAddress = order.DeliveryAddress,
            Recipient = order.Recipient,
            PhoneNumber = order.PhoneNumber,
            OrderDate = order.OrderDate,
            Products = products,
            ProductQuantities = productQunatities,
            ProductPrices = productPrices
        };
        return model;
    }
}