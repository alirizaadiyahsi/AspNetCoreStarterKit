using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AspNetCoreStarterKit.Application.Authentication;
using AspNetCoreStarterKit.Application.Authentication.Dto;
using AspNetCoreStarterKit.Application.Email;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Configuration;
using AspNetCoreStarterKit.WebApi.Core.Configuration;
using AspNetCoreStarterKit.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AspNetCoreStarterKit.Module.Core.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAuthenticationAppService _authenticationAppService;
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AccountController(
            IAuthenticationAppService authenticationAppService,
            IOptions<JwtTokenConfiguration> jwtTokenConfiguration,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _authenticationAppService = authenticationAppService;
            _configuration = configuration;
            _emailSender = emailSender;
            _jwtTokenConfiguration = jwtTokenConfiguration.Value;
        }

        [HttpPost("/api/[action]")]
        public async Task<ActionResult<LoginOutput>> Login([FromBody]LoginInput input)
        {
            var userToVerify = await CreateClaimsIdentityAsync(input.UserNameOrEmail, input.Password);
            if (userToVerify == null)
            {
                return NotFound("User name or password is not correct!"); // TODO: Make these messages static object
            }

            var token = new JwtSecurityToken
            (
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                claims: userToVerify.Claims,
                expires: _jwtTokenConfiguration.EndDate,
                notBefore: _jwtTokenConfiguration.StartDate,
                signingCredentials: _jwtTokenConfiguration.SigningCredentials
            );

            return Ok(new LoginOutput
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost("/api/[action]")]
        public async Task<ActionResult> Register([FromBody]RegisterInput input)
        {
            var user = await _authenticationAppService.FindUserByEmailAsync(input.Email);
            if (user != null) return Conflict("Email already exist!"); // TODO: Make these messages static object

            user = await _authenticationAppService.FindUserByUserNameAsync(input.Email);
            if (user != null) return Conflict("User name already exist!"); // TODO: Make these messages static object

            var applicationUser = new User
            {
                UserName = input.UserName,
                Email = input.Email
            };

            var result = await _authenticationAppService.CreateUserAsync(applicationUser, input.Password);

            if (!result.Succeeded)
            {
                return BadRequest(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }

            var confirmationToken = await _authenticationAppService.GenerateEmailConfirmationTokenAsync(applicationUser);
            var callbackUrl = $"{_configuration[AppConfig.App_ClientUrl]}/account/confirm-email?email={applicationUser.Email}&token={HttpUtility.UrlEncode(confirmationToken)}";
            var subject = "Confirm your email.";
            var message = $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>{callbackUrl}</a>";

            //await _emailSender.SendEmailAsync(applicationUser.Email, subject, message);

            return Ok(new RegisterOutput { ResetToken = confirmationToken });
        }

        [HttpPost("/api/[action]")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailInput input)
        {
            var user = await _authenticationAppService.FindUserByEmailAsync(input.Email);
            if (user == null) return NotFound("Email is not found!"); // TODO: Make these messages static object

            var result = await _authenticationAppService.ConfirmEmailAsync(user, input.Token);
            if (!result.Succeeded) return BadRequest(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

            return Ok();
        }


        [HttpPost("/api/[action]")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordInput input)
        {
            if (input.NewPassword != input.PasswordRepeat)
            {
                return BadRequest("Passwords are not matched!"); // TODO: Make these messages static object
            }

            var user = await _authenticationAppService.FindUserByUserNameAsync(User.Identity.Name);
            var result = await _authenticationAppService.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            if (!result.Succeeded) return BadRequest(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

            return Ok();
        }

        // TODO: Get user by only e-mail not user name
        [HttpPost("/api/[action]")]
        public async Task<ActionResult<ForgotPasswordOutput>> ForgotPassword([FromBody] ForgotPasswordInput input)
        {
            var user = await _authenticationAppService.FindUserByUserNameOrEmailAsync(input.UserNameOrEmail);
            if (user == null) return NotFound("User is not found!"); // TODO: Make these messages static object

            var resetToken = await _authenticationAppService.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"{_configuration[AppConfig.App_ClientUrl]}/account/reset-password?token={HttpUtility.UrlEncode(resetToken)}";
            var subject = "Reset your password.";
            var message = $"Please reset your password by clicking this link: <a href='{callbackUrl}'>{callbackUrl}</a>";

            //await _emailSender.SendEmailAsync(user.Email, subject, message);

            return Ok(new ForgotPasswordOutput { ResetToken = resetToken });
        }

        [HttpPost("/api/[action]")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordInput input)
        {
            var user = await _authenticationAppService.FindUserByUserNameOrEmailAsync(input.UserNameOrEmail);
            if (user == null) return NotFound("User is not found!"); // TODO: Make these messages static object

            var result = await _authenticationAppService.ResetPasswordAsync(user, input.Token, input.Password);
            if (!result.Succeeded) return BadRequest(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

            return Ok();
        }

        private async Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userNameOrEmail, string password)
        {
            if (string.IsNullOrEmpty(userNameOrEmail) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userToVerify = await _authenticationAppService.FindUserByUserNameOrEmailAsync(userNameOrEmail);
            if (userToVerify == null)
            {
                return null;
            }

            if (!await _authenticationAppService.CheckPasswordAsync(userToVerify, password)) return null;

            var claims = new List<Claim>(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userToVerify.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", userToVerify.Id.ToString()),
                new Claim("profileImageUrl", userToVerify.ProfileImageUrl ?? "")
            });

            // add roles to roleClaim to use build-in User.IsInRole method
            var roleNames = userToVerify.UserRoles.Select(ur => ur.Role.Name);
            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            return new ClaimsIdentity(new ClaimsIdentity(new GenericIdentity(userNameOrEmail, "Token"), claims));
        }
    }
}
