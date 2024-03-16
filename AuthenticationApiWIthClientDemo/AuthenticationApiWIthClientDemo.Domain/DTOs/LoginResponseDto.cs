namespace AuthenticationApiWIthClientDemo.Domain.DTOs;

public class LoginResponseDto
{
	public string TokenType { get; set; } = "Bearer";
	public int ExpiresIn { get; set; }
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
}