using School.Cafeteria.Domain.Entities;

namespace School.Cafeteria.Domain.Abstract
{
    public interface IEFAccountRepository
    {
        Account GetAccountByUid(string uid);
        Account GetAccountByEmail(string email);
        Account CreateAccount(Account account);
        Account UpdateAccount(Account existingAccount);
    }
}
