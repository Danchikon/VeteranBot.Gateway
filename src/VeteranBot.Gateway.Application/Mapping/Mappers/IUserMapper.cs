using System.Linq.Expressions;
using Mapster;
using VeteranBot.Gateway.Application.Dtos;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Application.Mapping.Mappers;

[Mapper]
public interface IUserMapper
{
    Expression<Func<UserEntity, UserDto>> ProjectToDto { get; }
    
    UserDto MapToDto(UserEntity customer);
}