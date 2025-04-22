
namespace Node1.Services;

public class UserService(AppDbContext context) : IUserService
{
	private readonly AppDbContext _context = context;

	public async Task<UserInfoDTO> GetUserInfoAsync(int userId)
	{
		var user = await _context.Users
			.Include(u => u.Accounts)  // Assuming you have a navigation property for Accounts in User entity
			.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null)
		{
			throw new Exception("User not found.");
		}

		var userInfoDTO = new UserInfoDTO
		{
			Id = user.Id,
			Name = user.Username,
			AccountsId = user.Accounts.Select(a => a.Id).ToList() // Map to Account IDs
		};

		return userInfoDTO;
	}

	public async Task<List<UserInfoDTO>> GetAll()
	{
		return await _context.Users.Select(u=> new UserInfoDTO
		{
			Id = u.Id,
			Name = u.Username,
			AccountsId = u.Accounts.Select(a=>a.Id).ToList()
		}).ToListAsync();
	}
}
