using Insurance.Application.DTOs;
using Insurance.Application.Interface;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Insurance.Application.Services
{
    // This service handles all the heavy lifting and business rules for Customer Nominees 
    public class CustomerNomineeService : ICustomerNomineeService
    {
        // Database connection and our fast in-memory cache tool
        private readonly ApplicationDbContext db;
        private readonly IMemoryCache cache;

        // Constructor: .NET automatically provides our database context and cache tool
        public CustomerNomineeService(ApplicationDbContext db, IMemoryCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        // Adds a new nominee to a customer's account safely
        public async Task<CustomerNominee> AddNomineeAsync(int customerId, NomineeDto dto, int createdBy)
        {
            // First, make sure the customer actually exists in our system
            if (!await db.Customers
                .AnyAsync(c => c.CustomerId == customerId))
                throw new KeyNotFoundException("Customer not found.");

            // Business Rule: Calculate what percentage of the policy is already claimed by other nominees
            var currentTotal = await db.CustomerNominees
                .Where(n => n.CustomerId == customerId)
                .SumAsync(n => n.SharePercentage);

            // Prevent the total share from going over 100%
            if (currentTotal + dto.SharePercentage > 100)
                throw new InvalidOperationException($"Adding this nominee exceeds 100% share. Current total: {currentTotal}%");

            // Prepare the new nominee record and save it to the database
            var nominee = new CustomerNominee
            {
                CustomerId = customerId,
                NomineeName = dto.NomineeName,
                Relation = dto.Relation,
                SharePercentage = dto.SharePercentage,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };

            db.CustomerNominees.Add(nominee);
            await db.SaveChangesAsync();

            // Clear the old cache so the next time we fetch nominees, it grabs the fresh data
            cache.Remove($"CustomerNominees{customerId}"); // CACHE INVALIDATION
            return nominee;
        }

        // CACHING IMPLEMENTATION: Fetches the list of nominees, prioritizing server RAM for speed
        public async Task<List<CustomerNominee>> GetNomineesByCustomerIdAsync(int customerId)
        {
            // Unique key to identify this specific customer's nominee list in the cache
            string cacheKey = $"CustomerNominees{customerId}";

            // Try to grab it from the fast RAM cache first. If it's there, return it immediately!
            if (cache.TryGetValue(cacheKey, out List<CustomerNominee>? cachedNominees)) return cachedNominees!;

            // If it wasn't in the cache, hit the database to get the records
            var nominees = await db.CustomerNominees
                .Where(n => n.CustomerId == customerId)
                .ToListAsync();

            // Store those records in the cache for 10 minutes to speed up future requests
            cache.Set(cacheKey, nominees, TimeSpan.FromMinutes(10));
            return nominees;
        }

        // Updates an existing nominee while protecting our 100% share rule
        public async Task<CustomerNominee> UpdateNomineeAsync(int nomineeId, NomineeDto dto, int modifiedBy)
        {
            // Find the nominee, or throw an error if they've already been deleted
            var nominee = await db.CustomerNominees.FindAsync(nomineeId) ??
                throw new KeyNotFoundException("Nominee not found.");

            // Calculate the total share of EVERYONE ELSE except the person we are currently updating
            var otherNomineesTotal = await db.CustomerNominees
                .Where(n => n.CustomerId == nominee.CustomerId && n.NomineeId != nomineeId)
                .SumAsync(n => n.SharePercentage);

            // Ensure this update doesn't push the overall total past 100%
            if (otherNomineesTotal + dto.SharePercentage > 100) throw new InvalidOperationException($"Updating this nominee exceeds 100% share. Available remaining share: {100 - otherNomineesTotal}%");

            // Apply the new values and save to the database
            nominee.NomineeName = dto.NomineeName;
            nominee.Relation = dto.Relation;
            nominee.SharePercentage = dto.SharePercentage;
            nominee.ModifiedBy = modifiedBy;
            nominee.ModifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            // Wipe the cache so the customer's dashboard reflects the updated info immediately
            cache.Remove($"CustomerNominees{nominee.CustomerId}");
            return nominee;
        }

        // Completely removes a nominee from the database
        public async Task DeleteNomineeAsync(int nomineeId)
        {
            // Find them, delete them, and save the changes
            var nominee = await db.CustomerNominees.FindAsync(nomineeId) ??
                throw new KeyNotFoundException("Nominee not found.");
            db.CustomerNominees.Remove(nominee); await db.SaveChangesAsync();

            // Wipe the cache so the deleted nominee disappears from the frontend instantly
            cache.Remove($"CustomerNominees{nominee.CustomerId}");
        }
    }
}