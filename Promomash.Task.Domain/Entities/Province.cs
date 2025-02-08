using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PromomashTask.Domain.Entities;

public class Province
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string Name { get; set; }
	[ForeignKey(nameof(Country))]
	public Guid CountryId { get; set; }
	[JsonIgnore]
	public Country Country { get; set; } = null!;
}
