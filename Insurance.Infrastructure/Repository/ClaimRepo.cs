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
    public class ClaimRepo : IClaimRepo
    {
        ApplicationDbContext db;
        IMapper mapper;

        public ClaimRepo(ApplicationDbContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<List<ClaimDTO>> GetClaims()
        {
            var data = await db.Claims.Include(x => x.Policy).Include(x => x.Agent).ToListAsync();

            return mapper.Map<List<ClaimDTO>>(data);
        }
        //public async Task<string> Add(ClaimDTO2 dto)
        //{
        //    var claim = mapper.Map<Claim>(dto);

        //    claim.ClaimDate = DateTime.Now;
        //    claim.Status = "Pending";

        //    db.Claims.Add(claim);

        //    await db.SaveChangesAsync();

        //    return "Claim Added Successfully";
        //}
        public async Task<string> Add(ClaimDTO2 dto)
        {
            try
            {
                var claim = mapper.Map<Claim>(dto);

                claim.ClaimDate = DateTime.Now;
                claim.Status = "Pending";

                db.Claims.Add(claim);

                await db.SaveChangesAsync();

                return "Claim Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }
        public async Task<string> Update(int id, ClaimDTO2 dto)
        {
            var claim = await db.Claims.FindAsync(id);

            if (claim == null)
                return "Claim Not Found";

            claim.AgentId = dto.AgentId;
            claim.PolicyId = dto.PolicyId;
            claim.ClaimAmount = dto.ClaimAmount;
            claim.ClaimReason = dto.ClaimReason;

            claim.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();

            return "Claim Updated Successfully";
        }

        public async Task<ClaimDTO> GetById(int id)
        {
            var data = await db.Claims.FindAsync(id);

            return mapper.Map<ClaimDTO>(data);
        }
        public async Task<string> Delete(int id)
        {
            var claim = await db.Claims.FindAsync(id);

            if (claim == null)
                return "Claim Not Found";

            db.Claims.Remove(claim);

            await db.SaveChangesAsync();

            return "Claim Deleted Successfully";
        }
        public async Task<List<ClaimDTO>> GetByPolicyId(int policyId)
        {
            var data = await db.Claims.Include(x => x.Agent) .Include(x => x.Policy) .Where(x => x.PolicyId == policyId).ToListAsync();

            return mapper.Map<List<ClaimDTO>>(data);
        }

    }



}

