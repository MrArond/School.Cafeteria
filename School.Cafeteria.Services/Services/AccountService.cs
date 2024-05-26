using School.Cafeteria.Domain.Abstract;
using School.Cafeteria.Domain.Entities;
using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Models.Models.Account;
using School.Cafeteria.Services.Interfaces;

namespace School.Cafeteria.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IEFAccountRepository _accountRepository;

        public AccountService(IEFAccountRepository iEFAccountRepository)
        {
            this._accountRepository = iEFAccountRepository;
        }

        public AccountDto EnrollNewAccount(FirebaseUser firebaseUser, AccountDto accountDto)
        {
            var user = firebaseUser.Users.FirstOrDefault();

            var existingAccount = this._accountRepository.GetAccountByUid(user.LocalId);
            if (existingAccount != null)
            {
                throw new Exception("User exists.");
            }

            var newAccount = new Account
            {
                Uid = user.LocalId,
                Email = string.IsNullOrEmpty(accountDto.Email) ? user.Email : accountDto.Email,
                FirstName = accountDto.FirstName,
                LastName = accountDto.LastName,
                PhoneNumber = accountDto.PhoneNumber,
            };

            var createdAccount = this._accountRepository.CreateAccount(newAccount);

            return new AccountDto
            {
                AccountId = createdAccount.AccountId.ToString(),
                Email = createdAccount.Email,
                FirstName = createdAccount.FirstName,
                LastName = createdAccount.LastName,
                PhoneNumber = createdAccount.PhoneNumber,
            };
        }

        public AccountDto GetAccount(FirebaseUser firebaseUser)
        {
            var user = firebaseUser.Users.FirstOrDefault();

            var existingAccount = this._accountRepository.GetAccountByUid(user.LocalId);
            if (existingAccount == null)
            {
                throw new Exception("User doesn't exists.");
            }

            return new AccountDto
            {
                AccountId = existingAccount.AccountId.ToString(),
                Email = existingAccount.Email,
                FirstName = existingAccount.FirstName,
                LastName = existingAccount.LastName,
                PhoneNumber = existingAccount.PhoneNumber,
            };
        }

        public AccountDto UpdateAccount(FirebaseUser firebaseUser, AccountDto accountDto)
        {
            var user = firebaseUser.Users.FirstOrDefault();

            var existingAccount = this._accountRepository.GetAccountByUid(user.LocalId);
            if (existingAccount == null)
            {
                throw new Exception("User doesn't exists.");
            }

            var account = this._accountRepository.GetAccountByUid(user.LocalId);

            account.FirstName = string.IsNullOrEmpty(accountDto.FirstName) ? account.FirstName : accountDto.FirstName;
            account.LastName = string.IsNullOrEmpty(accountDto.LastName) ? account.LastName : accountDto.LastName;
            account.Email = string.IsNullOrEmpty(accountDto.Email) ? account.Email : accountDto.Email;
            account.PhoneNumber = string.IsNullOrEmpty(accountDto.PhoneNumber) ? account.PhoneNumber : accountDto.PhoneNumber;


            var updatedAccount = this._accountRepository.UpdateAccount(account);

            return new AccountDto
            {
                AccountId = updatedAccount.AccountId.ToString(),
                Email = updatedAccount.Email,
                FirstName = updatedAccount.FirstName,
                LastName = updatedAccount.LastName,
                PhoneNumber = updatedAccount.PhoneNumber,
            };
        }
    }
}
