using Scalar.AspNetCore;

namespace minimalAPIStructure.Extensions;

public static class OpenApiExtensions
{
    public static void ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new()
                {
                    Title = "Minimal API Patterns",
                    Version = "v1",
                    Description = """
                        <img src="/images/CF_Logo_WO.png" height="120" />  

                        Design Patterns for building minimal APIs
                        """
                };

                return Task.CompletedTask;
            });
        });
    }

    public static void MapScalar(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference("", opt =>
        {
            opt.Title = "Minimal API Server";
            opt.Theme = ScalarTheme.Mars;
        });
    }
}
