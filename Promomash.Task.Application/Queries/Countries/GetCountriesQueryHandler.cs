using MediatR;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Queries.Countries;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IEnumerable<Country>>
{
	private readonly ICountryRepository _countryRepository;

	public GetCountriesQueryHandler(ICountryRepository countryRepository)
	{
		_countryRepository = countryRepository;
	}
	public async Task<IEnumerable<Country>> Handle(GetCountriesQuery request,CancellationToken cancellationToken)
	{
	  return await _countryRepository.GetAllAsync();
	}
}
