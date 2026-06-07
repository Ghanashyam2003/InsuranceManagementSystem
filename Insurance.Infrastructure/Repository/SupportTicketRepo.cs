using Insurance.Application.DTO.SupportTicket;
using Insurance.Application.Interface;
using Insurance.Domain.Models;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Repository
{
    public class SupportTicketRepo : ISupportTicketRepo
    {
        private readonly ApplicationDbContext db;

        public SupportTicketRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> CreateTicketAsync(CreateSupportTicketDto dto)
        {
            SupportTicket ticket =new SupportTicket
                {
                    TicketNumber ="TKT-" + DateTime.Now.Ticks,

                    UserId = dto.UserId,

                    Subject = dto.Subject,

                    Description = dto.Description,

                    Status = "Open",

                    CreatedDate = DateTime.Now,

                    CreatedBy = dto.UserId
                };

            db.SupportTickets.Add(ticket);

            await db.SaveChangesAsync();

            return true;
        }
        public async Task<List<SupportTicket>>GetAllTicketsAsync()
        {
            return await db.SupportTickets
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        public async Task<SupportTicket?>GetTicketByIdAsync(int id)
        {
            return await db.SupportTickets
                .FirstOrDefaultAsync(x =>
                    x.TicketId == id);
        }
        public async Task<bool>ResolveTicketAsync(int id)
        {
            var ticket =
                await db.SupportTickets
                    .FirstOrDefaultAsync(x =>
                        x.TicketId == id);

            if (ticket == null)
            {
                return false;
            }

            ticket.Status = "Resolved";
            ticket.ResolvedDate = DateTime.Now;

            await db.SaveChangesAsync();

            return true;
        }
        public async Task<List<SupportTicket>>GetOpenTicketsAsync()
        {
            return await db.SupportTickets
                .Where(x => x.Status == "Open")
                .ToListAsync();
        }
        public async Task<List<SupportTicket>>GetResolvedTicketsAsync()
        {
            return await db.SupportTickets
                .Where(x => x.Status == "Resolved")
                .OrderByDescending(x => x.ResolvedDate)
                .ToListAsync();
        }
        public async Task<List<SupportTicket>>GetTicketsByUserAsync(int userId)
        {
            return await db.SupportTickets
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        public async Task<bool>UpdateStatusAsync(int ticketId,string status)
        {
            var ticket =
                await db.SupportTickets
                    .FirstOrDefaultAsync(x =>
                        x.TicketId == ticketId);

            if (ticket == null)
            {
                return false;
            }

            ticket.Status = status;

            await db.SaveChangesAsync();

            return true;
        }
        public async Task<bool>CloseTicketAsync(int id)
        {
            var ticket =
                await db.SupportTickets
                    .FirstOrDefaultAsync(x =>
                        x.TicketId == id);

            if (ticket == null)
            {
                return false;
            }

            ticket.Status = "Closed";

            await db.SaveChangesAsync();

            return true;
        }
    }
}
