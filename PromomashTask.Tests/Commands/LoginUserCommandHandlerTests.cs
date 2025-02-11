using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PromomashTask.Application.Commands.UserLogin;
using PromomashTask.Application.DTOs.UserLogin;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;
using PromomashTask.Domain.Entities;
using System.Collections.Generic;

namespace PromomashTask.ApplicationTests.Commands;

[TestFixture]
public class LoginUserCommandHandlerTests
{
	private Mock<IUserRepository> _userRepositoryMock = new();
	private Mock<IConfiguration> _configurationMock = new();
	private Mock<IPasswordHasher> _passwordHasherMock = new();
	private LoginUserCommandHandler _handler;

	[SetUp]
	public void Setup()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_configurationMock = new Mock<IConfiguration>();
		_passwordHasherMock = new Mock<IPasswordHasher>();

		_handler = new LoginUserCommandHandler(
			_userRepositoryMock.Object,
			_configurationMock.Object,
			_passwordHasherMock.Object);

		var jwtSettings = new Dictionary<string, string>
			{
				{ "JwtSettings:SecretKey", "a-very-secure-secret-key" },
				{ "JwtSettings:Issuer", "TestIssuer" },
				{ "JwtSettings:Audience", "TestAudience" },
				{ "JwtSettings:ExpiryMinutes", "60" }
			};

		foreach (var setting in jwtSettings)
		{
			_configurationMock.Setup(x => x[setting.Key]).Returns(setting.Value);
		}
	}

	[Test]
	public async Task Handle_ValidCredentials_ReturnsToken()
	{
		// Arrange
		var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Password = "hashedPassword" };
		var userRequest = new UserRequestDto { Email = "test@example.com", Password = "correctPassword" };
		var request = new LoginUserCommand(userRequest);

		_userRepositoryMock.Setup(repo => repo.GetByEmailAsync("test@example.com"))
			.ReturnsAsync(user);

		_passwordHasherMock.Setup(hasher => hasher.VerifyPassword("hashedPassword", "correctPassword"))
			.Returns(true);

		var response = await _handler.Handle(request, CancellationToken.None);

		Assert.That(response.Tocken, Is.Not.Null);
		Assert.That(response.RefreshTocken, Is.Not.Null);
	}

	[Test]
	public void Handle_InvalidCredentials_ThrowsUnauthorizedAccessException()
	{
		var userRequest = new UserRequestDto { Email = "test@example.com", Password = "correctPassword" };
		var request = new LoginUserCommand(userRequest);


		_userRepositoryMock.Setup(repo => repo.GetByEmailAsync("test@example.com"))
			.ReturnsAsync((User?)null);


		Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
			await _handler.Handle(request, CancellationToken.None));
	}

	[Test]
	public void GenerateJwtToken_MissingSecretKey_ThrowsInvalidOperationException()
	{
		_configurationMock.Setup(x => x["JwtSettings:SecretKey"]).Returns(string.Empty);

		var userId = Guid.NewGuid();
		var email = "test@example.com";

		Assert.Throws<InvalidOperationException>(() => _handler.GenerateJwtToken(userId, email));
	}
}

