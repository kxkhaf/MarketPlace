using System.Net;
using System.Text;

namespace MarketPlace
{
    public static class StaticFile
    {
        const string _errorMessage = "error";
        public static async Task ShowStatic(this HttpListenerContext context)
        {
            var response = context.Response;
            var requestPath = context.Request.RawUrl;
            var staticPath = Path.Combine(Directory.GetCurrentDirectory(), $"WWW{requestPath}");
            if (File.Exists(staticPath))
            {
                await ShowFile(response, staticPath);
            }
            else
            {
                await ShowError(response);
            }
        }
        public static async Task ShowFile(this HttpListenerResponse response, string path)
        {
            response.ContentType = Path.GetExtension(path) switch
            {
                ".js" => "application/javascript",
                ".html" => "text/html",
                ".css" => "text/css",
                ".png" => "image/png",
                ".cur" => "image/cur",
                ".ani" => "image/ani",
                ".gif" => "image/gif",
                _ => throw new NotImplementedException()
            };
            response.StatusCode = 200;
            using var stream = response.OutputStream;
            await stream.WriteAsync(File.ReadAllBytes(path));
        }

        public static async Task ShowError(HttpListenerResponse response)
        {
            response.ContentType = "text/plain";
            response.StatusCode = 404;
            using var stream = response.OutputStream;
            await stream.WriteAsync(Encoding.UTF8.GetBytes(_errorMessage));
        }
    }
}

