using MediatR;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Queries.Countries;

public class GetCountriesQuery:IRequest<IEnumerable<Country>> { }
