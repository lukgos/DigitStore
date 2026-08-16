using Account.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.GetUser;

public record GetUserQuery(Guid Id) : IQuery<UserDto>;