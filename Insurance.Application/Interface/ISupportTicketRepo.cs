using Insurance.Application.DTO.SupportTicket;
using Insurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface ISupportTicketRepo
    {
        Task<bool> CreateTicketAsync(CreateSupportTicketDto dto);

        Task<List<SupportTicket>> GetAllTicketsAsync();

        Task<SupportTicket?> GetTicketByIdAsync(int id);

        Task<bool> ResolveTicketAsync(int id);

        Task<List<SupportTicket>>GetOpenTicketsAsync();

        Task<List<SupportTicket>>GetResolvedTicketsAsync();

        Task<List<SupportTicket>>GetTicketsByUserAsync(int userId);

        Task<bool> UpdateStatusAsync(int ticketId,string status);

        Task<bool> CloseTicketAsync(int id);


    }
}
