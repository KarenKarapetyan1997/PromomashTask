using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;
using PromomashTask.Infrastructure.Repositories;

namespace PromomashTask.InfrastructureTests.RepositoriesTests;

[TestFixture]
public class UserRepositoryTests
{
	private ApplicationDbContext _context;
	private UserRepository _repository;

	[SetUp]
	public void SetUp()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
			.Options;

		_context = new ApplicationDbContext(options);
		_repository = new UserRepository(_context);
	}

	[TearDown]
	public void TearDown()
	{
		_context.Dispose();
	}

	[Test]
	public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
	{
		var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Password = "securePassword" };
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		var result = await _repository.GetByEmailAsync(user.Email);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Email, Is.EqualTo(user.Email));
	}

	[Test]
	public async Task GetByEmailAsync_ShouldThrowException_WhenUserDoesNotExist()
	{
		var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
			await _repository.GetByEmailAsync("nonexistent@example.com"));

		Assert.That(exception.Message, Is.EqualTo("User with email 'nonexistent@example.com' not found."));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
	{
		var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Password = "securePassword" };
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		var result = await _repository.GetByIdAsync(user.Id);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(user.Id));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
	{
		var result = await _repository.GetByIdAsync(Guid.NewGuid());

		Assert.That(result, Is.Null);
	}

	[Test]
	public async Task AddAsync_ShouldAddUser()
	{
		var user = new User { Id = Guid.NewGuid(), Email = "newuser@example.com", Password = "securePassword" };

		await _repository.AddAsync(user);
		await _context.SaveChangesAsync();

		var addedUser = await _context.Users.FindAsync(user.Id);
		Assert.That(addedUser, Is.Not.Null);
		Assert.That(addedUser.Email, Is.EqualTo(user.Email));
	}

	[Test]
	public async Task UpdateAsync_ShouldUpdateUser()
	{
		var user = new User { Id = Guid.NewGuid(), Email = "oldemail@example.com", Password = "securePassword" };
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		user.Email = "updatedemail@example.com";

		await _repository.UpdateAsync(user);
		await _context.SaveChangesAsync();

		var updatedUser = await _context.Users.FindAsync(user.Id);
		Assert.That(updatedUser, Is.Not.Null);
		Assert.That(updatedUser.Email, Is.EqualTo(user.Email));
	}

	[Test]
	public async Task DeleteAsync_ShouldRemoveUser_WhenUserExists()
	{
		var user = new User { Id = Guid.NewGuid(), Email = "deleteuser@example.com", Password = "securePassword" };
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		await _repository.DeleteAsync(user.Id);
		await _context.SaveChangesAsync();

		var deletedUser = await _context.Users.FindAsync(user.Id);
		Assert.That(deletedUser, Is.Null);
	}

	[Test]
	public async Task DeleteAsync_ShouldNotRemoveUser_WhenUserDoesNotExist()
	{
		await _repository.DeleteAsync(Guid.NewGuid());
		await _context.SaveChangesAsync();

		var count = await _context.Users.CountAsync();
		Assert.That(count, Is.EqualTo(0)); 
	}
}
