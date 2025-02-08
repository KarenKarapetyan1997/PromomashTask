using Microsoft.EntityFrameworkCore;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;

namespace PromomashTask.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
	private readonly ApplicationDbContext _context;

	public CountryRepository(ApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<IEnumerable<Country>> GetAllAsync()
	{

		return await _context.Countries
			.AsNoTracking()
			.Include(c => c.Provinces) // Ensure eager loading
			.ToListAsync();

	}
	public async Task<Country?> GetByIdAsync(Guid id) => await _context.Countries.FindAsync(id);
}
