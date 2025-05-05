using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Store.App.Crosscutting.Commom.Logging;

namespace Store.App.Crosscutting.Commom.HandleError
{
    public static class ExceptionHandler
    {
        public static WebApplication HandlerInternalError(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                    await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                    {
                        await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                    }

                    LogWriter.GenerateDefaultLog(exceptionHandlerPathFeature.Error.StackTrace);
                    
                    await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                    await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.StackTrace);
                    await context.Response.WriteAsync("</body></html>\r\n");
                    await context.Response.WriteAsync(new string(' ', 512));
                });
            });

            return app;
        }
    }
}
