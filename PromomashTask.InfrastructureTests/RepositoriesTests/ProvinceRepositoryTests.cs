using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;
using PromomashTask.Infrastructure.Repositories;

namespace PromomashTask.InfrastructureTests.RepositoriesTests;

[TestFixture]
public class ProvinceRepositoryTests
{
	private ApplicationDbContext _context;
	private ProvinceRepository _repository;

	[SetUp]
	public void SetUp()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
			.Options;

		_context = new ApplicationDbContext(options);
		_repository = new ProvinceRepository(_context);
	}

	[TearDown]
	public void TearDown()
	{
		_context.Dispose();
	}

	[Test]
	public async Task GetAllProvincesByCountryAsync_ShouldReturnProvinces_WhenCountryExists()
	{
		var countryId = Guid.NewGuid();
		var provinces = new List<Province>
			{
				new Province { Id = Guid.NewGuid(), Name = "Province 1", CountryId = countryId },
				new Province { Id = Guid.NewGuid(), Name = "Province 2", CountryId = countryId }
			};

		await _context.Provinces.AddRangeAsync(provinces);
		await _context.SaveChangesAsync();

		var result = await _repository.GetAllProvincesByCountryAsync(countryId);

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.Not.Empty);
		Assert.That(result.Count(), Is.EqualTo(2));
	}

	[Test]
	public async Task GetAllProvincesByCountryAsync_ShouldReturnEmpty_WhenCountryHasNoProvinces()
	{
		var countryId = Guid.NewGuid();

		var result = await _repository.GetAllProvincesByCountryAsync(countryId);

		Assert.That(result, Is.Empty);
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnProvince_WhenProvinceExists()
	{
		var province = new Province { Id = Guid.NewGuid(), Name = "Test Province", CountryId = Guid.NewGuid() };
		await _context.Provinces.AddAsync(province);
		await _context.SaveChangesAsync();

		var result = await _repository.GetByIdAsync(province.Id);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(province.Id));
		Assert.That(result.Name, Is.EqualTo(province.Name));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnNull_WhenProvinceDoesNotExist()
	{
		var result = await _repository.GetByIdAsync(Guid.NewGuid());

		Assert.That(result, Is.Null);
	}
}
