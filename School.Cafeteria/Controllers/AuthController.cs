using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using School.Cafeteria.Common.Extensions;
using School.Cafeteria.Models.Firebase;
using School.Cafeteria.Models.Models.Firebase;
using School.Cafeteria.Services.Interfaces;

using Swashbuckle.AspNetCore.Annotations;
namespace School.Cafeteria.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        public AuthController(IFirebaseService firebaseService)
        {
            this._firebaseService = firebaseService;
        }

        /// <summary>
        /// Creates new Firebase user.
        /// </summary>
        /// <param name="firebaseCredentials">Firebase credentials</param>
        /// <returns>Firebase Token</returns>
        [HttpPost]
        [Route("user/create")]
        [SwaggerOperation(
            Summary = "Creates Firebase user",
            Description = "Creates Firebase user",
            Tags = new[] { "Firebase" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(FirebaseToken))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed, Type = typeof(string))]
        public async Task<IActionResult> CreateFirebaseUser([FromBody] FirebaseCredentials firebaseCredentials)
        {
            var response = await this._firebaseService.SignUp(firebaseCredentials.Email, firebaseCredentials.Password);

            return this.Ok(response);
        }

        /// <summary>
        /// Authorize Firebase user.
        /// </summary>
        /// <param name="firebaseCredentials">Firebase credentials</param>
        /// <remarks>
        /// Authorize Firebase user.
        /// </remarks>
        /// <returns>FirebaseToken</returns>
        [HttpPost]
        [Route("user/login")]
        [SwaggerOperation(
            Summary = "Authorizes firebase user",
            Description = "Authorizes firebase user",
            Tags = new[] { "Firebase" })]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(FirebaseToken))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<IActionResult> AuthorizeFirebaseUser([FromBody] FirebaseCredentials firebaseCredentials)
        {
            var response = await _firebaseService.SignIn(firebaseCredentials.Email, firebaseCredentials.Password);

            return this.Ok(response);
        }

        /// <summary>
        /// Refreshes Firebase Token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Firebase Token</returns>
        [HttpPost]
        [Route("token/refresh")]
        [SwaggerOperation(
            Summary = "Refreshes Firebase Token",
            Description = "Refreshes Firebase Token",
            Tags = new[] { "Firebase" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(RefreshedFirebaseToken))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<IActionResult> RefreshFirebaseToken([FromBody] RefreshFirebaseToken refreshToken)
        {
            var response = await this._firebaseService.RefreshFirebaseToken(refreshToken.RefreshToken);

            return this.Ok(response);
        }

        /// <summary>
        /// Updates Firebase user password.
        /// </summary>
        /// <remarks>
        /// Required Bearer Authorization Token: Firebase
        /// </remarks>
        /// <param name="password">Password</param>
        /// <returns>Firebase Token</returns>
        [HttpPost]
        [Route("user/password")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Updates Firebase user password",
            Description = "Updates Firebase user password",
            Tags = new[] { "Firebase" })]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(FirebaseToken))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task<FirebaseToken> UpdateFirebaseUserPassword([FromBody] FirebaseChangePassword password)
        {
            string firebaseToken = Request.GetFirebaseToken();
            var response = await _firebaseService.ChangePassword(firebaseToken, password.Password);

            return new FirebaseToken
            {
                IdToken = response.IdToken,
                RefreshToken = response.RefreshToken,
                ExpiresIn = response.ExpiresIn,
            };
        }

        /// <summary>
        /// Removes Firebase user.
        /// </summary>
        /// <remarks>
        /// Required Bearer Authorization Token: Firebase
        /// </remarks>
        [HttpDelete]
        [Route("user/remove")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Removes Firebase user",
            Description = "Removes Firebase user",
            Tags = new[] { "Firebase" })]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(FirebaseError))]
        public async Task RemoveFirebaseUser()
        {
            await _firebaseService.RemoveAccount(Request.GetFirebaseToken());
        }
    }
}
