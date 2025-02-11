using Moq;
using NUnit.Framework;
using PromomashTask.Application.Commands.UserRegister;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Common.Interfaces.UnitOfWork;
using PromomashTask.Domain.Entities;

namespace PromomashTask.ApplicationTests.Commands;

public class AddUserRegisterCommandHandlerTests
{
	private Mock<IUserRepository> _userRepositoryMock = new();
	private Mock<IUnitOfWork> _unitOfWorkMock = new();
	private AddUserRegisterCommandHandler _handler;

	[SetUp]
	public void Setup()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_unitOfWorkMock = new Mock<IUnitOfWork>();

		_handler = new AddUserRegisterCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
	}

	[Test]
	public async Task Handle_ValidRequest_AddsUserAndCommitsTransaction()
	{
		var command = new AddUserRegisterCommand(
		"test@example.com",
		"securePassword",
		Guid.NewGuid(),
		Guid.NewGuid());

		await _handler.Handle(command, CancellationToken.None);

		_userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
			u.Email == command.Email &&
			u.Password == command.Password &&
			u.CountryId == command.CountryId &&
			u.ProvinceId == command.ProvinceId
		)), Times.Once);

		_unitOfWorkMock.Verify(uow => uow.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Test]
	public void Handle_ThrowsException_WhenUnitOfWorkFails()
	{
		var command = new AddUserRegisterCommand(
		"test@example.com",
		"securePassword",
		Guid.NewGuid(),
		Guid.NewGuid());

		_unitOfWorkMock.Setup(uow => uow.CommitChangesAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new Exception("Database error"));

		Assert.ThrowsAsync<Exception>(async () =>
			await _handler.Handle(command, CancellationToken.None));

		_userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
	}
}
