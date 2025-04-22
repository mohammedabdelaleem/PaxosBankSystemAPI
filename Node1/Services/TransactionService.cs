namespace Node1.Services;
public class TransactionService : ITransactionService
{
	private readonly AppDbContext _context;

	public TransactionService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<(bool Success, string? ErrorMessage, int? TransactionId)> InitiateTransferAsync(int fromUserId, int toUserId, decimal amount)
	{
		if (fromUserId == toUserId)
		{
			return (false, "Cannot transfer to yourself.", null);
		}

		// Get the accounts by user IDs
		var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == fromUserId);
		var toAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == toUserId);

		if (fromAccount == null || toAccount == null)
		{
			return (false, "Account not found.", null);
		}

		if (fromAccount.Balance < amount)
		{
			return (false, "Insufficient balance.", null);
		}

		// TODO: Paxos logic will go here instead of direct DB update

		// Update balances
		fromAccount.Balance -= amount;
		toAccount.Balance += amount;

		// Create a transaction record
		var transaction = new Transaction
		{
			FromUserId = fromUserId,
			ToUserId = toUserId,
			Amount = amount,
			Timestamp = DateTime.UtcNow
		};

		_context.Transactions.Add(transaction);
		await _context.SaveChangesAsync();

		return (true, null, transaction.Id);
	}

	public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(int userId)
	{
		return await _context.Transactions
			.Where(t => t.FromUserId == userId || t.ToUserId == userId)
			.OrderByDescending(t => t.Timestamp)
			.ToListAsync();
	}
}
