using AutoMapper;
using Insurance.Application.DTO.Payment;
using Insurance.Application.DTOs.Customer;
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


            // Payment

            CreateMap<Payment, PaymentResponseDto>();

            CreateMap<PaymentResponseDto, Payment>();
        }
    }
}