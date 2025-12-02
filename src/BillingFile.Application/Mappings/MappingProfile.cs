using AutoMapper;
using BillingFile.Application.DTOs;
using BillingFile.Domain.Entities;

namespace BillingFile.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity-DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel mappings (read-only)
        CreateMap<Hotel, HotelDto>();

        // Reservation mappings (read-only)
        CreateMap<FullReservation, ReservationDto>();
    }
}

