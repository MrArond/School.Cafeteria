using System.Net.Http.Headers;

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace School.Cafeteria.Common.Extensions
{
    public static class HttpRequestExtension
    {
        public static string GetFirebaseToken(this HttpRequest httpRequest)
        {
            var firebaseToken = httpRequest.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(firebaseToken, out var headerToken))
            {
                return headerToken.Parameter;
            }

            return string.Empty;
        }
    }
}
