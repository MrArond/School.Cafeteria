using Microsoft.Extensions.Options;
using School.Cafeteria.Models.Firebase;

namespace School.Cafeteria.Business.Handlers
{
    public class FirebaseKeyHandler : DelegatingHandler
    {
        private readonly FirebaseConfig _firebaseConfig;
        public FirebaseKeyHandler(IOptions<FirebaseConfig> firebaseConfig)
        {
            this._firebaseConfig = firebaseConfig.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            uriBuilder.Query = $"key={_firebaseConfig.ApiKey}";

            request.RequestUri = uriBuilder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
