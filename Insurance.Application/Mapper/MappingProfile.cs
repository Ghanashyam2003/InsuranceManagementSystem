using AutoMapper;
using Insurance.Application.DTOs.Customer;
using Insurance.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Insurance.Application.DTOs.Policy;


namespace Insurance.Application.Mappings
    {
        public class MappingProfile: Profile
        {
        public MappingProfile()
        {
            CreateMap<Policy, PolicyResponseDto>()
                .ForMember(
                    dest => dest.PolicyStartDate,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PolicyStartDate))
                )
                .ForMember(
                    dest => dest.PolicyEndDate,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PolicyEndDate))
                );

            CreateMap<CreatePolicyRequestDto, Policy>();

            CreateMap<PolicyMember, PolicyMemberDto>().ReverseMap();
        }
    }
    }