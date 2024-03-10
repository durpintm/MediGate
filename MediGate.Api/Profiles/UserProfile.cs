using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediGate.Entities.DbSet;
using MediGate.Entities.DTOs.Incoming;

namespace MediGate.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>()
            .ForMember(
                dest => dest.FirstName,
                from => from.MapFrom(x => $"{x.FirstName}")
            )
            .ForMember(
                dest => dest.LastName,
                from => from.MapFrom(x => $"{x.LastName}")
            )
            .ForMember(
                dest => dest.Email,
                from => from.MapFrom(x => $"{x.Email}")
            )
            .ForMember(
                dest => dest.Phone,
                from => from.MapFrom(x => $"{x.Phone}")
            )
            .ForMember(
                dest => Convert.ToDateTime(dest.DateOfBirth),
                from => from.MapFrom(x => $"{x.DateOfBirth}")
            )
            .ForMember(
                dest => dest.Country,
                from => from.MapFrom(x => $"{x.Country}")
            )
            .ForMember(
                dest => dest.Status,
                from => from.MapFrom(x => 1)
            );

        }

    }
}