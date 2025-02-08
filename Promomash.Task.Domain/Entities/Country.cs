namespace PromomashTask.Domain.Entities;

public class Country
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string Name { get; set; }
	public ICollection<Province> Provinces { get; set; } = [];
}
