using Insurance.Application.DTOs;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces
{
    public interface ICustomerService
    {
        // CREATE
        Task<Customer> CreateByAgentAsync(CustomerRegisterDto dto, int agentId);
        Task<Customer> SelfRegisterAsync(CustomerRegisterDto dto);

        // READ
        Task<PagedResult<Customer>> GetAllAsync(int page, int pageSize, bool? isActive = null);
        Task<Customer?> GetByIdAsync(int customerId);
        Task<Customer?> GetByAuthIdAsync(int authId);

        // UPDATE
        Task<Customer> UpdateProfileAsync(int customerId, CustomerProfileUpdateDto dto, string modifiedBy);

        // DELETE
        Task DeleteAsync(int customerId, string deletedBy);         // Admin → soft delete
        Task HardDeleteAsync(int customerId);                        // Customer → hard delete
    }
}