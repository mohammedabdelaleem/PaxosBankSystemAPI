using System.ComponentModel.DataAnnotations;

namespace Node1.Models;

public class TransferRequestDTO
{

	[Required]
	public int ToUserId { get; set; }


	[Required]
	public decimal Amount { get; set; }
}