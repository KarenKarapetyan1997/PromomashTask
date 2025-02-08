using MediatR;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Queries.Provinces;

public class GetProvincesQueryHandler : IRequestHandler<GetProvincesQuery, IEnumerable<Province>>
{
	private readonly IProvinceRepository _provinceRepository;

	public GetProvincesQueryHandler(IProvinceRepository provinceRepository)
	{
		_provinceRepository = provinceRepository;
	}

	public async Task<IEnumerable<Province>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
	{
		var provinces = await _provinceRepository.GetAllProvincesByCountryAsync(request.CountryId);
		return provinces ?? Enumerable.Empty<Province>();
	}
}
