using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuthenticationApiWIthClientDemo.Client.Auth;

/// <summary>
/// Custom Authentication State Provider som vi kan använda för att hålla reda på om användaren är inloggad eller inte.
/// </summary>
public class CustomAuthStateProvider : AuthenticationStateProvider
{
	private readonly UserService _userService;

	public CustomAuthStateProvider(UserService userService)
	{
		_userService = userService;
	}
	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var identity = _userService.GetUser();
		var user = new ClaimsPrincipal(identity);
		return Task.FromResult(new AuthenticationState(user));
	}
}