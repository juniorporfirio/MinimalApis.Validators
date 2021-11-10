using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MinimalApis.Validators.DataAnnotation.Test;

public class ValidatorWithDataAnnotationTest
{

    [Fact]
    public async Task Must_Returning_Success_With_Content_Valid()
    {
        // Arrange
       using var application = new MinimalApiApplication();
        var client = application.CreateClient();
        var customer = new Customer("John", 30, "john@gmail.com");

        // Act
        var response = await client.PostAsJsonAsync("/customer", customer);
        var result = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var customerResult = JsonSerializer.Deserialize<Customer>(result, options);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        customerResult.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task Must_Returning_BadRequest_With_Content_Invalid()
    {
        // Arrange
        using var application = new MinimalApiApplication();
        var client = application.CreateClient();
        var customer = new Customer("John", 30, "");

        // Act
        var response = await client.PostAsJsonAsync("/customer", customer);
        var result = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Should().Contain("The Email field is not a valid e-mail address.");
    }
}