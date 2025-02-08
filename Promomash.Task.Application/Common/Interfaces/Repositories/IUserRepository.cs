using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
	Task AddAsync(User user);
	Task<User> GetByEmailAsync(string email);
	Task<User?> GetByIdAsync(Guid id);
	Task<IEnumerable<User>> GetAllAsync();
	Task UpdateAsync(User user);
	Task DeleteAsync(Guid id);
}
