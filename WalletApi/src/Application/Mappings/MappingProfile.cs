using AutoMapper;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Domain.Entities;

namespace WalletApi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterModel, AppUser>()
            .ReverseMap();

        CreateMap<AppUser, AppUserResponse>()
            .ReverseMap();
        
        CreateMap<Transfer, TransferResponse>()
            .ForMember(dest => dest.Date,
                opt => opt.MapFrom(src => src.TransferDate));

        CreateMap<AppUser, WalletResponse>()
            .ForMember(dest => dest.Amount, 
                opt => 
                    opt.MapFrom(src => src.WalletBalance))
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(
                    src => src.Id));

        CreateMap<LoginModel, AppUserResponse>()
            .ReverseMap();
    }
    
}