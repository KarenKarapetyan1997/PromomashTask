using Microsoft.EntityFrameworkCore;
using PromomashTask.Application.Common.Interfaces.UnitOfWork;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Infrastructure.Common;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
	public DbSet<User> Users { get; set; }
	public DbSet<Country> Countries { get; set; }
	public DbSet<Province> Provinces { get; set; }
	public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
	{
		await SaveChangesAsync(cancellationToken);
	}
	protected override async void OnModelCreating(ModelBuilder modelBuilder)
	{
	    modelBuilder.SeedData();
	}	
}
