﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Node1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
	private readonly IUserService _userService = userService;

	[HttpGet("all")]
	public async Task<ActionResult> GetAll()
	{
		var users = await _userService.GetAll();
		return users!= null? Ok(users) : NotFound(new {message = "No Users Found"});
	}


}
