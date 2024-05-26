using System.Text.Json.Serialization;

namespace School.Cafeteria.Models.Firebase
{
    public class FirebaseUser
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("users")]
        public List<User> Users { get; set; }
    }

    public class User
    {
        [JsonPropertyName("localId")]
        public string LocalId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("passwordUpdatedAt")]
        public long PasswordUpdatedAt { get; set; }

        [JsonPropertyName("providerUserInfo")]
        public List<ProviderUserInfo> ProviderUserInfo { get; set; }

        [JsonPropertyName("validSince")]
        public string ValidSince { get; set; }

        [JsonPropertyName("lastLoginAt")]
        public string LastLoginAt { get; set; }

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("lastRefreshAt")]
        public DateTime LastRefreshAt { get; set; }
    }
}
