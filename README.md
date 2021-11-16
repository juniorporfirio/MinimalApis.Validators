# MinimalApis.Validators
The easiest way to validate web apis from .NET6 MinimalApis.

Inspired in an example of [Nick Chapsa](https://github.com/Elfocrash).

<b>Thank's a lot Nick Chapsa !!!</b>

## Nuget Packages

### Core Library

* [MinimalApis.Validators](src/MinimalApis.Validators/DataAnnotationValidatorExtension.cs) - Validator using DataAnnotation.
* [MiimalApis.Validators.FluentValidation](src/MinimalValidator/FluentValidationExtension.cs) - Validator using package [FluentValidation](https://fluentvalidation.net/).

## How can use it
Only use the key <b>WithValidator<></b> in your endpoint and finish:
```csharp
app.MapPost("/customer",(Customer customer) =>
{
    return Results.Created(nameof(customer), customer);
}).WithValidator<Customer>();
```

## Basic Usage DataAnnotation

```csharp
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


```
More details in source example [Example With DataAnnotation](samples/ExampleWithValidatorDataAnnotation)


## Basic Usage FluentValidation

```csharp
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



```
More details in source example [Example With FluentValidation](samples/ExampleWithValidatorFluentValidation)

## Credits and contributors


<b>Special credit to [Nick Chapsa](https://github.com/Elfocrash) to made it's possible.</b>


Maintained by Junior Porfirio - follow me  on twitter **[@juniorporfirio](https://twitter.com/juniorporfirio)** for updates.
