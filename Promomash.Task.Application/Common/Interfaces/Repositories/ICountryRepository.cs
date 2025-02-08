using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Common.Interfaces.Repositories;

public interface ICountryRepository 
{
	Task<IEnumerable<Country>> GetAllAsync();
	Task<Country?> GetByIdAsync(Guid id);
}
