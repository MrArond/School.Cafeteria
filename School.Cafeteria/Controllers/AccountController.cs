using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using School.Cafeteria.Common.Extensions;
using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Models.Models.Account;
using School.Cafeteria.Services.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace School.Cafeteria.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IFirebaseService _firebaseService;

        public AccountController(IAccountService accountService,
            IFirebaseService firebaseService)
        {
            _accountService = accountService;
            _firebaseService = firebaseService;
        }

        /// <summary>
        /// Enrolles new user account.
        /// </summary>
        /// <param name="accountDto">Account DTO</param>
        /// <remarks>
        /// Required Bearer Authorization Token: Firebase
        /// </remarks>
        /// <returns>Account Data DTO</returns>
        [HttpPost]
        [Authorize]
        [Route("account/enroll")]
        [SwaggerOperation(
            Summary = "Enrolles new account",
            Description = "Enrolles new account",
            Tags = new[] { "Account" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AccountDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<IActionResult> EnrollNewAccount([FromBody] AccountDto accountDto)
        {
            var firebaseToken = Request.GetFirebaseToken();
            var existingFirebaseAccount = await this._firebaseService.GetFirebaseAccount(firebaseToken);

            var account = this._accountService.EnrollNewAccount(existingFirebaseAccount, accountDto);

            return this.Ok(account);
        }

        /// <summary>
        /// Updates user account.
        /// </summary>
        /// <param name="accountDto">Account DTO</param>
        /// <remarks>
        /// Required Bearer Authorization Token: Firebase
        /// </remarks>
        /// <returns>Account Data DTO</returns>
        [HttpPut]
        [Authorize]
        [Route("account/update")]
        [SwaggerOperation(
            Summary = "Updates account",
            Description = "Updates account",
            Tags = new[] { "Account" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AccountDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDto accountDto)
        {
            var firebaseToken = Request.GetFirebaseToken();
            var existingFirebaseAccount = await this._firebaseService.GetFirebaseAccount(firebaseToken);

            var account = this._accountService.UpdateAccount(existingFirebaseAccount, accountDto);

            return this.Ok(account);
        }

        /// <summary>
        /// Gets user account.
        /// </summary>
        /// <remarks>
        /// Required Bearer Authorization Token: Firebase
        /// </remarks>
        /// <returns>Account Data DTO</returns>
        [HttpGet]
        [Authorize]
        [Route("account")]
        [SwaggerOperation(
            Summary = "Gets account",
            Description = "Gets account",
            Tags = new[] { "Account" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AccountDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<IActionResult> GetAccount()
        {
            var firebaseToken = Request.GetFirebaseToken();
            var existingFirebaseAccount = await this._firebaseService.GetFirebaseAccount(firebaseToken);

            var account = this._accountService.GetAccount(existingFirebaseAccount);

            return this.Ok(account);
        }
    }
}
