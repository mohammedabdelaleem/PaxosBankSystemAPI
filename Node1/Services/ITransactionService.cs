namespace Node1.Services;

public interface ITransactionService
{
	Task<(bool Success, string? ErrorMessage, int? TransactionId)> InitiateTransferAsync(int fromUserId, int toUserId, decimal amount);
	Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(int userId);
}