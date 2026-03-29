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

public class TestCredentialsBuilder
{
    private readonly TestCredentials _credentials = new();
    /// <summary>
    /// Static factory method to create a new TestCredentialsBuilder 
    /// instance for fluent configuration of test credentials
    /// </summary>
    /// <returns></returns>
    public static TestCredentialsBuilder Create() => new();
    /// <summary>
    /// Set email for test credentials,
    /// also updates IsEmailEmpty flag based on whether email is empty or not for validation purposes
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithEmail(string email)
    {
        _credentials.Email = email;
        _credentials.IsEmailEmpty = string.IsNullOrEmpty(email);
        return this;
    }
    /// <summary>
    /// Set password for test credentials,
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithPassword(string password)
    {
        _credentials.Password = password;
        _credentials.IsPasswordEmpty = string.IsNullOrEmpty(password);
        return this;
    }
    /// <summary>
    /// Set description for test credentials, useful for logging and debugging to
    /// understand the purpose of the credentials being used in tests
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithDescription(string? description)
    {
        _credentials.Description = description;
        return this;
    }
    /// <summary>
    /// Set expected username error flag for test credentials, indicating whether a username
    /// error is expected when using these credentials in login tests.
    /// </summary>
    /// <param name="expected"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithExpectedUsernameError(bool expected)
    {
        _credentials.ExpectedUsernameError = expected;
        return this;
    }
    /// <summary>
    /// Set expected password error flag for test credentials, indicating whether a password
    /// </summary>
    /// <param name="expected"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithExpectedPasswordError(bool expected)
    {
        _credentials.ExpectedPasswordError = expected;
        return this;
    }
    /// <summary>
    /// Convenience method to set valid credentials for testing successful login scenarios,
    /// </summary>
    /// <returns></returns>
    public TestCredentialsBuilder WithValidCredentials()
    {
        _credentials.Email = "admin@admin.com";
        _credentials.Password = "admin123";
        return this;
    }
    /// <summary>
    ///     Convenience method to set invalid credentials for testing unsuccessful login scenarios,
    /// </summary>
    /// <param name="isEmpty"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithIsEmailEmpty(bool isEmpty)
    {
        _credentials.IsEmailEmpty = isEmpty;
        return this;
    }
    /// <summary>
    /// Method to set password empty flag for testing login scenarios with empty password field,
    /// </summary>
    /// <param name="isEmpty"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithIsPasswordEmpty(bool isEmpty)
    {
        _credentials.IsPasswordEmpty = isEmpty;
        return this;
    }

    public TestCredentials Build() => _credentials;
}
