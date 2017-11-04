using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToggleAPI.Models;

namespace ToggleAPI.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Provides an API to create auth tokens
    /// </summary>
    public class AuthController : Controller
    {
        private readonly UserManager<SystemUser> _userMgr;
        private readonly IPasswordHasher<SystemUser> _hasher;
        private readonly IConfiguration _config;

        public AuthController( UserManager<SystemUser> userMgr, IPasswordHasher<SystemUser> hasher, IConfiguration config){
            _userMgr = userMgr;
            _hasher = hasher;
            _config = config;
        }

        /// <summary>
        /// Creates a JWT token for for the user in <paramref name="credentials"/>
        /// </summary>
        /// <param name="credentials">The user authentication credentials</param>
        /// <returns>Ok response with token</returns>
        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel credentials)
        {
            var user = await _userMgr.FindByNameAsync(credentials.UserName);

            if (user == null) return BadRequest("Invalid User");
            if (!AreValidCredentials(user, credentials)) return BadRequest("Invalid User");

            var token = await CreateJwtSecurityTokenForUser(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        private async Task<JwtSecurityToken> CreateJwtSecurityTokenForUser(SystemUser user)
        {
            var claims = await GetClaimsAsync(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var encryptedCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Tokens:Issuer"],
                audience: _config["Tokens:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: encryptedCredentials
            );
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(SystemUser user)
        {
            var userClaims = _userMgr.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(await userClaims);
            return claims;
        }

        private bool AreValidCredentials(SystemUser user, CredentialModel credentials)
        {
            return _hasher.VerifyHashedPassword(user, user.PasswordHash, credentials.Password) == PasswordVerificationResult.Success;
        }
    }
}