using AutoMapper;
using InMotion.Data.Models.Auth;
using InMotion.Services.Models.Auth;
using LR.Services.Models.Auth;
using System.IO;

namespace InMotion.WebHost.Models;

/// <summary>
/// Mapping profile.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        this.CreateMap<User, UsersVM>();
        this.CreateMap<UserUM, RegisterModel>();
    }
}