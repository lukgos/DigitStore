using Account.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.GetMe;

public sealed record GetMeQuery(Guid UserId) : IQuery<UserDto>;