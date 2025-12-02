using AutoMapper;
using BillingFile.Application.DTOs;
using BillingFile.Domain.Entities;

namespace BillingFile.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity-DTO mappings - matching actual database schema
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel mappings (read-only) - map actual database columns
        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.HotelChainID, opt => opt.MapFrom(src => src.fkHotelChainID));

        // Reservation mappings (read-only) - direct mapping
        CreateMap<FullReservation, ReservationDto>();

        // Billing mappings - only mapped fields from FullReservation
        // This maps to BillingDto which contains only the fields defined in the mapping table
        CreateMap<FullReservation, BillingDto>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.Secondary_Source) 
                    ? src.Secondary_Source 
                    : src.Travel_Agency_Name));
    }
}

