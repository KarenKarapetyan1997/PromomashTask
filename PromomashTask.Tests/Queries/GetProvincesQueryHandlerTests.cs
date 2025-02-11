using Moq;
using NUnit.Framework;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Queries.Provinces;
using PromomashTask.Domain.Entities;

namespace PromomashTask.ApplicationTests.Queries;

[TestFixture]
public class GetProvincesQueryHandlerTests
{
	private Mock<IProvinceRepository> _provinceRepositoryMock = new();
	private GetProvincesQueryHandler _handler;

	[SetUp]
	public void Setup()
	{
		_provinceRepositoryMock = new Mock<IProvinceRepository>();
		_handler = new GetProvincesQueryHandler(_provinceRepositoryMock.Object);
	}

	[Test]
	public async Task Handle_ShouldReturnListOfProvinces_WhenProvincesExist()
	{
		// Arrange
		var countryId = Guid.NewGuid();
		var expectedProvinces = new List<Province>
			{
				new Province { Id = Guid.NewGuid(), Name = "California", CountryId = countryId },
				new Province { Id = Guid.NewGuid(), Name = "Texas", CountryId = countryId }
			};

		_provinceRepositoryMock
			.Setup(repo => repo.GetAllProvincesByCountryAsync(countryId))
			.ReturnsAsync(expectedProvinces);

		var query = new GetProvincesQuery (countryId);

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(expectedProvinces));
		Assert.That(result.Count(), Is.EqualTo(2));
		Assert.That(result, Does.Contain(expectedProvinces[0]));
		Assert.That(result, Does.Contain(expectedProvinces[1]));

		_provinceRepositoryMock.Verify(repo => repo.GetAllProvincesByCountryAsync(countryId), Times.Once);
	}

	[Test]
	public async Task Handle_ShouldReturnEmptyList_WhenNoProvincesExist()
	{
		// Arrange
		var countryId = Guid.NewGuid();
		_provinceRepositoryMock
			.Setup(repo => repo.GetAllProvincesByCountryAsync(countryId))
			.ReturnsAsync((List<Province>?)null); // Simulating a null return

		var query = new GetProvincesQuery (countryId);

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.Empty);

		_provinceRepositoryMock.Verify(repo => repo.GetAllProvincesByCountryAsync(countryId), Times.Once);
	}
}
