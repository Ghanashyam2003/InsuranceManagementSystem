using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Insurance.Application.Services
{
    // This service handles all the core business logic for managing Customers
    public class CustomerService : ICustomerService
    {
        // Database, email sender, and in-memory cache dependencies
        private readonly ApplicationDbContext db;
        private readonly IEmailService email;
        private readonly IMemoryCache cache;

        // Constructor: .NET automatically injects these dependencies for us
        public CustomerService(ApplicationDbContext db, IEmailService email, IMemoryCache cache)
        {
            this.db = db;
            this.email = email;
            this.cache = cache;
        }

        // Called when an Agent creates an account on behalf of a new customer
        public async Task<Customer> CreateByAgentAsync(CustomerRegisterDto dto, int agentId)
        {
            // First, ensure the email isn't already taken in our system
            await EnsureEmailFree(dto.Email);

            // Create the login (Auth) credentials and save them
            var auth = CreateAuth(dto.Email, dto.Password);
            db.Auths.Add(auth);
            await db.SaveChangesAsync();

            // Build the actual customer profile, linking it to both the login and the agent
            var customer = BuildCustomer(dto, auth.AuthId, agentId, "Agent");
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            // Fire off a welcome email with their credentials (runs safely in the background)
            _ = email.SendCredentialsAsync(dto.Email, dto.Password, "Customer");
            return customer;
        }

        // Called when a user registers themselves directly on the website
        public async Task<Customer> SelfRegisterAsync(CustomerRegisterDto dto)
        {
            await EnsureEmailFree(dto.Email);
            var auth = CreateAuth(dto.Email, dto.Password);
            db.Auths.Add(auth); await db.SaveChangesAsync();

            // Similar to above, but without an Agent ID attached
            var customer = BuildCustomer(dto, auth.AuthId, null, "Self");
            db.Customers.Add(customer); await db.SaveChangesAsync();
            return customer;
        }

        // PAGINATION IMPLEMENTATION: Grabs a specific "chunk" of customers so we don't crash the UI with thousands of records
        public async Task<PagedResult<Customer>> GetAllAsync(int page, int pageSize, bool? isActive = null)
        {
            var query = db.Customers.AsQueryable();

            // Filter by active/inactive if requested by the frontend
            if (isActive.HasValue)
                query = query.Where(c => c.IsDeleted == isActive.Value);

            // Always show the newest customers at the top
            query = query.OrderByDescending(c => c.CreatedAt);

            // Count total records so the UI knows how many pages exist
            var total = await query.CountAsync();

            // .Skip() bypasses previous pages, .Take() grabs exactly the amount needed for this current page
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Customer> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
        }

        // CACHING IMPLEMENTATION: Fetches a single customer, prioritizing super-fast RAM storage
        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            // Unique key to find this specific customer in the cache
            string cacheKey = $"Customer{customerId}";

            // If the customer is already in RAM, return them instantly without hitting the database!
            if (cache.TryGetValue(cacheKey, out Customer? cachedCustomer)) return cachedCustomer;

            // Otherwise, query the database (ensuring they are an active user)
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.IsDeleted);

            // Save the result to RAM for 10 minutes to speed up their next request
            if (customer != null) cache.Set(cacheKey, customer, TimeSpan.FromMinutes(10));

            return customer;
        }

        // Finds a customer profile based strictly on their secure Auth/Login ID
        public async Task<Customer?> GetByAuthIdAsync(int authId)
            => await db.Customers.FirstOrDefaultAsync(c => c.AuthId == authId && c.IsDeleted);

        // Updates the customer's personal details
        public async Task<Customer> UpdateProfileAsync(int customerId, CustomerProfileUpdateDto dto, string modifiedBy)
        {
            var customer = await db.Customers.FindAsync(customerId) ?? throw new KeyNotFoundException("Customer not found.");
            if (!customer.IsDeleted) throw new KeyNotFoundException("Customer account is inactive.");

            // Apply the new data
            customer.FirstName = dto.FirstName; customer.LastName = dto.LastName; customer.MobileNumber = dto.MobileNumber;
            customer.DOB = dto.DOB; customer.Gender = dto.Gender; customer.PANNumber = dto.PanNumber;
            customer.AadharNumber = dto.AadhaarNumber; customer.ModifiedAt = DateTime.UtcNow; customer.ModifiedBy = modifiedBy;

            await db.SaveChangesAsync();

            // CACHE INVALIDATION: Wipe the old data from RAM so the user immediately sees their updates
            cache.Remove($"Customer{customerId}");
            return customer;
        }

        // Soft Delete: Doesn't actually erase the data, just hides it by marking IsActive = false
        public async Task DeleteAsync(int customerId, string deletedBy)
        {
            var customer = await db.Customers.FindAsync(customerId) ?? throw new KeyNotFoundException("Customer not found.");

            // Mark the customer and their login as inactive
            customer.IsDeleted = false; customer.ModifiedAt = DateTime.UtcNow; customer.ModifiedBy = deletedBy;
            var auth = await db.Auths.FindAsync(customer.AuthId);
            if (auth != null) auth.IsActive = false;

            await db.SaveChangesAsync();
            cache.Remove($"Customer{customerId}"); // Clear from cache so they disappear from the UI
        }

        // Hard Delete: Completely and permanently erases the user and their login from the database
        public async Task HardDeleteAsync(int customerId)
        {
            var customer = await db.Customers.FindAsync(customerId) ?? throw new KeyNotFoundException("Customer not found.");

            // Remove login record first
            var auth = await db.Auths.FindAsync(customer.AuthId);
            if (auth != null) db.Auths.Remove(auth);

            // Remove customer record
            db.Customers.Remove(customer);

            await db.SaveChangesAsync();
            cache.Remove($"Customer{customerId}"); // Clear from cache
        }

        // Helper method: Prevents duplicate accounts by throwing an error if the email is already in use
        private async Task EnsureEmailFree(string email)
        {
            if (await db.Auths.AnyAsync(a => a.Email == email)) throw new InvalidOperationException("Email already registered.");
        }

        // Helper method: Builds the baseline Auth (login) object and assigns them the 'Customer' role (RoleId 3)
        private static Auth CreateAuth(string email, string password) => new()
        { Email = email, Password = password, RoleId = 3, IsActive = true };

        // Helper method: Maps DTO data to the Domain model and generates a random, unique CUSTXXXXXX code
        private static Customer BuildCustomer(CustomerRegisterDto dto, int authId, int? agentId, string createdBy) => new()
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MobileNumber = dto.MobileNumber,
            PANNumber = dto.PanNumber,
            AadharNumber = dto.AadhaarNumber,
            AgentId = agentId,
            AuthId = authId,
            IsDeleted = false,
            CustomerCode = $"CUST{Guid.NewGuid().ToString()[..6].ToUpper()}",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }
}