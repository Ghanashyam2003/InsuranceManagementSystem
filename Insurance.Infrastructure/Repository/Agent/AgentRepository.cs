using Insurance.Application.DTO.Agent;
using Insurance.Application.Interface;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ApplicationDbContext db;

        public AgentRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        private string GenerateAgentCode(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Agent name is required");


            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);


            string initials = "";

            foreach (var part in parts)
            {
                initials += char.ToUpper(part[0]);
            }


            if (initials.Length == 1)
                initials += initials;


            var random = new Random();
            int number = random.Next(10, 99);

            return $"A{initials}{number}";
        }


        public async Task<int> CreateAgent(CreateAgentDto dto)
        {
            var exists = await db.Agents
                .AnyAsync(x => x.Email == dto.Email);

            if (exists)
                throw new Exception("Agent already exists");


            string agentCode = GenerateAgentCode(dto.AgentName);

            var agent = new Agents
            {
                AgentCode = agentCode,
                AgentName = dto.AgentName,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            db.Agents.Add(agent);
            await db.SaveChangesAsync();

            return agent.AgentId;
        }


        public async Task<List<Agents>> GetAllAgents(int pageNumber,
    int pageSize)
        {
            return await db.Agents
                .Where(x => x.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Agents?> GetAgentById(int id)
        {
            return await db.Agents
                .FirstOrDefaultAsync(x => x.AgentId == id && x.IsActive);
        }


        public async Task UpdateAgent(int id, CreateAgentDto dto)
        {
            var agent = await db.Agents.FindAsync(id);

            if (agent == null)
                throw new Exception("Agent not found");


            agent.AgentName = dto.AgentName;
            agent.Email = dto.Email;
            agent.MobileNumber = dto.MobileNumber;
            agent.ModifiedBy = dto.CreatedBy;
            agent.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }


        public async Task DeleteAgent(int id)
        {
            var agent = await db.Agents.FindAsync(id);

            if (agent == null)
                throw new Exception("Agent not found");

            agent.IsActive = false;
            agent.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public async Task<List<Policy>> GetAgentPolicies(int agentId, int pageNumber, int pageSize)
        {
            return await db.Policies
                .Where(x =>
                    x.AgentId == agentId &&
                    !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetAgentCustomers(int agentId, int pageNumber, int pageSize)
        {
            return await db.Customers
                .Where(x =>
                    x.AgentId == agentId &&
                    !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Agents>> SearchAgents(string name, int pageNumber, int pageSize)
        {
            return await db.Agents
                .Where(x =>
                    x.IsActive &&
                    x.AgentName.Contains(name))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetCustomersWithPendingQuotes(int agentId, int pageNumber, int pageSize)
        {
            return await db.Customers
                .Where(c =>
                    c.AgentId == agentId &&
                    db.Quotes.Any(q =>
                        q.CustomerId == c.CustomerId &&
                        q.Status == "Pending"))
                .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetCustomersWithActivePolicies(int agentId, int pageNumber, int pageSize)
        {
            return await db.Customers
                .Where(c =>
                    c.AgentId == agentId &&
                    db.Policies.Any(p =>
                        p.CustomerId == c.CustomerId &&
                        p.Status == "Active" &&
                        !p.IsDeleted))
                .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AgentDashboardDto> GetDashboard(int agentId)
        {
            var agent = await db.Agents
                .FirstOrDefaultAsync(x => x.AgentId == agentId);

            if (agent == null)
                throw new Exception("Agent Not Found");

            var totalCustomers =
                await db.Customers
                    .CountAsync(x =>
                        x.AgentId == agentId &&
                        !x.IsDeleted);

            var totalPolicies =
                await db.Policies
                    .CountAsync(x =>
                        x.AgentId == agentId &&
                        !x.IsDeleted);

            var activePolicies =
                await db.Policies
                    .CountAsync(x =>
                        x.AgentId == agentId &&
                        x.Status == "Active" &&
                        !x.IsDeleted);

            var pendingQuotes =
                await db.Quotes
                    .CountAsync(x =>
                        x.Status == "Pending" &&
                        db.Customers.Any(c =>
                            c.CustomerId == x.CustomerId &&
                            c.AgentId == agentId));

            var totalCommission =
                await db.Commissions
                    .Where(x => x.AgentId == agentId)
                    .SumAsync(x =>
                        (decimal?)x.CommissionAmount) ?? 0;

            var totalPremium =
                await db.Policies
                    .Where(x =>
                        x.AgentId == agentId &&
                        !x.IsDeleted)
                    .SumAsync(x =>
                        (decimal?)x.PremiumAmount) ?? 0;

            return new AgentDashboardDto
            {
                TotalCustomers = totalCustomers,
                TotalPolicies = totalPolicies,
                ActivePolicies = activePolicies,
                PendingQuotes = pendingQuotes,
                TotalCommissionEarned = totalCommission,
                TotalPremiumCollected = totalPremium
            };
        }
    }
}