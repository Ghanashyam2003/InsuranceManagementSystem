using Insurance.Application.DTOs.Quote;

namespace Insurance.Application.Interfaces
{
    public interface IQuoteRepo
    {
        Task<QuoteResponseDto> GenerateQuote(CreateQuoteDto dto);

        Task<List<QuoteResponseDto>> GetAllQuotes(
    int pageNumber,
    int pageSize);

        Task<PremiumBreakdownDto> GetPremiumBreakdown(int quoteId);

        Task<List<QuoteResponseDto>> GetQuotesByCustomerId(int customerId);

        Task<bool> AcceptQuote(int quoteId);

        Task<bool> RejectQuote(int quoteId);

        Task<List<QuoteResponseDto>> GetQuotesByStatus(string status);

    }

}