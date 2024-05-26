using School.Cafeteria.Common.CustomException;
using School.Cafeteria.Models.Firebase;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace School.Cafeteria.Business.HttpClients
{
    public class FirebaseClient
    {
        public readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FirebaseClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;

            this._jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public async Task ThrowIfNotSuccessful(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var firebaseError = JsonSerializer.Deserialize<FirebaseError>(error, this._jsonSerializerOptions);

                throw new SchoolCafeteriaException(firebaseError);
            }
        }
    }
}
