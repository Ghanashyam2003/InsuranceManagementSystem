using Insurance.Application.DTO.SupportTicket;
using Insurance.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v1/SupportTicket")]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISupportTicketRepo service;

        public SupportTicketController(
            ISupportTicketRepo service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateSupportTicketDto dto)
        {
            var result =
                await service.CreateTicketAsync(dto);

            if (!result)
            {
                return BadRequest();
            }

            return Ok("Ticket Created Successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var result =
                await service.GetAllTicketsAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var result =
                await service.GetTicketByIdAsync(id);

            if (result == null)
            {
                return NotFound("Ticket Not Found.");
            }

            return Ok(result);
        }

        [HttpPut("resolve/{id}")]
        public async Task<IActionResult> ResolveTicket(int id)
        {
            var result =
                await service.ResolveTicketAsync(id);

            if (!result)
            {
                return NotFound("Ticket Not Found.");
            }

            return Ok("Ticket Resolved Successfully.");
        }

        [HttpGet("open")]
        public async Task<IActionResult> GetOpenTickets()
        {
            var result =
                await service.GetOpenTicketsAsync();

            return Ok(result);
        }

        [HttpGet("resolved")]
        public async Task<IActionResult> GetResolvedTickets()
        {
            var result =
                await service.GetResolvedTicketsAsync();

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketsByUser(int userId)
        {
            var result =
                await service.GetTicketsByUserAsync(userId);

            return Ok(result);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateTicketStatusDto dto)
        {
            var result =
                await service.UpdateStatusAsync(
                    id,
                    dto.Status);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Status Updated Successfully.");
        }

        [HttpPut("close/{id}")]
        public async Task<IActionResult> CloseTicket(int id)
        {
            var result =
                await service.CloseTicketAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Ticket Closed Successfully.");
        }
    }
}