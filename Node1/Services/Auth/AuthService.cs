
namespace Node1.Services.Auth;

public class AuthService : IAuthService
{
	private readonly AppDbContext _context;
	private readonly IConfiguration _config;

	public AuthService(AppDbContext context, IConfiguration config)
	{
		_context = context;
		_config = config;
	}

	public async Task<string> RegisterAsync(RegisterRequestDTO request)
	{
		if (_context.Users.Any(u => u.Username == request.Username))
			throw new Exception("Username already exists.");

		var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

		var user = new User
		{
			Username = request.Username,
			PasswordHash = passwordHash
		};

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return GenerateJwtToken(user);
	}

	public async Task<string> LoginAsync(LoginRequestDTO request)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
		if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
			throw new Exception("Invalid username or password.");

		return GenerateJwtToken(user);
	}

	private string GenerateJwtToken(User user)
	{
		var claims = new[] {
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Username)
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"]));

		var token = new JwtSecurityToken(
			issuer: _config["Jwt:Issuer"],
			audience: _config["Jwt:Audience"],
			claims: claims,
			expires: expires,
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
