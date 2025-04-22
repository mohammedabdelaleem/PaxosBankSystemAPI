namespace Node1;

public static class DependencyInjection
{
	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		// Add DbContext
		services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
																	 
		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();


		// Register scoped services
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IAccountService, AccountService>();
		services.AddScoped<ITransactionService, TransactionService>();
		services.AddScoped<IUserService, UserService>();


		// Configure Authentication with JWT Bearer
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
				};
			});

		// Add Authorization
		services.AddAuthorization();
		services.AddHttpClient(); // Registers IHttpClientFactory

		return services;
	}
}
