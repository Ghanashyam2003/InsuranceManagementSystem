using Insurance.Application.DTO.Agent;
using Insurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface IAgentRepository
    {
        Task<int> CreateAgent(CreateAgentDto dto);

        Task<List<Agents>> GetAllAgents(int pageNumber, int pageSize);

        Task<Agents?> GetAgentById(int id);

        Task UpdateAgent(int id, CreateAgentDto dto);

        Task DeleteAgent(int id);

        Task<List<Policy>> GetAgentPolicies(int agentId, int pageNumber, int pageSize);

        Task<List<Customer>> GetAgentCustomers(int agentId, int pageNumber, int pageSize);

        Task<List<Agents>> SearchAgents(string name, int pageNumber, int pageSize);

        Task<List<Customer>> GetCustomersWithPendingQuotes(int agentId, int pageNumber, int pageSize);

        Task<List<Customer>>GetCustomersWithActivePolicies(int agentId, int pageNumber, int pageSize);

        Task<AgentDashboardDto> GetDashboard(int agentId);

    }
}
