﻿using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration configuration;

		public AuthController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[HttpPost]
		public IActionResult Authenticate([FromBody] Credential credential)
		{
			if (credential.UserName == "admin" && credential.Password == "password")
			{
				// creating the security context
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, "admin"),
					new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
					new Claim("Department", "HR"),
					new Claim("Admin", "true"),
					new Claim("Manager", "true"),
					new Claim("EmploymentDate", "2022-02-01")
				};

				var expiresAt = DateTime.UtcNow.AddMinutes(10);

				return Ok(new
				{
					access_token = CreateToken(claims, expiresAt),
					expires_at = expiresAt,
				});
			}

			ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint");
			return Unauthorized(ModelState);
		}


		private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
		{
			var securityKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

			var jwt = new JwtSecurityToken(
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: expiresAt,
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(securityKey),
					SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

	}


	public class Credential
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
