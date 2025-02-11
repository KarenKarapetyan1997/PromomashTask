using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromomashTask.Application.Commands.UserRegister;
using PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;
using PromomashTask.Application.DTOs.UserRegister;
using PromomashTask.Infrastructure.Common;

namespace PromomashTask.Api.Controllers
{
	[Route("api/registration")]
	[ApiController]
	public class UserRegisterController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IPasswordHasher _passwordHasher;
		public UserRegisterController(IMediator mediator,IPasswordHasher passwordHasher)
		{
			_mediator = mediator;
			_passwordHasher = passwordHasher;
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDto request)
		{
			var command = new AddUserRegisterCommand
				(
					request.Email,
					_passwordHasher.HashPassword(request.Password),
					request.CountryId,
					request.ProvinceId
				);

			if (!ModelState.IsValid) return BadRequest(ModelState);

			await _mediator.Send(command);
			return Ok();
		}
	}
}
