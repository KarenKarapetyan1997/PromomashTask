using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromomashTask.Application.Commands.UserLogin;
using PromomashTask.Application.DTOs.UserLogin;

namespace PromomashTask.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IMediator _mediator;
		public LoginController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserRequestDto userRequest)
		{
			var command = new LoginUserCommand(userRequest);
			return Ok(await _mediator.Send(command));
		}
	}
}
