using Insurance.Application.DTO.Claim;
using Insurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface IClaimRepo 
    {
        Task<List<ClaimDTO>> GetClaims();

        Task<ClaimDTO> GetById(int id);

        Task<string> Add(ClaimDTO2 dto);

        Task<string> Update(int id, ClaimDTO2 dto);

        Task<string> Delete(int id);
        Task<List<ClaimDTO>> GetByPolicyId(int policyId);

    }
}
