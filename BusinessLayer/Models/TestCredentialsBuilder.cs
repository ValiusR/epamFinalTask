namespace BusinessLayer.Models;

public class TestCredentialsBuilder
{
    private readonly TestCredentials _credentials = new();
    public static TestCredentialsBuilder Create() => new();
    /// <summary>
    /// Set email for test credentials,
    /// also updates IsEmailEmpty flag based on whether email is empty or not
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
    /// Set description for test credentials
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithDescription(string? description)
    {
        _credentials.Description = description;
        return this;
    }
    /// <summary>
    /// Set expected username error flag for test credentials
    /// </summary>
    /// <param name="expected"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithExpectedUsernameError(bool expected)
    {
        _credentials.ExpectedUsernameError = expected;
        return this;
    }
    /// <summary>
    /// Set expected password error flag for test credentials
    /// </summary>
    /// <param name="expected"></param>
    /// <returns></returns>
    public TestCredentialsBuilder WithExpectedPasswordError(bool expected)
    {
        _credentials.ExpectedPasswordError = expected;
        return this;
    }

    public TestCredentials Build() => _credentials;
}
