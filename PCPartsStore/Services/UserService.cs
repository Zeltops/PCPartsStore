using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAddressRepository _addressRepository;

    public UserService(UserManager<IdentityUser> userManager, IAddressRepository addressRepository,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _addressRepository = addressRepository;
    }

    public void AddAddress(Address model, ClaimsPrincipal user)
    {
        var lastRecordId = _addressRepository.GetLastAddressId();
        model.Id = lastRecordId + 1;

        var userId = _userManager.GetUserId(user);
        model.UserId = userId;

        _addressRepository.AddAddress(model);
    }

    public void EditAddress(Address model)
    {
        var entity = _addressRepository.GetAddressById(model.Id);
        entity.ShortName = model.ShortName;
        entity.Recipient = model.Recipient;
        entity.City = model.City;
        entity.Street = model.Street;

        _addressRepository.UpdateAddress(entity);
    }

    public void DeleteAddress(int id)
    {
        Address? address = _addressRepository.GetAddressById(id);
        _addressRepository.DeleteAddress(address);
    }
}