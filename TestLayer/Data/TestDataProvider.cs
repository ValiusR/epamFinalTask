using BusinessLayer.Models;

namespace TestLayer.Data;

// test data for data-driven tests
public static class TestDataProvider
{
    // valid credentials
    public static IEnumerable<object[]> ValidCredentialsData
    {
        get
        {
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("test@qabrains.com").WithPassword("Password123").WithDescription("Primary test user").Build() };
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("practice@qabrains.com").WithPassword("Password123").WithDescription("Practice user").Build() };
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("student@qabrains.com").WithPassword("Password123").WithDescription("Student user").Build() };
        }
    }

    // invalid creds for login test
    public static IEnumerable<object[]> InvalidCredentialsData
    {
        get
        {
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("invalid@email.com").WithPassword("wrongpass").WithExpectedUsernameError(true).WithExpectedPasswordError(true).Build() };
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("fake@fake.com").WithPassword("123456").WithExpectedUsernameError(true).WithExpectedPasswordError(true).Build() };
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("").WithPassword("").WithExpectedUsernameError(true).WithExpectedPasswordError(true).WithDescription("Empty credentials").Build() };
            yield return new object[] { TestCredentialsBuilder.Create().WithEmail("test@test.com").WithPassword("password").WithExpectedUsernameError(true).WithExpectedPasswordError(true).WithDescription("Random invalid credentials").Build() };
        }
    }

    // sort options
    public static IEnumerable<object[]> SortOptionsData
    {
        get
        {
            yield return new object[] { "Price: Low to High" };
            yield return new object[] { "Price: High to Low" };
            yield return new object[] { "Name: A to Z" };
            yield return new object[] { "Name: Z to A" };
            yield return new object[] { "Newest" };
        }
    }

}
