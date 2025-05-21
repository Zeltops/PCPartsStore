using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AddressRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddAddress(Address address)
    {
        _dbContext.UserAddress.Add(address);
        _dbContext.SaveChanges();
    }

    public List<Address> GetAddresses()
    {
        return _dbContext.UserAddress.ToList();
    }


    public Address? GetAddressById(int id)
    {
        return _dbContext.UserAddress.FirstOrDefault(x => x.Id == id);
    }

    public int GetLastAddressId()
    {
        return _dbContext.UserAddress.Max(a => (int?)a.Id) ?? 0;
    }

    public List<Address> GetAddressesByUserId(string userId)
    {
        return _dbContext.UserAddress.Where(x => x.User.Id == userId).ToList();
    }

    public void UpdateAddress(Address address)
    {
        _dbContext.Entry(address).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void DeleteAddress(Address address)
    {
        _dbContext.UserAddress.Remove(address);
        _dbContext.SaveChanges();
    }
}