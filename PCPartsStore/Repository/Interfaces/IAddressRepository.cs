using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IAddressRepository
{
    void AddAddress(Address address);
    
    List<Address> GetAddresses();
    
    Address? GetAddressById(int id);
    
   int GetLastAddressId();
    
   List<Address> GetAddressesByUserId(string id);
    
    void UpdateAddress(Address address);
    
    void DeleteAddress(Address address);
}