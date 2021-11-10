namespace MinimalValidation
{
    public static class ValidateExtensions
    {
        public static IEndpointConventionBuilder Validate<TModel>(this IEndpointConventionBuilder builder) where TModel : class
        {
            builder.Add(endpoint =>
            {
                var original = endpoint.RequestDelegate;
                endpoint.RequestDelegate = async ctx =>
                {
                    var Errors = new List<ValidationResult>();
                    
                    ctx.Request.EnableBuffering();
                    
                    var model = await ctx.Request.ReadFromJsonAsync<TModel>();
                    
                    ArgumentException.ThrowIfNull(model, nameof(model));

                    ValidationContext validator = new(model, null, null);

                    if (!Validator.TryValidateObject(model, validator, Errors, true))
                    {
                        ctx.Response.StatusCode = 400;
                        ctx.Response.ContentType = "application/problem+json";
                        await ctx.Response.WriteAsJsonAsync(Errors);
                        return;
                    }

                    ctx.Request.Body.Position = 0;
                    await original!(ctx);
                };
            });
            return builder;
        }

    }
}