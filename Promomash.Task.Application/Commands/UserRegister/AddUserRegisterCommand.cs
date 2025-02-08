using MediatR;

namespace PromomashTask.Application.Commands.UserRegister;

public record AddUserRegisterCommand(
string Email,
string Password,
Guid CountryId,
Guid ProvinceId
) : IRequest;

