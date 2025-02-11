using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PromomashTask.Domain.Entities;
using PromomashTask.Infrastructure.Common;
using PromomashTask.Infrastructure.Repositories;


namespace PromomashTask.InfrastructureTests.RepositoriesTests;

[TestFixture]
public class CountryRepositoryTests
{
	private ApplicationDbContext _context;
	private CountryRepository _countryRepository;

	[SetUp]
	public void SetUp()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
			.Options;

		_context = new ApplicationDbContext(options);
		_countryRepository = new CountryRepository(_context);

		SeedDatabase(); 
	}

	private void SeedDatabase()
	{
		var countries = new List<Country>
			{
				new Country { Id = Guid.NewGuid(), Name = "USA", Provinces = new List<Province> { new Province { Id = Guid.NewGuid(), Name = "California" } } },
				new Country { Id = Guid.NewGuid(), Name = "Canada", Provinces = new List<Province> { new Province { Id = Guid.NewGuid(), Name = "Ontario" } } }
			};

		_context.Countries.AddRange(countries);
		_context.SaveChanges();
	}

	[Test]
	public async Task GetAllAsync_ShouldReturnAllCountriesWithProvinces()
	{
		var countries = await _countryRepository.GetAllAsync();

		Assert.That(countries, Is.Not.Null);
		Assert.That(countries.Count(), Is.EqualTo(2));

		Assert.That(countries.First().Provinces, Is.Not.Null);
		Assert.That(countries.First().Provinces.Count(), Is.GreaterThan(0));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnCorrectCountry()
	{
		var country = _context.Countries.First();

		var result = await _countryRepository.GetByIdAsync(country.Id);

		Assert.That(result, Is.Not.Null);
		Assert.That(result!.Id, Is.EqualTo(country.Id));
		Assert.That(result.Name, Is.EqualTo(country.Name));
	}

	[Test]
	public async Task GetByIdAsync_ShouldReturnNull_WhenCountryDoesNotExist()
	{
		var result = await _countryRepository.GetByIdAsync(Guid.NewGuid());

		Assert.That(result, Is.Null);
	}

	[TearDown]
	public void TearDown()
	{
		_context.Dispose();
	}
}
