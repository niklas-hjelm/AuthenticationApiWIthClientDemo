namespace AuthenticationApiWIthClientDemo.Domain.DTOs;

public class UserRegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}