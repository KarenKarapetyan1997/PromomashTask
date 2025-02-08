using Microsoft.EntityFrameworkCore;
using PromomashTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromomashTask.Infrastructure.Common
{
	public static class DataGenerator
	{
		public static void SeedData(this ModelBuilder builder)
		{
			// Generate new GUIDs for countries
			var country1 = new Country { Id = Guid.Parse("51bc79f6-0c5d-448c-9ce5-1d8fdc15f7c9"), Name = "Country 1" };
			var country2 = new Country { Id = Guid.Parse("8c315951-198e-47ad-b1eb-b9ca401dc883"), Name = "Country 2" };

			builder.Entity<Country>().HasData(country1, country2);

			// Generate new GUIDs for provinces
			var provinces1 = new Province { Id = Guid.Parse("8d73681f-ea00-4d42-a5ab-59eba2d90c5d"), Name = "Province 1.1", CountryId = country1.Id };
			var provinces2 = new Province { Id = Guid.Parse("5c83ce22-c9aa-48f6-b901-544110b38bdd"), Name = "Province 1.2", CountryId = country1.Id };
			var provinces3 = new Province { Id = Guid.Parse("2c35eb01-5fe6-4c42-b073-44b3b2c15c90"), Name = "Province 2.1", CountryId = country2.Id };
			var provinces4 = new Province { Id = Guid.Parse("106b144f-ffd3-4a9f-a029-3c991cd140d5"), Name = "Province 2.2", CountryId = country2.Id };

			builder.Entity<Province>().HasData(provinces1,provinces2,provinces3,provinces4);
		}
	}
}
