var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapPost("/customer",(Customer customer) =>
{
    return Results.Created(nameof(customer),customer);
}).WithValidator<Customer>();

app.Run("http://localhost:5005");

public class Customer
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Email { get; set; } = "";
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0);
        RuleFor(x => x.Email).EmailAddress();
    }
}
