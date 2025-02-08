using MediatR;
using PromomashTask.Application.DTOs.UserLogin;

namespace PromomashTask.Application.Commands.UserLogin;

public record LoginUserCommand(UserRequestDto userRequest) : IRequest<UserResponseDto>;
