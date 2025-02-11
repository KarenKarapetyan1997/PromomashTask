using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromomashTask.Application.Queries.Provinces;

namespace PromomashTask.Api.Controllers
{
	[Route("api/province")]
	[ApiController]
	public class ProvincesController : ControllerBase
	{
		private readonly IMediator _mediator;
		public ProvincesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetProvincesById(Guid CountryId)
		{
			return Ok(await _mediator.Send(new GetProvincesQuery(CountryId)));
		}
	}
}
