using Microsoft.AspNetCore.Hosting;

namespace TaskManager.Api.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseContentRootSafe(this IWebHostBuilder hostBuilder, string contentRoot)
        {
            if (!string.IsNullOrEmpty(contentRoot))
            {
                return hostBuilder;
            }

            return hostBuilder.UseSetting(WebHostDefaults.ContentRootKey, contentRoot);
        }
    }
}
