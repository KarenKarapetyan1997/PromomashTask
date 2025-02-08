using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Common.Interfaces.Repositories;

public interface IProvinceRepository
{
	Task<IEnumerable<Province>> GetAllProvincesByCountryAsync(Guid countryId);
	Task<Province?> GetByIdAsync(Guid id);
}
