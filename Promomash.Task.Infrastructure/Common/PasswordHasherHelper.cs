using PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;
using System.Security.Cryptography;

namespace PromomashTask.Infrastructure.Common;

public class PasswordHasherHelper:IPasswordHasher
{
	private const int Iterations = 100_000;
	private const int SaltSize = 16;
	private const int HashSize = 64;
	private const char Delimiter = ':';

	public string HashPassword(string password)
	{
		// Generate random salt
		using var rng = RandomNumberGenerator.Create();
		var salt = new byte[SaltSize];
		rng.GetBytes(salt);

		// Create hash value
		using var pbkdf2 = new Rfc2898DeriveBytes(
			password: password,
			salt: salt,
			iterations: Iterations,
			hashAlgorithm: HashAlgorithmName.SHA512
		);

		var hash = pbkdf2.GetBytes(HashSize);

		return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
	}

	public bool VerifyPassword(string hashedPassword, string inputPassword)
	{
		if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(inputPassword))
			return false;

		try
		{
			// Split stored password
			var parts = hashedPassword.Split(Delimiter);
			var salt = Convert.FromBase64String(parts[0]);
			var storedHash = Convert.FromBase64String(parts[1]);

			// Compute hash of input password
			using var pbkdf2 = new Rfc2898DeriveBytes(
				password: inputPassword,
				salt: salt,
				iterations: Iterations,
				hashAlgorithm: HashAlgorithmName.SHA512
			);

			var computedHash = pbkdf2.GetBytes(storedHash.Length);

			// Compare hashes using constant-time comparison
			return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
		}
		catch
		{
			// Log exception if needed
			return false;
		}
	}
}