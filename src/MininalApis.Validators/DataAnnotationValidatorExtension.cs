namespace MiminalApis.Validators;

public static class DataAnnotationValidatorExtension
{
    public static IEndpointConventionBuilder WithValidator<TModel>(this IEndpointConventionBuilder builder) where TModel : class
    {
        builder.Add(endpoint =>
        {
            var original = endpoint.RequestDelegate;
            endpoint.RequestDelegate = async ctx =>
            {
                var errors = new List<ValidationResult>();

                ctx.Request.EnableBuffering();
                var model = await ctx.Request.ReadFromJsonAsync<TModel>();

                ArgumentNullException.ThrowIfNull(model, nameof(model));
                ValidationContext validator = new(model, null, null);

                if (!Validator.TryValidateObject(model, validator, errors, true))
                {
                    ctx.Response.StatusCode = 400;
                    ctx.Response.ContentType = "application/problem+json";
                    await ctx.Response.WriteAsJsonAsync(errors);
                    return;
                }
                ctx.Request.Body.Position = 0;
                await original!(ctx);
            };
        });
        return builder;
    }

}