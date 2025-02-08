namespace PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;

public interface IPasswordHasher
{
	string HashPassword(string password);
	bool VerifyPassword(string hashedPassword, string inputPassword);
}
