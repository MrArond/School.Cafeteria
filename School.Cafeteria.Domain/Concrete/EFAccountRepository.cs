using School.Cafeteria.Domain.Abstract;
using School.Cafeteria.Domain.Entities;

namespace School.Cafeteria.Domain.Concrete
{
    public class EFAccountRepository : IEFAccountRepository
    {
        private readonly EFDbContext _context;
        public EFAccountRepository(EFDbContext dbContext)
        {
            this._context = dbContext;
        }

        public Account CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        public Account GetAccountByEmail(string email)
        {
            return this._context
                .Accounts
                .FirstOrDefault(account => account.Email == email);
        }

        public Account GetAccountByUid(string uid)
        {
            return this._context
                .Accounts
                .FirstOrDefault(account => account.Uid == uid);
        }

        public Account UpdateAccount(Account existingAccount)
        {
            _context.Accounts.Update(existingAccount);
            _context.SaveChanges();

            return existingAccount;
        }
    }
}
