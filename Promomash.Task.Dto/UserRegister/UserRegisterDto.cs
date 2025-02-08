using System.ComponentModel.DataAnnotations;

namespace PromomashTask.Dto.UserRegister
{

	public class UserRegisterDto
	{
		[Required]
		[EmailAddress]
		public required string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public required string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		public required string CofirmPassword {  get; set; }
		[Required]
		public required Guid CountryId { get; set; }
		[Required]
		public required Guid ProvinceId { get; set; }
	}
}
