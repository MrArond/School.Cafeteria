using School.Cafeteria.Business.HttpClients;
using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Services.Interfaces;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace School.Cafeteria.Services.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private const string RefreshTokenPath = @"https://securetoken.googleapis.com/v1/token";

        public FirebaseService(FirebaseClient firebaseClient)
        {
            this._firebaseClient = firebaseClient;

            this._jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public async Task<SimpleFirebaseUser> ChangePassword(string firebaseToken, string newPassword)
        {
            var payload = new
            {
                idToken = firebaseToken,
                password = newPassword,
                returnSecureToken = true,
            };
            var request = this.CreateRequestMessage(HttpMethod.Post, "update", payload);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);

            return JsonSerializer.Deserialize<SimpleFirebaseUser>(await response.Content.ReadAsStringAsync(), this._jsonSerializerOptions);
        }

        public async Task RemoveAccount(string firebaseToken)
        {
            var payload = new
            {
                idToken = firebaseToken,
            };
            var request = this.CreateRequestMessage(HttpMethod.Post, "delete", payload);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);
        }

        public async Task<RefreshedFirebaseToken> RefreshFirebaseToken(string refreshToken)
        {
            var payload = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken,
            };

            var request = this.CreateRequestMessage(HttpMethod.Post, string.Empty, payload);
            request.RequestUri = new Uri(RefreshTokenPath);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);

            return JsonSerializer.Deserialize<RefreshedFirebaseToken>(await response.Content.ReadAsStringAsync(), this._jsonSerializerOptions);
        }

        public async Task<FirebaseToken> SignIn(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true,
            };
            var request = this.CreateRequestMessage(HttpMethod.Post, "signInWithPassword", payload);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);

            return JsonSerializer.Deserialize<FirebaseToken>(await response.Content.ReadAsStringAsync(), this._jsonSerializerOptions);
        }

        public async Task<FirebaseToken> SignUp(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true,
            };
            var request = this.CreateRequestMessage(HttpMethod.Post, "signUp", payload);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);

            return JsonSerializer.Deserialize<FirebaseToken>(await response.Content.ReadAsStringAsync(), this._jsonSerializerOptions);
        }

        public async Task<FirebaseUser> GetFirebaseAccount(string firebaseToken)
        {
            var payload = new
            {
                idToken = firebaseToken,
            };
            var request = this.CreateRequestMessage(HttpMethod.Post, "lookup", payload);

            var response = await this._firebaseClient._httpClient.SendAsync(request);

            await this._firebaseClient.ThrowIfNotSuccessful(response);

            return JsonSerializer.Deserialize<FirebaseUser>(await response.Content.ReadAsStringAsync(), this._jsonSerializerOptions);
        }

        private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, object payload = null)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = method;
            httpRequest.RequestUri = new Uri($"{this._firebaseClient._httpClient.BaseAddress}{url}"); ;

            if (payload != null)
            {
                httpRequest.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            }

            return httpRequest;
        }
    }
}
