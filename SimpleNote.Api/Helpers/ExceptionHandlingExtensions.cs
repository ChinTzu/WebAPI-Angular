using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SimpleNote.Api.Helpers
{
    public static class ExceptionHandlingExtensions
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        var logger = loggerFactory.CreateLogger("SimpleNote.Api.Helpers.ExceptionHandlingExtensions");
                        logger.LogError(500, ex.Error, ex.Error.Message);
                    }

                    await context.Response.WriteAsync(ex?.Error?.Message ?? "An Error Occurred.");
                });
            });
        }
    }
}
