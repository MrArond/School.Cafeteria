using School.Cafeteria.Models.Firebase;

namespace School.Cafeteria.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task<RefreshedFirebaseToken> RefreshFirebaseToken(string refreshToken);
        Task<FirebaseToken> SignIn(string email, string password);
        Task<FirebaseToken> SignUp(string email, string password);
        Task RemoveAccount(string firebaseToken);
        Task<SimpleFirebaseUser> ChangePassword(string firebaseToken, string newPassword);
        Task<FirebaseUser> GetFirebaseAccount(string firebaseToken);
    }
}
