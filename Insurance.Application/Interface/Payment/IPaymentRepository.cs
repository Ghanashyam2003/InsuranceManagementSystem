using Insurance.Application.DTO.Payment;

namespace Insurance.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<bool> MakePaymentAsync(
            MakePaymentDto dto);

        Task<List<PaymentResponseDto>>
            GetAllPaymentsAsync();

        Task<PaymentResponseDto?>
            GetPaymentByIdAsync(int paymentId);

        Task<List<PaymentResponseDto>>
            GetPaymentHistoryByCustomerAsync(
                int customerId);

        Task<List<PaymentResponseDto>>
            GetSuccessfulPaymentsAsync();

        Task<List<PaymentResponseDto>>
            GetFailedPaymentsAsync();

        Task<List<PaymentResponseDto>>
            GetPendingPaymentsAsync();

        Task<List<DuePremiumDto>>
            GetDuePremiumsAsync(int customerId);
    }
}