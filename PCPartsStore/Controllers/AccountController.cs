using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Entities;
using PCPartsStore.Models.Account;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAddressRepository _addressRepository;
    private readonly IOrderHistoryService _orderHistoryService;

    private readonly IUserService _userService;

    public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, IAddressRepository addressRepository, IUserService userService, IOrderHistoryService orderHistoryService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _addressRepository = addressRepository;
        _userService = userService;
        _orderHistoryService = orderHistoryService;
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            if (!_userManager.Users.Any())
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, ["Admin", "User"]);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            else
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }
        }

        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        if (_userManager.Users.Any() == false) return RedirectToAction("Register");

        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            var userFound = await _userManager.FindByEmailAsync(model.Email);
            if (userFound == null)
                ModelState.AddModelError("", "User not found!");
            else
                ModelState.AddModelError("", "Invalid Login attempt");
        }

        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("SessionData");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    [Route("Account/Admin")]
    public IActionResult AdminPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("Account/User")]
    public IActionResult UserPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("Account/User/Addresses")]
    public IActionResult Addresses()
    {
        var userId = _userManager.GetUserId(User);
        var addresses = _addressRepository.GetAddressesByUserId(userId);
        if (addresses.Count == 0)
            return RedirectToAction("AddAddress");

        return View(addresses);
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/New")]
    public IActionResult AddAddress()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/New")]
    public IActionResult AddAddress(Address address, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            _userService.AddAddress(address, User);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Addresses");
        }

        return View();
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/Edit")]
    public IActionResult EditAddress([FromForm] int id)
    {
        var model = _addressRepository.GetAddressById(id);
        return View(model);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("/Account/User/Address/Edit/{shortName}")]
    public IActionResult EditAddress(Address model, string shortName)
    {
        Address? address = _addressRepository.GetAddressById(model.Id);
        string? userId = _userManager.GetUserId(User);

        if (address.UserId != userId)
            return Unauthorized();


        if (ModelState.IsValid)
        {
            _userService.EditAddress(model);
            return RedirectToAction("Addresses");
        }

        return View("EditAddress", model);
    }


    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/Delete")]
    [HttpDelete]
    public IActionResult DeleteAddress(int id)
    {
        Address? address = _addressRepository.GetAddressById(id);
        string? userId = _userManager.GetUserId(User);

        if (address.UserId != userId)
            return Unauthorized();

        _userService.DeleteAddress(id);
        return Json(Url.Action("Addresses"));
    }

    [Authorize(Roles = "User")]
    [Route("Account/User/Orders")]
    public IActionResult Orders(int? page)
    {
        var paginatedList = _orderHistoryService.OrderPage(page);
        if (page > paginatedList.TotalPages) return NotFound();

        return View(paginatedList);
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Order/{id}")]
    public IActionResult OrderDetails(int id)
    {
        var model = _orderHistoryService.OrderDetailsPage(id);
        return View(model);
    }
}