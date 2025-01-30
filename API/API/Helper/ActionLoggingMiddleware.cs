using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace API.Middleware
{
    public class ActionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ActionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestLog = await FormatRequest(context.Request);

            await WriteLogAsync($"Request:\n{requestLog}");

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var responseLog = await FormatResponse(context.Response);

            await WriteLogAsync($"Response:\n{responseLog}");

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var bodyAsText = "";
            if (request.ContentLength > 0)
            {
                using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
                bodyAsText = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            return $"Method: {request.Method}\nPath: {request.Path}\nBody: {bodyAsText}\n";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"Status Code: {response.StatusCode}\nBody: {text}\n";
        }

        private async Task WriteLogAsync(string log)
        {
            var logFilePath = System.IO.Path.Combine(
             @"D:\code\yedek\InkSpire.AI-main\backend\Inkspire.AI\API\API\Logs",
             $"{System.DateTime.Now:yyyy-MM-dd}_action_log.txt");
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(logFilePath)))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(logFilePath)!);
            }

            var logMessage = $"{System.DateTime.Now} - {log}\n\n";
            await File.AppendAllTextAsync(logFilePath, logMessage);
        }
    }
}
