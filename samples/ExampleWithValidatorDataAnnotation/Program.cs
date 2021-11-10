var app = WebApplication.Create(args);
app.MapPost("/customer",(Customer customer) =>
{
    return Results.Created(nameof(customer), customer);
}).WithValidator<Customer>();

app.Run("http://localhost:5010");

public class Customer
{
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public int Age { get; set; }
    [EmailAddress]
    public string Email { get; set; } = "";
}