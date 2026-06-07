using AutoMapper;
using Insurance.Application.DTO.Payment;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly ILogger<PaymentRepository> logger;

        public PaymentRepository(
            ApplicationDbContext db,
            IMapper mapper,
            ILogger<PaymentRepository> logger)
        {
            this.db = db;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<bool> MakePaymentAsync(
            MakePaymentDto dto)
        {
            var schedule = await db.PremiumSchedules
                .FirstOrDefaultAsync(x =>
                    x.ScheduleId == dto.ScheduleId);

            if (schedule == null)
            {
                return false;
            }

            Payment payment = new Payment()
            {
                PolicyId = schedule.PolicyId,
                Amount = schedule.PremiumAmount,
                PaymentMode = dto.PaymentMode,
                PaymentStatus = "Success",
                TransactionNumber =
                    Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now,
                CreatedBy = 1,
                CreatedAt = DateTime.Now
            };

            await db.Payments.AddAsync(payment);

            schedule.IsPaid = true;
            schedule.PaidDate = DateTime.Now;
            schedule.ModifiedBy = 1;
            schedule.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();

            logger.LogInformation(
                $"Payment Success for Policy {schedule.PolicyId}");

            return true;
        }

        public async Task<List<PaymentResponseDto>>
            GetAllPaymentsAsync()
        {
            var data = await db.Payments.ToListAsync();

            return mapper.Map<List<PaymentResponseDto>>(data);
        }

        public async Task<PaymentResponseDto?>
            GetPaymentByIdAsync(int paymentId)
        {
            var payment = await db.Payments
                .FirstOrDefaultAsync(x =>
                    x.PaymentId == paymentId);

            if (payment == null)
            {
                return null;
            }

            return mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<List<PaymentResponseDto>>
            GetSuccessfulPaymentsAsync()
        {
            var data = await db.Payments
                .Where(x =>
                    x.PaymentStatus == "Success")
                .ToListAsync();

            return mapper.Map<List<PaymentResponseDto>>(data);
        }

        public async Task<List<PaymentResponseDto>>
            GetFailedPaymentsAsync()
        {
            var data = await db.Payments
                .Where(x =>
                    x.PaymentStatus == "Failed")
                .ToListAsync();

            return mapper.Map<List<PaymentResponseDto>>(data);
        }

        public async Task<List<PaymentResponseDto>>
            GetPendingPaymentsAsync()
        {
            var data = await db.Payments
                .Where(x =>
                    x.PaymentStatus == "Pending")
                .ToListAsync();

            return mapper.Map<List<PaymentResponseDto>>(data);
        }

        public async Task<List<PaymentResponseDto>>
            GetPaymentHistoryByCustomerAsync(
                int customerId)
        {
            var data = await db.Payments
                .Include(x => x.Policy)
                .Where(x =>
                    x.Policy.CustomerId == customerId)
                .ToListAsync();

            return mapper.Map<List<PaymentResponseDto>>(data);
        }

        public async Task<List<DuePremiumDto>>
            GetDuePremiumsAsync(int customerId)
        {
            var data = await db.PremiumSchedules
                .Include(x => x.Policy)
                .Where(x =>
                    x.Policy.CustomerId == customerId
                    &&
                    x.IsPaid == false)
                .Select(x => new DuePremiumDto
                {
                    ScheduleId = x.ScheduleId,
                    PolicyId = x.PolicyId,
                    InstallmentNumber =
                        x.InstallmentNumber,
                    DueDate = x.DueDate,
                    PremiumAmount =
                        x.PremiumAmount
                })
                .ToListAsync();

            return data;
        }
    }
}