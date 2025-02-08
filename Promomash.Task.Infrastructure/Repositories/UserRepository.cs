using Microsoft.EntityFrameworkCore;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;

namespace PromomashTask.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
	public async Task<User> GetByEmailAsync(string email) =>
	await context.Users.FirstOrDefaultAsync(u => u.Email == email)
	?? throw new KeyNotFoundException($"User with email '{email}' not found.");

	public async Task<User?> GetByIdAsync(Guid id) => await context.Users.FindAsync(id);

	public async Task<IEnumerable<User>> GetAllAsync() => await context.Users.AsNoTracking().ToListAsync();

	public Task UpdateAsync(User user)
	{
		context.Users.Update(user);
		return Task.CompletedTask;
	}

	public async Task AddAsync(User user)
	{
		await context.Users.AddAsync(user);
	}

	public async Task DeleteAsync(Guid id)
	{
		var entity = await context.Users.FindAsync(id);
		if (entity != null) context.Users.Remove(entity);
	}
}
