using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using Wealthperk.Model;
using Wealthperk.Web.ViewModels;
using Wealthperl.Model;

namespace aspnetCoreReactTemplate.aspnetCoreReactTemplate.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
       // private readonly IEmailSender _emailSender;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly ILogger _logger;

        public AuthController(
            UserManager<UserInfo> userManager,
            IOptions<IdentityOptions> identityOptions,
           // IEmailSender emailSender,
            SignInManager<UserInfo> signInManager,
            ILoggerFactory loggerFactory,
            IUserAccountsRepository accountsRepo)
        {
            _userManager = userManager;
            _identityOptions = identityOptions;
         //   _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [AllowAnonymous]
        //[HttpPost("~/api/auth/login")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Login(OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                // Ensure the username and password is valid.
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return BadRequest(new
                    {
                        ErrorId = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username or password is invalid."
                    });
                }

                // Ensure the email is confirmed.
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest(new
                    {
                        ErrorId = "email_not_confirmed",
                        ErrorDescription = "You must have a confirmed email to log in."
                    });
                }

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user);

                _logger.LogInformation($"User logged in (id: {user.Id})");

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The refresh token is no longer valid."
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user, info.Properties);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        [AllowAnonymous]
        //[HttpPost("~/api/auth/register")]
        [HttpPost]
        public async Task<IActionResult> Register(NewUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new UserInfo {
                UserName = model.username,
                Email = model.username
            };
            var result = await _userManager.CreateAsync(user, model.password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"New user registered (id: {user.Id})");

                if (!user.EmailConfirmed)
                {
                    // Send email confirmation email
                    var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailConfirmUrl = Url.RouteUrl("ConfirmEmail", new { uid = user.Id, token = confirmToken }, this.Request.Scheme);
                  /*  await _emailSender.SendEmailAsync(model.username, "Please confirm your account",
    $"Please confirm your account by clicking this <a href=\"{emailConfirmUrl}\">link</a>."
                    );*/

                    _logger.LogInformation($"Sent email confirmation email (id: {user.Id})");
                }

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(new OpenIdConnectRequest(), user);

                _logger.LogInformation($"User logged in (id: {user.Id})");

                //return Ok();
                return Redirect("/api/Home/Index?success");
            }
            else
            {
                //return BadRequest(new { general = result.Errors.Select(x => x.Description) });
                return Redirect("/api/Home/Index?error=" + System.Net.WebUtility.UrlEncode(string.Join("|", result.Errors.Select(x => x.Description))));
            }
        }

        [AllowAnonymous]
        [HttpGet("~/api/auth/confirm", Name = "ConfirmEmail")]
        public async Task<IActionResult> Confirm(string uid, string token)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (confirmResult.Succeeded)
            {
                return Redirect("/?confirmed=1");
            }
            else
            {
                return Redirect("/error/email-confirm");
            }
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(
            OpenIdConnectRequest request, UserInfo user,
            AuthenticationProperties properties = null)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal,
            properties ?? new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);

            if (!request.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
                });//.Intersect(request.GetScopes()));
            }

            ticket.SetResources("resource-server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                var destinations = new List<string>
                {
                    OpenIdConnectConstants.Destinations.AccessToken
                };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            return ticket;
        }
    }
}
