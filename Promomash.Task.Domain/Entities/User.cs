namespace PromomashTask.Domain.Entities;

public class User
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string Email { get; set; } 
	public required string Password { get; set; } 
	public Guid CountryId { get; set; }
	public Guid ProvinceId { get; set; }
}
