using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Models.Models.Account;

namespace School.Cafeteria.Services.Interfaces
{
    public interface IAccountService
    {
        AccountDto EnrollNewAccount(FirebaseUser firebaseUser, AccountDto accountDto);
        AccountDto UpdateAccount(FirebaseUser firebaseUser, AccountDto accountDto);
        AccountDto GetAccount(FirebaseUser firebaseUser);
    }
}
