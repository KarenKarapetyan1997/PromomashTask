using Moq;
using NUnit.Framework;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Queries.Countries;
using PromomashTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromomashTask.ApplicationTests.Queries;

[TestFixture]
public class GetCountriesQueryHandlerTests
{
	private Mock<ICountryRepository> _countryRepositoryMock = new();
	private GetCountriesQueryHandler _handler;

	[SetUp]
	public void Setup()
	{
		_countryRepositoryMock = new Mock<ICountryRepository>();
		_handler = new GetCountriesQueryHandler(_countryRepositoryMock.Object);
	}

	[Test]
	public async Task Handle_ShouldReturnListOfCountries_WhenCountriesExist()
	{
		// Arrange
		var expectedCountries = new List<Country>
			{
				new Country { Id = Guid.NewGuid(), Name = "USA" },
				new Country { Id = Guid.NewGuid(), Name = "Canada" }
			};

		_countryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCountries);

		var query = new GetCountriesQuery();

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(expectedCountries));
		Assert.That(result, Has.Count.EqualTo(2));
		Assert.That(result, Does.Contain(expectedCountries[0]));
		Assert.That(result, Does.Contain(expectedCountries[1]));

		_countryRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
	}

	[Test]
	public async Task Handle_ShouldReturnEmptyList_WhenNoCountriesExist()
	{
		// Arrange
		_countryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Country>());

		var query = new GetCountriesQuery();

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.Empty);

		_countryRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
	}
}
