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

        // Billing mappings from Stored Procedure results
        // Maps GetBillingFileReservations SP output to BillingDto
        CreateMap<BillingSpResult, BillingDto>();
    }
}

