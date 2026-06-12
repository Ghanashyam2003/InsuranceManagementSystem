using Insurance.Application.DTOs;
using Insurance.Domain.Models;

namespace Insurance.Application.Interface
{
    public interface ICustomerNomineeService
    {
        Task<CustomerNominee> AddNomineeAsync(int customerId, NomineeDto dto, int createdBy);
        Task<List<CustomerNominee>> GetNomineesByCustomerIdAsync(int customerId);
        Task<CustomerNominee> UpdateNomineeAsync(int nomineeId, NomineeDto dto, int modifiedBy);
        Task DeleteNomineeAsync(int nomineeId);
    }
}