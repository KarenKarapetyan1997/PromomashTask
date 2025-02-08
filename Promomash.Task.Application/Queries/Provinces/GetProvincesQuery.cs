using MediatR;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Queries.Provinces;

public record GetProvincesQuery(Guid CountryId): IRequest<IEnumerable<Province>>;
