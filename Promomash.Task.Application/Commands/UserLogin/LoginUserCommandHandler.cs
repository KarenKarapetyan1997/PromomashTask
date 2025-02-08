using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.DTOs.UserLogin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PromomashTask.Application.Commands.UserLogin
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserResponseDto>
	{
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;
		private readonly IPasswordHasher _passwordHasher;

		public LoginUserCommandHandler(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher passwordHasher)
		{
			_configuration = configuration;
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
		}

		public async Task<UserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByEmailAsync(request.userRequest.Email);
			if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.userRequest.Password))
			{
				throw new UnauthorizedAccessException("Invalid credentials");
			}

			string accessToken = GenerateJwtToken(user.Id, user.Email);
			string refreshToken = GenerateRefreshToken();

			return new UserResponseDto
			{
				Tocken = accessToken,
				RefreshTocken = refreshToken
			};
		}

		public string GenerateJwtToken(Guid userId, string email)
		{
			// Retrieve the secret key from configuration
			var secretKey = _configuration["JwtSettings:SecretKey"];
			if (string.IsNullOrEmpty(secretKey))
			{
				throw new InvalidOperationException("Secret key for JWT is not configured.");
			}

			// Ensure the key is 256 bits (32 bytes) for HS256 algorithm
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			if (key.Key.Length < 32)
			{
				var hashedKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
				key = new SymmetricSecurityKey(hashedKey); // Ensure 256 bits key length
			}

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			// Create the claims
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				new Claim(ClaimTypes.Email, email),
                // Add other claims as needed
            };

			// Fetch expiry time from configuration
			var expiryMinutesString = _configuration["JwtSettings:ExpiryMinutes"];
			if (string.IsNullOrEmpty(expiryMinutesString) || !int.TryParse(expiryMinutesString, out int expiryMinutes))
			{
				// Set a default expiry time of 60 minutes if it's not provided or invalid
				expiryMinutes = 60;
			}

			// Log to debug values
			Console.WriteLine("Secret Key: " + secretKey);
			Console.WriteLine("Key Length: " + key.Key.Length); // Log key length
			Console.WriteLine("Expiry Minutes: " + expiryMinutes); // Log expiry time
			Console.WriteLine("Claims: " + string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}"))); // Log claims

			// Create the JWT token
			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(expiryMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private string GenerateRefreshToken()
		{
			// Generate a unique refresh token
			return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		}
	}
}
