using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProjectPlutus.Extensions.Models;
using System.Net;

namespace ProjectPlutus.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(
            this IApplicationBuilder app,
            ILogger logger,
            bool isProdEnvironment)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        logger.LogError(
                            LoggingEvents.ServerError,
                            $"Error Message: {contextFeature.Error.Message} " +
                            $"\n Stack Trace: {contextFeature.Error.StackTrace}");

                        BaseExceptionDetails exceptionDetails = new BaseExceptionDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        };

                        if (!isProdEnvironment)
                        {
                            exceptionDetails = new DevExceptionDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message,
                                Stacktrace = contextFeature.Error.StackTrace
                            };
                        }


                        await context.Response.WriteAsync(exceptionDetails.ToString());
                    }
                });
            });
        }
    }
}
