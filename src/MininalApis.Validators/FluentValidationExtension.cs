namespace MiminalApis.Validators.FluentValidation;

public static class FluentValidationExtension
{
    public static IEndpointConventionBuilder WithValidator<TModel>(this IEndpointConventionBuilder builder) where TModel : class
    {
        builder.Add(endpoint =>
        {
            var original = endpoint.RequestDelegate;
            endpoint.RequestDelegate = async ctx =>
            {
                var validator = ctx.RequestServices.GetService<Fluent.IValidator<TModel>>();

                ctx.Request.EnableBuffering();
                var model = await ctx.Request.ReadFromJsonAsync<TModel>();

                ArgumentNullException.ThrowIfNull(model, nameof(model));
                
                var result = await validator.ValidateAsync(model);
                if (!result.IsValid)
                {
                    ctx.Response.StatusCode = 400;
                    ctx.Response.ContentType = "application/problem+json";
                    await ctx.Response.WriteAsJsonAsync(new { errors = result.ToDictionary() });
                    return;
                }
                ctx.Request.Body.Position = 0;
                await original!(ctx);
            };
        });
        return builder;
    }

    internal static IDictionary<string, string[]> ToDictionary(this Fluent.Results.ValidationResult validationResult)
        => validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );
}
