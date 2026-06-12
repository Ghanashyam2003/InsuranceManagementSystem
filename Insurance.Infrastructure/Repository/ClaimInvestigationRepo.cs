using AutoMapper;
using Insurance.Application.DTO.Claim;
using Insurance.Application.DTO.ClaimInvestigation;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Repository
{
    public class ClaimInvestigationRepo : IClaimInvestigationRepo
    {
        ApplicationDbContext db;
        IMapper mapper;

        public ClaimInvestigationRepo(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<ClaimInvestigationDTO>> GetAll()
        {
            var data = await db.ClaimInvestigations.Include(x => x.Claim).ToListAsync();

            return mapper.Map<List<ClaimInvestigationDTO>>(data);
        }

        public async Task<ClaimInvestigationDTO> GetById(int id)
        {
            var data = await db.ClaimInvestigations.Include(x => x.Claim).FirstOrDefaultAsync(x => x.InvestigationId == id);

            return mapper.Map<ClaimInvestigationDTO>(data);
        }

        public async Task<string> Add(ClaimInvestigationCreateDTO dto)
        {
            var investigation = mapper.Map<ClaimInvestigation>(dto);

            investigation.InvestigationDate = DateTime.Now;

            await db.ClaimInvestigations.AddAsync(investigation);

            await db.SaveChangesAsync();

            return "Investigation Added Successfully";
        }

        public async Task<string> Update(int id, ClaimInvestigationCreateDTO dto)
        {
            var data = await db.ClaimInvestigations.FindAsync(id);

            if (data == null)
                return "Investigation Not Found";

            data.ClaimId = dto.ClaimId;
            data.Remarks = dto.Remarks;
            data.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();

            return "Investigation Updated Successfully";
        }
    }
}

