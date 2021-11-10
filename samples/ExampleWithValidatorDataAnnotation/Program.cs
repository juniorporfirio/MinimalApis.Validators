var app = WebApplication.Create(args);
app.MapPost("/customer", (Customer customer) =>
 {
     return Results.Created(nameof(customer), customer);
 }).WithValidator<Customer>();

app.Run();

public class Customer
{
    public Customer(string name, int age, string email)
    {
        Name = name;
        Age = age;
        Email = email;
    }
    [Required]
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}