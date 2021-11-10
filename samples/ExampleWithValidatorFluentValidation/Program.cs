var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapPost("/customer", (Customer customer) =>
 {
     return Results.Created(nameof(customer), customer);
 }).WithValidator<Customer>();

app.Run();

public class Customer
{
    public Customer()
    {

    }
    public Customer(string name, int age, string email)
    {
        Name = name;
        Age = age;
        Email = email;

    }
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
