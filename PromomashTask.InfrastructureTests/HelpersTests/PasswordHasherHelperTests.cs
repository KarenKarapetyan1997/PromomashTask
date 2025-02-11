using NUnit.Framework;
using PromomashTask.Infrastructure.Common; 
namespace PromomashTask.InfrastructureTests.HelpersTests;

[TestFixture]
public class PasswordHasherHelperTests
{
	private PasswordHasherHelper _passwordHasher = new(); 

	[SetUp]
	public void Setup()
	{
		_passwordHasher = new PasswordHasherHelper(); 
	}

	[Test]
	public void HashPassword_ShouldReturnHashedPassword()
	{
		var password = "SecurePassword123!";

		var hashedPassword = _passwordHasher.HashPassword(password);

		Assert.That(hashedPassword, Is.Not.Null.And.Not.Empty);
		Assert.That(hashedPassword, Does.Contain(":")); 
		Assert.That(hashedPassword.Split(':').Length, Is.EqualTo(2)); 
	}

	[Test]
	public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
	{
		var password = "SecurePassword123!";
		var hashedPassword = _passwordHasher.HashPassword(password);

		var result = _passwordHasher.VerifyPassword(hashedPassword, password);

		Assert.That(result, Is.True);
	}

	[Test]
	public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
	{
		var password = "SecurePassword123!";
		var incorrectPassword = "WrongPassword456!";
		var hashedPassword = _passwordHasher.HashPassword(password);

		var result = _passwordHasher.VerifyPassword(hashedPassword, incorrectPassword);

		Assert.That(result, Is.False);
	}

	[Test]
	public void VerifyPassword_ShouldReturnFalse_WhenHashedPasswordIsInvalidFormat()
	{
		var invalidHashedPassword = "InvalidFormatHash";

		var result = _passwordHasher.VerifyPassword(invalidHashedPassword, "SomePassword");

		Assert.That(result, Is.False);
	}

	[Test]
	public void VerifyPassword_ShouldReturnFalse_WhenHashedPasswordIsNullOrEmpty()
	{
		Assert.That(_passwordHasher.VerifyPassword(null, "password"), Is.False);
		Assert.That(_passwordHasher.VerifyPassword("", "password"), Is.False);
	}

	[Test]
	public void VerifyPassword_ShouldReturnFalse_WhenInputPasswordIsNullOrEmpty()
	{
		var hashedPassword = _passwordHasher.HashPassword("SecurePassword123!");

		Assert.That(_passwordHasher.VerifyPassword(hashedPassword, null), Is.False);
		Assert.That(_passwordHasher.VerifyPassword(hashedPassword, ""), Is.False);
	}
}
