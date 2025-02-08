using MediatR;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Common.Interfaces.UnitOfWork;
using PromomashTask.Domain.Entities;

namespace PromomashTask.Application.Commands.UserRegister;

public class AddUserRegisterCommandHandler : IRequestHandler<AddUserRegisterCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public AddUserRegisterCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;	
	}

	public async Task Handle(AddUserRegisterCommand request,CancellationToken cancellationToken)
	{
		var user = new User
		{
			Email = request.Email,
			Password = request.Password,
			CountryId = request.CountryId,
			ProvinceId = request.ProvinceId,
		};

		await _userRepository.AddAsync(user);
		await _unitOfWork.CommitChangesAsync(cancellationToken);
	}
}
