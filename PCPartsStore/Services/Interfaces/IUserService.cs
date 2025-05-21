using System.Security.Claims;
using PCPartsStore.Entities;

namespace PCPartsStore.Services.Interfaces;

public interface IUserService
{
    public void AddAddress(Address model, ClaimsPrincipal user);
    public void EditAddress(Address model);
    public void DeleteAddress(int id);
}