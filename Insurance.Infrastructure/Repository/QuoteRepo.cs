using AutoMapper;
using Insurance.Application.DTOs.Quote;
using Insurance.Application.Interfaces;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repository
{
    public class QuoteRepo : IQuoteRepo
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public QuoteRepo(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<QuoteResponseDto> GenerateQuote(CreateQuoteDto dto)
        {
            var customer = await db.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == dto.CustomerId);

            var product = await db.InsuranceProducts
    .FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);

            if (product == null)
                throw new Exception("Product Not Found");

            var benefit = await db.ProductBenefits
    .FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);

            if (benefit == null)
                throw new Exception("Product Benefit Not Found");


            
            int riskScore = 0;

            // AGE SCORE (common base rule)
            if (dto.Age >= 18 && dto.Age <= 30)
                riskScore += 5;
            else if (dto.Age <= 45)
                riskScore += 15;
            else if (dto.Age <= 60)
                riskScore += 30;
            else
                riskScore += 50;

            
            if (dto.IsDiabetic)
                riskScore += 20;

            if (dto.HasHypertension)
                riskScore += 15;

            if (dto.HasHeartDisease)
                riskScore += 40;

            if (dto.IsSmoking)
                riskScore += 25;

            if (dto.ConsumesAlcohol)
                riskScore += 10;

            if (dto.BMI > 30)
                riskScore += 15;

            
            var productType = product.ProductType?.ToLower();

            if (productType == "life")
            {
                if (dto.Age > 50)
                    riskScore += 25;

                if (dto.IsSmoking)
                    riskScore += 30;

                if (dto.ConsumesAlcohol)
                    riskScore += 15;

                if (dto.HasHeartDisease)
                    riskScore += 40;

                if (dto.IsDiabetic)
                    riskScore += 20;
            }

            
            string riskCategory =
                riskScore <= 30 ? "Low" :
                riskScore <= 70 ? "Medium" :
                "High";

            
            decimal riskLoading = 0;

            if (dto.Age >= 18 && dto.Age < 30)
                riskLoading = 0;
            else if (dto.Age >= 30 && dto.Age < 45)
                riskLoading = 10;
            else if (dto.Age >= 45 && dto.Age < 60)
                riskLoading = 20;
            else
                riskLoading = 40;

            
            decimal basePremium =
                dto.SumInsured * benefit.BaseRate / 100;

            decimal annualPremium =
                     basePremium * (1 + riskLoading / 100);

            decimal monthlyPremium =
                Math.Round(annualPremium / 12, 2);

           
            var healthProfile = new HealthProfile
            {
                CustomerId = dto.CustomerId,
                BMI = dto.BMI,
                IsDiabetic = dto.IsDiabetic,
                HasHypertension = dto.HasHypertension,
                HasHeartDisease = dto.HasHeartDisease,
                IsSmoking = dto.IsSmoking,
                ConsumesAlcohol = dto.ConsumesAlcohol,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow
            };

            db.HealthProfiles.Add(healthProfile);
            await db.SaveChangesAsync();

            
            var quote = new Quote
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                SumInsured = dto.SumInsured,
                PremiumAmount = monthlyPremium,
                QuoteDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                Status = "Pending",
                RiskCategory = riskCategory,
                RiskLoadingPercentage = riskLoading,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow
            };

            db.Quotes.Add(quote);
            await db.SaveChangesAsync();

            
            var underwriting = new UnderwritingCase
            {
                QuoteId = quote.QuoteId,
                RiskCategory = riskCategory,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow
            };

            db.UnderwritingCases.Add(underwriting);
            await db.SaveChangesAsync();

           
            return mapper.Map<QuoteResponseDto>(quote);
        }

        public async Task<List<QuoteResponseDto>> GetAllQuotes(
    int pageNumber,
    int pageSize)
        {
            var quotes = await db.Quotes
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return mapper.Map<List<QuoteResponseDto>>(quotes);
        }


        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public async Task<PremiumBreakdownDto> GetPremiumBreakdown(int quoteId)
        {
            var quote = await db.Quotes
                .FirstOrDefaultAsync(x => x.QuoteId == quoteId);

            if (quote == null)
                throw new Exception("Quote Not Found");

            var product = await db.InsuranceProducts
                .FirstOrDefaultAsync(x => x.ProductId == quote.ProductId);

            var benefit = await db.ProductBenefits
                .FirstOrDefaultAsync(x => x.ProductId == quote.ProductId);

            var customer = await db.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == quote.CustomerId);

            var health = await db.HealthProfiles
                .FirstOrDefaultAsync(x => x.CustomerId == quote.CustomerId);

            if (product == null || benefit == null || customer == null)
                throw new Exception("Required data missing");

            
            int age = CalculateAge(customer.DOB);

            
            int riskScore = 0;

            // AGE SCORE
            if (age >= 18 && age <= 30)
                riskScore += 5;
            else if (age <= 45)
                riskScore += 15;
            else if (age <= 60)
                riskScore += 30;
            else
                riskScore += 50;

            
            if (health != null)
            {
                if (health.IsDiabetic)
                    riskScore += 20;

                if (health.HasHypertension)
                    riskScore += 15;

                if (health.HasHeartDisease)
                    riskScore += 40;

                if (health.IsSmoking)
                    riskScore += 25;

                if (health.ConsumesAlcohol)
                    riskScore += 10;

                if (health.BMI > 30)
                    riskScore += 15;
            }

      
            if (product.ProductType?.ToLower() == "life")
            {
                if (age > 50)
                    riskScore += 25;

                if (health?.IsSmoking == true)
                    riskScore += 30;

                if (health?.ConsumesAlcohol == true)
                    riskScore += 15;

                if (health?.HasHeartDisease == true)
                    riskScore += 40;

                if (health?.IsDiabetic == true)
                    riskScore += 20;
            }

            
            string riskCategory =
                riskScore <= 30 ? "Low" :
                riskScore <= 70 ? "Medium" :
                "High";

           
            decimal basePremium =
                quote.SumInsured * benefit.BaseRate / 100;

           
            decimal riskLoading =
                age >= 18 && age < 30 ? 0 :
                age >= 30 && age < 45 ? 10 :
                age >= 45 && age < 60 ? 20 :
                40;

            
            decimal annualPremium =
                basePremium * (1 + riskLoading / 100);

            decimal monthlyPremium =
                Math.Round(annualPremium / 12, 2);

            
            return new PremiumBreakdownDto
            {
                QuoteId = quote.QuoteId,
                SumInsured = quote.SumInsured,
                BaseRate = benefit.BaseRate,
                BasePremium = basePremium,
                RiskScore = riskScore,
                RiskCategory = riskCategory,
                RiskLoadingPercentage = riskLoading,
                FinalPremium = monthlyPremium,
                ProductType = product.ProductType
            };
        }

        public async Task<List<QuoteResponseDto>> GetQuotesByCustomerId(int customerId)
        {
            var quotes = await db.Quotes
                .Where(x => x.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();

            if (quotes == null || quotes.Count == 0)
                return new List<QuoteResponseDto>();

            return mapper.Map<List<QuoteResponseDto>>(quotes);
        }

        public async Task<bool> AcceptQuote(int quoteId)
        {
            var quote = await db.Quotes
                .FirstOrDefaultAsync(x => x.QuoteId == quoteId);

            if (quote == null)
                throw new Exception("Quote Not Found");

            
            if (quote.Status == "Accepted")
                throw new Exception("Quote is already accepted");

            if (quote.Status == "Expired")
                throw new Exception("Cannot accept expired quote");

            
            quote.Status = "Accepted";
            quote.ModifiedAt = DateTime.UtcNow;

            db.Quotes.Update(quote);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectQuote(int quoteId)
        {
            var quote = await db.Quotes
                .FirstOrDefaultAsync(x => x.QuoteId == quoteId);

            if (quote == null)
                throw new Exception("Quote Not Found");

            
            if (quote.Status == "Rejected")
                throw new Exception("Quote is already rejected");

            if (quote.Status == "Accepted")
                throw new Exception("Cannot reject an accepted quote");

            if (quote.Status == "Expired")
                throw new Exception("Cannot reject expired quote");

            
            quote.Status = "Rejected";
            quote.ModifiedAt = DateTime.UtcNow;

            db.Quotes.Update(quote);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<List<QuoteResponseDto>> GetQuotesByStatus(string status)
        {
            var quotes = await db.Quotes
                .Where(x => x.Status == status)
                .AsNoTracking()
                .ToListAsync();

            return mapper.Map<List<QuoteResponseDto>>(quotes);
        }
    }
}