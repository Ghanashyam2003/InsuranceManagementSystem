using AutoMapper;
using Insurance.Application.DTOs.Customer;
using Insurance.Application.DTOs.Quote;
using Insurance.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Insurance.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<Customer, CustomerResponseDto>();

            CreateMap<CreateQuoteDto, HealthProfile>();

            CreateMap<Domain.Models.Quote, QuoteResponseDto>();
        }
    }
}