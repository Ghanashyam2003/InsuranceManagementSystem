using Insurance.Application.DTO.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface IClaimSettlementRepo
    {
        Task<List<ClaimSettlementDTO>> GetAll();

        Task<ClaimSettlementDTO> GetById(int id);

        Task<string> Add(ClaimSettlementCreateDTO dto);

        Task<string> Update(int id, ClaimSettlementCreateDTO dto);
    }
}
