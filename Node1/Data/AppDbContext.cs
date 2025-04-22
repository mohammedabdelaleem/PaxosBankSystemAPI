

namespace Node1.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

	public DbSet<User> Users { get; set; }
	public DbSet<Account> Accounts { get; set; }
	public DbSet<Transaction> Transactions { get; set; }


}