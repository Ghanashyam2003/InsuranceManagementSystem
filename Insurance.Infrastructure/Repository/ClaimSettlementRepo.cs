using AutoMapper;
using Insurance.Application.DTO.Claim;
using Insurance.Application.Interface;
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
    public class ClaimSettlementRepo : IClaimSettlementRepo
    {
        ApplicationDbContext db;
        IMapper mapper;

        public ClaimSettlementRepo(ApplicationDbContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<List<ClaimSettlementDTO>> GetAll()
        {
            var data = await db.ClaimSettlements.Include(x => x.Claim).ToListAsync();

            return mapper.Map<List<ClaimSettlementDTO>>(data);
        }

        public async Task<ClaimSettlementDTO> GetById(int id)
        {
            var data = await db.ClaimSettlements.Include(x => x.Claim).FirstOrDefaultAsync(x => x.SettlementId == id);

            return mapper.Map<ClaimSettlementDTO>(data);
        }

        public async Task<string> Add(ClaimSettlementCreateDTO dto)
        {
            var settlement = mapper.Map<ClaimSettlement>(dto);

            settlement.SettlementDate = DateTime.Now;

            await db.ClaimSettlements.AddAsync(settlement);

            await db.SaveChangesAsync();

            return "Claim Settlement Added Successfully";
        }

        public async Task<string> Update(int id, ClaimSettlementCreateDTO dto)
        {
            var settlement = await db.ClaimSettlements.FindAsync(id);

            if (settlement == null)
                return "Settlement Not Found";

            settlement.ClaimId = dto.ClaimId;
            settlement.ApprovedAmount = dto.ApprovedAmount;
            settlement.SettlementStatus = dto.SettlementStatus;
            settlement.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();

            return "Claim Settlement Updated Successfully";
        }
    }
}
