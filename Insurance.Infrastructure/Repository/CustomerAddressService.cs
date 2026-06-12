using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Insurance.Application.Services
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ApplicationDbContext db;
        private readonly IMemoryCache cache;

        public CustomerAddressService(ApplicationDbContext db, IMemoryCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        public async Task<CustomerAddress> AddAddressAsync(int customerId, AddressDto dto, int createdBy)
        {
            if (!await db.Customers.AnyAsync(c => c.CustomerId == customerId))
                throw new KeyNotFoundException("Customer not found.");

            var address = new CustomerAddress
            {
                CustomerId = customerId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PinCode = dto.PinCode,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };
            db.CustomerAddresses.Add(address);
            await db.SaveChangesAsync();

            cache.Remove($"CustomerAddresses{customerId}"); // CACHE INVALIDATION
            return address;
        }

        // CACHING IMPLEMENTATION
        public async Task<List<CustomerAddress>> GetAddressesByCustomerIdAsync(int customerId)
        {
            string cacheKey = $"CustomerAddresses{customerId}";
            if (cache.TryGetValue(cacheKey, out List<CustomerAddress>? cachedAddresses)) return cachedAddresses!;

            var addresses = await db.CustomerAddresses
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
            cache.Set(cacheKey, addresses, TimeSpan.FromMinutes(10));
            return addresses;
        }

        public async Task<CustomerAddress> UpdateAddressAsync(int addressId, AddressDto dto, int modifiedBy)
        {
            var address = await db.CustomerAddresses.FindAsync(addressId) ?? throw new KeyNotFoundException("Address not found.");
            address.AddressLine1 = dto.AddressLine1;
            address.AddressLine2 = dto.AddressLine2;
            address.City = dto.City;
            address.State = dto.State;
            address.PinCode = dto.PinCode;
            address.ModifiedBy = modifiedBy;
            address.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            cache.Remove($"CustomerAddresses{address.CustomerId}");
            return address;
        }

        public async Task DeleteAddressAsync(int addressId)
        {
            var address = await db.CustomerAddresses.FindAsync(addressId) ?? throw new KeyNotFoundException("Address not found.");
            db.CustomerAddresses.Remove(address);
            await db.SaveChangesAsync();
            cache.Remove($"CustomerAddresses{address.CustomerId}");
        }
    }
}