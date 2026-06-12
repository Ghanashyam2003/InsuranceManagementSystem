using Insurance.Application.DTO.Claim;
using Insurance.Application.DTO.ClaimInvestigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IClaimInvestigationRepo
{
    Task<List<ClaimInvestigationDTO>> GetAll();

    Task<ClaimInvestigationDTO> GetById(int id);

    Task<string> Add(ClaimInvestigationCreateDTO dto);

    Task<string> Update(int id, ClaimInvestigationCreateDTO dto);
}