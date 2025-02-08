using Microsoft.EntityFrameworkCore;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;

namespace PromomashTask.Infrastructure.Repositories;

public class ProvinceRepository: IProvinceRepository
{
	private readonly ApplicationDbContext _context;

	public ProvinceRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Province>> GetAllProvincesByCountryAsync(Guid countryId) 
	{
		return await _context.Provinces.Where(x => x.CountryId == countryId).ToListAsync();
	}
	public async Task<Province?> GetByIdAsync(Guid id) => await _context.Provinces.FindAsync(id);
}
