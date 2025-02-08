using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromomashTask.Application.Common.Interfaces.PasswordHasherHelper;
using PromomashTask.Application.Common.Interfaces.Repositories;
using PromomashTask.Application.Common.Interfaces.UnitOfWork;
using PromomashTask.Infrastructure.Common;
using PromomashTask.Infrastructure.Repositories;

namespace PromomashTask.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContextPool<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IProvinceRepository, ProvinceRepository>();
			services.AddScoped<IPasswordHasher,PasswordHasherHelper>();

			//Unit Of Work
			services.AddScoped<IUnitOfWork>(serviceProvider =>
				serviceProvider.GetRequiredService<ApplicationDbContext>());

			return services;
		}
	}
}
