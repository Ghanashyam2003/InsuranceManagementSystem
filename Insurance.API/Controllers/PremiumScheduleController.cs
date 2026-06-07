using Insurance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Insurance.API.Controllers
{
    [ApiController]
    [Route("api/v1/premium-schedules")]
    public class PremiumScheduleController : ControllerBase
    {
        private readonly IPremiumScheduleRepo service;

        public PremiumScheduleController(
            IPremiumScheduleRepo service)
        {
            this.service = service;
        }

        [HttpPost("generate/{policyId}")]
        public async Task<IActionResult> Generate(int policyId)
        {
            var result =
                await service.GenerateScheduleAsync(policyId);

            if (!result)
            {
                return BadRequest(
                    "Policy not found or schedule already exists.");
            }

            return Ok(
                "Premium Schedule Generated Successfully.");
        }

        [HttpPut("pay/{scheduleId}")]
        public async Task<IActionResult> PayInstallment(int scheduleId)
        {
            var result =
                await service.PayInstallmentAsync(scheduleId);

            if (!result)
            {
                return BadRequest("Installment not found.");
            }

            return Ok("Installment Paid Successfully.");
        }

        [HttpGet("policy/{policyId}")]
        public async Task<IActionResult> GetByPolicy(int policyId)
        {
            var result =
                await service.GetScheduleByPolicyIdAsync(policyId);

            if (result == null || !result.Any())
            {
                return NotFound("No schedule found.");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var result =
                await service.GetAllSchedulesAsync();

            return Ok(result);
        }
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingSchedules()
        {
            var result =
                await service.GetPendingSchedulesAsync();

            return Ok(result);
        }
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueSchedules()
        {
            var result =
                await service.GetOverdueSchedulesAsync();

            return Ok(result);
        }
        [HttpPost("/api/v1/premium-reminders/send")]
        public async Task<IActionResult> SendReminders()
        {
            int count =
                await service.SendRemindersAsync();

            return Ok(
                $"{count} reminder(s) generated successfully.");
        }
        [HttpGet("/api/v1/premium-reminders")]
        public async Task<IActionResult> GetAllReminders()
        {
            var result =
                await service.GetAllRemindersAsync();

            return Ok(result);
        }

        [HttpGet("/api/v1/premium-reminders/{id}")]
        public async Task<IActionResult>
        GetReminderById(int id)
        {
            var result =
                await service.GetReminderByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPut("/api/v1/premium-reminders/read/{id}")]
        public async Task<IActionResult>
         MarkAsRead(int id)
        {
            var result =
                await service.MarkAsReadAsync(id);

            if (!result)
                return NotFound();

            return Ok("Reminder marked as read.");
        }
        [HttpGet("/api/v1/premium-reminders/unread-count")]
        public async Task<IActionResult>
        GetUnreadCount()
        {
            var count =
                await service.GetUnreadCountAsync();

            return Ok(count);
        }
        [HttpGet("installments/{scheduleId}")]
        public async Task<IActionResult>
         GetInstallmentById(int scheduleId)
        {
            var result =
                await service.GetInstallmentByIdAsync(scheduleId);

            if (result == null)
            {
                return NotFound("Installment not found.");
            }

            return Ok(result);
        }
        [HttpGet("summary/{policyId}")]
        public async Task<IActionResult>
         GetSummary(int policyId)
        {
            var result =
                await service.GetPremiumSummaryAsync(policyId);

            return Ok(result);
        }
        [HttpGet("/api/v1/premium-reminders/policy/{policyId}")]
        public async Task<IActionResult>
         GetReminderHistory(int policyId)
        {
            var result =
                await service.GetReminderHistoryByPolicyAsync(policyId);

            return Ok(result);
        }
        [HttpPost("/api/v1/premium-reminders/resend/{scheduleId}")]
        public async Task<IActionResult>
        ResendReminder(int scheduleId)
        {
            var result =
                await service.ResendReminderAsync(scheduleId);

            if (!result)
            {
                return NotFound("Schedule not found.");
            }

            return Ok("Reminder Sent Successfully.");
        }
    }
}