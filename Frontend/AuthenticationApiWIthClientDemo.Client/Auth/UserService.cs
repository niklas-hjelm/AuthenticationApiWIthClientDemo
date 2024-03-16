using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace AuthenticationApiWIthClientDemo.Client.Auth;

/// <summary>
/// UserService håller reda på användaren för den aktuella sessionen. 
/// </summary>
public class UserService
{
	private ClaimsPrincipal currentUser = new(new ClaimsIdentity());

	public ClaimsPrincipal GetUser()
	{
		return currentUser;
	}

	internal void SetUser(ClaimsPrincipal user)
	{
		if (currentUser != user)
		{
			currentUser = user;
		}
	}
}

/// <summary>
/// UserCircuitHandler uppdaterar användaren när användaren loggar in eller ut.
/// Här används CircuitHandler för att lyssna på när användaren loggar in eller ut.
/// Eventet AuthenticationStateChanged triggar när användaren loggar in eller ut.
/// </summary>
internal sealed class UserCircuitHandler : CircuitHandler, IDisposable
{
	private readonly AuthenticationStateProvider authenticationStateProvider;
	private readonly UserService userService;

	public UserCircuitHandler(
		AuthenticationStateProvider authenticationStateProvider,
		UserService userService)
	{
		this.authenticationStateProvider = authenticationStateProvider;
		this.userService = userService;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit,
		CancellationToken cancellationToken)
	{
		authenticationStateProvider.AuthenticationStateChanged +=
			AuthenticationChanged;

		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	private void AuthenticationChanged(Task<AuthenticationState> task)
	{
		_ = UpdateAuthentication(task);

		async Task UpdateAuthentication(Task<AuthenticationState> task)
		{
			try
			{
				var state = await task;
				userService.SetUser(state.User);
			}
			catch
			{
			}
		}
	}

	public override async Task OnConnectionUpAsync(Circuit circuit,
		CancellationToken cancellationToken)
	{
		var state = await authenticationStateProvider.GetAuthenticationStateAsync();
		userService.SetUser(state.User);
	}

	public void Dispose()
	{
		authenticationStateProvider.AuthenticationStateChanged -=
			AuthenticationChanged;
	}
}