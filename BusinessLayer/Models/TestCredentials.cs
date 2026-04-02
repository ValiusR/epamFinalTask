namespace BusinessLayer.Models;

public class TestCredentials
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool ExpectedUsernameError { get; set; }
    public bool ExpectedPasswordError { get; set; }
    
    // for empty field testing
    public bool IsEmailEmpty { get; set; }
    public bool IsPasswordEmpty { get; set; }

    public override string ToString() => $"Email: {Email}, Password: {Password}, Description: {Description}";
}
