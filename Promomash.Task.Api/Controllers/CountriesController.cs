using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromomashTask.Application.Queries.Countries;

namespace PromomashTask.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CountriesController(IMediator mediator)
		{
				_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCountries()
		{
			return Ok(await _mediator.Send(new GetCountriesQuery()));
		}
	}
}
