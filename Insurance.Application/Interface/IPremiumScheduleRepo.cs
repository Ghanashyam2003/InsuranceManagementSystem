using Insurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interfaces
{
    public interface IPremiumScheduleRepo
    {
        Task<bool> GenerateScheduleAsync(int policyId);

        Task<bool> PayInstallmentAsync(int scheduleId);

        Task<List<PremiumSchedule>> GetScheduleByPolicyIdAsync(int policyId);

        Task<List<PremiumSchedule>> GetAllSchedulesAsync();

        Task<List<PremiumSchedule>> GetPendingSchedulesAsync();

        Task<List<PremiumSchedule>> GetOverdueSchedulesAsync();

        Task<int> SendRemindersAsync();

        Task<List<Notification>> GetAllRemindersAsync();

        Task<Notification?> GetReminderByIdAsync(int id);

        Task<bool> MarkAsReadAsync(int id);

        Task<int> GetUnreadCountAsync();

        Task<PremiumSchedule?> GetInstallmentByIdAsync(int scheduleId);
        
        Task<object> GetPremiumSummaryAsync(int policyId);

        Task<List<Notification>>GetReminderHistoryByPolicyAsync(int policyId);

        Task<bool> ResendReminderAsync(int scheduleId);

    }
}
