using Insurance.Application.DTOs;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface ICustomerAddressService
    {
        Task<CustomerAddress> AddAddressAsync(int customerId, AddressDto dto, int createdBy);
        Task<List<CustomerAddress>> GetAddressesByCustomerIdAsync(int customerId);
        Task<CustomerAddress> UpdateAddressAsync(int addressId, AddressDto dto, int modifiedBy);
        Task DeleteAddressAsync(int addressId);
    }
}