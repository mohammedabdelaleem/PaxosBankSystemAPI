﻿namespace Node1.Models;

public class Transaction
{
	public int Id { get; set; }
	public int FromUserId { get; set; }
	public int ToUserId { get; set; }
	public decimal Amount { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.Now;

	public User FromUser { get; set; }
	public User ToUser { get; set; }
}

