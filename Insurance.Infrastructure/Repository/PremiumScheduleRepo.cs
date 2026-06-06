using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class PremiumScheduleRepo : IPremiumScheduleRepo
    {
        private readonly ApplicationDbContext db;

        public PremiumScheduleRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> GenerateScheduleAsync(int policyId)
        {
            var policy = await db.Policies
                .FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return false;
            }

            var scheduleExists = await db.PremiumSchedules
                .AnyAsync(x => x.PolicyId == policyId);

            if (scheduleExists)
            {
                return false;
            }

            decimal monthlyPremium = policy.PremiumAmount / 12;

            for (int i = 1; i <= 12; i++)
            {
                PremiumSchedule schedule = new PremiumSchedule
                {
                    PolicyId = policy.PolicyId,
                    InstallmentNumber = i,
                    DueDate = policy.PolicyStartDate.AddMonths(i - 1),
                    PremiumAmount = monthlyPremium,
                    IsPaid = false,
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now
                };

                await db.PremiumSchedules.AddAsync(schedule);
            }

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PayInstallmentAsync(int scheduleId)
        {
            var schedule = await db.PremiumSchedules
                .FirstOrDefaultAsync(x => x.ScheduleId == scheduleId);

            if (schedule == null)
            {
                return false;
            }

            schedule.IsPaid = true;
            schedule.PaidDate = DateTime.Now;
            schedule.ModifiedAt = DateTime.Now;
            schedule.ModifiedBy = 1;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<List<PremiumSchedule>> GetScheduleByPolicyIdAsync(int policyId)
        {
            return await db.PremiumSchedules
                .Where(x => x.PolicyId == policyId)
                .OrderBy(x => x.InstallmentNumber)
                .ToListAsync();
        }

        public async Task<List<PremiumSchedule>> GetAllSchedulesAsync()
        {
            return await db.PremiumSchedules
                .OrderBy(x => x.PolicyId)
                .ThenBy(x => x.InstallmentNumber)
                .ToListAsync();
        }
        public async Task<List<PremiumSchedule>> GetPendingSchedulesAsync()
        {
            return await db.PremiumSchedules
                .Where(x => x.IsPaid == false)
                .OrderBy(x => x.DueDate)
                .ToListAsync();
        }
        public async Task<List<PremiumSchedule>> GetOverdueSchedulesAsync()
        {
            return await db.PremiumSchedules
                .Where(x =>
                    x.IsPaid == false &&
                    x.DueDate < DateTime.Now)
                .OrderBy(x => x.DueDate)
                .ToListAsync();
        }

        public async Task<int> SendRemindersAsync()
        {
            var overdueSchedules =
                await db.PremiumSchedules
                .Where(x =>
                    x.IsPaid == false &&
                    x.DueDate < DateTime.Now)
                .ToListAsync();

            foreach (var item in overdueSchedules)
            {
                Notification notification =
                    new Notification()
                    {
                        UserId = 2,
                        Message =
                            $"Premium installment {item.InstallmentNumber} is overdue.",
                        NotificationType = "Premium Reminder",
                        IsRead = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1
                    };

                db.Notifications.Add(notification);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

            return overdueSchedules.Count;
        }
        public async Task<List<Notification>>
        GetAllRemindersAsync()
        {
                return await db.Notifications
                .Where(x => x.NotificationType == "Premium Reminder")
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        public async Task<Notification?>
         GetReminderByIdAsync(int id)
        {
            return await db.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == id);
        }
        public async Task<bool>
        MarkAsReadAsync(int id)
        {
            var reminder =
                await db.Notifications
                    .FirstOrDefaultAsync(x =>
                        x.NotificationId == id);

            if (reminder == null)
                return false;

            reminder.IsRead = true;

            await db.SaveChangesAsync();

            return true;
        }
        public async Task<int>
        GetUnreadCountAsync()
        {
            return await db.Notifications
                .CountAsync(x =>
                    x.NotificationType ==
                    "Premium Reminder"
                    &&
                    !x.IsRead);
        }
        public async Task<PremiumSchedule?> GetInstallmentByIdAsync(int scheduleId)
        {
            return await db.PremiumSchedules
                .FirstOrDefaultAsync(x =>
                    x.ScheduleId == scheduleId);
        }
        public async Task<object>
        GetPremiumSummaryAsync(int policyId)
        {
            var schedules =
                await db.PremiumSchedules
                    .Where(x => x.PolicyId == policyId)
                    .ToListAsync();

            return new
            {
                Total = schedules.Count,
                Paid = schedules.Count(x => x.IsPaid == true),
                Pending = schedules.Count(x => x.IsPaid == false),
                Overdue = schedules.Count(x =>
                    x.IsPaid == false &&
                    x.DueDate < DateTime.Now)
            };
        }
        public async Task<List<Notification>>
         GetReminderHistoryByPolicyAsync(int policyId)
        {
            return await db.Notifications
                .Where(x =>
                    x.NotificationType == "Premium Reminder" &&
                    x.Message.Contains(policyId.ToString()))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool>
    ResendReminderAsync(int scheduleId)
        {
            var schedule =
                await db.PremiumSchedules
                    .FirstOrDefaultAsync(x =>
                        x.ScheduleId == scheduleId);

            if (schedule == null)
            {
                return false;
            }

            Notification notification =
                new Notification
                {
                    UserId = 2,
                    Message =
                        $"Reminder for Installment {schedule.InstallmentNumber}",
                    NotificationType =
                        "Premium Reminder",
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1
                };

            db.Notifications.Add(notification);

            await db.SaveChangesAsync();

            return true;
        }
    }
}