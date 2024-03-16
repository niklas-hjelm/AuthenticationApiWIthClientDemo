using System.Net.Http.Headers;
using System.Security.Claims;
using AuthenticationApiWIthClientDemo.Domain.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuthenticationApiWIthClientDemo.Client.Auth;

/// <summary>
/// AuthService Hanterar Login och Register.
/// Här skickas http-requests till APIet 
/// </summary>
public class AuthService
{
    private readonly UserService _userService;
    private readonly HttpClient _httpClient;

    public AuthService(IHttpClientFactory httpClientFactory, UserService userService)
    {
        _userService = userService;
        _httpClient = httpClientFactory.CreateClient("AuthenticationApi");
    }

    public async Task<bool> Register(UserRegisterDto registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("/register", registerModel);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> Login(UserLoginDto loginModel)
    {
        var result = await _httpClient.PostAsJsonAsync("/login", loginModel);
        if (result.IsSuccessStatusCode)
        {
            // Hömta ut token och refresh-token
            var response = await result.Content.ReadFromJsonAsync<LoginResponseDto>();
            var name = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Email),
                new Claim("token", response.AccessToken),
                new Claim("refreshToken", response.RefreshToken)
            }, "bearer");

            // Spara användaren i UserService
            _userService.SetUser(new ClaimsPrincipal(name));
            return true;
        }
        return false;
    }
}