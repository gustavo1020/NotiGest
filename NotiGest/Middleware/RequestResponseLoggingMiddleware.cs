using System.Text;
using System.Text.Json;

namespace NotiGest.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string requestBody = await ReadRequestBodyAsync(context);

                LogRequest(context.Request, requestBody);

                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

                await _next(context);

                await LogResponseAsync(context.Response);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al leer el cuerpo de la solicitud como JSON: {ex.Message}");
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private void LogRequest(HttpRequest request, string requestBody)
        {
            Console.WriteLine($"Solicitud recibida: {request.Method} {request.Path}");

            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }

            Console.WriteLine($"Cuerpo de la solicitud: {requestBody}");
        }
        private async Task LogResponseAsync(HttpResponse response)
        {
            try
            {
                Console.WriteLine($"Respuesta enviada: {response.StatusCode}");

                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }

                var originalResponseBody = response.Body;

                using (var responseBodyStream = new MemoryStream())
                {
                    response.Body = responseBodyStream;

                    originalResponseBody.Seek(0, SeekOrigin.Begin);

                    await originalResponseBody.CopyToAsync(responseBodyStream);

                    response.Body = originalResponseBody;

                    responseBodyStream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(responseBodyStream, Encoding.UTF8))
                    {
                        string responseBody = await reader.ReadToEndAsync();
                        Console.WriteLine($"Cuerpo de la respuesta: {responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el cuerpo de la respuesta: {ex.Message}");
            }
        }
    }
}
