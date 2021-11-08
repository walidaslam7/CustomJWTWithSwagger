using Blogifier.Shared.ViewModels.RequestModels;
using Blogifier.Shared.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Blogifier.Shared.ViewModels.AppModels;
using Blogifier.Providers.Context;
using System.Security.Cryptography;
using Blogifier.Shared.DomainEntities;
using Blogifier.Helpers.Security;
using Blogifier.Helpers.Constants;

namespace Blogifier.Services
{
    public interface IAccountService
    {
        Response<AccountResponses> SignIn(SignInRequest request);
        void SignOut(string token);
        AccountResponses RefreshToken(RefreshRequest request);
    }
    public class AccountService : IAccountService
    {
        public IConfiguration Configuration { get; }
        private readonly AppDbContext _dBContext;
        public AccountService(AppDbContext dBContext, IConfiguration configuration)
        {
            _dBContext = dBContext;
            Configuration = configuration;
        }

        #region External Methods
        public Response<AccountResponses> SignIn(SignInRequest request)
        {
            Response<AccountResponses> responseModel = new();
            var section = Configuration.GetSection("Blogifier");
            var account = _dBContext.Accounts.Where(acc => acc.Email == request.email && acc.IsDeleted == false).FirstOrDefault();
            var getDecryptedPassword = SecurityHelper.DecryptString(account.Password, section["SecurityKey"]);

            if (account != null && getDecryptedPassword == request.password)
            {
                var authClaims = new[]
                {
                     new Claim(JwtRegisteredClaimNames.Sub, account.Name),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(ClaimTypes.Role, "Admin"),
                     new Claim(JwtRegisteredClaimNames.GivenName, account.Name),
                     new Claim(JwtRegisteredClaimNames.Email, account.Email),
                };
                var generate = SaveGeneratedTokens(authClaims, account);
                responseModel.SetResponse(generate,System.Net.HttpStatusCode.OK, Constant.Messages.SIGN_IN_SUCCESS);
                return responseModel;
            }
            return responseModel;
        }
        public void SignOut(string email)
        {
            var account = _dBContext.Accounts.Where(acc => acc.Email == email).FirstOrDefault();
            if (account != null)
            {
                account.Token = null;
                account.RefreshToken = null;
                account.TokenExpirationTime = DateTime.Now.AddDays(-1);
                account.RefreshTokenExpirationTime = DateTime.Now.AddDays(-1);
                _dBContext.SaveChanges();
            }
        }
        public AccountResponses RefreshToken(RefreshRequest request)
        {
            var getprinicipalModel = GetPrincipalFromExpiredToken(request.token);
            var account = _dBContext.Accounts.SingleOrDefault(acc => acc.Email == getprinicipalModel.email);
            if (account == null || account.RefreshToken != request.refreshToken || account.RefreshTokenExpirationTime <= DateTime.Now)
            {
                return null;
            }
            return SaveGeneratedTokens(getprinicipalModel.claims, account);
        }
        #endregion

        #region Internal Methods
        private AccountResponses SaveGeneratedTokens(IEnumerable<Claim> claims, Accounts account)
        {
            AccountResponses generatedTokens = GenerateTokens(claims);
            SaveUserAccountTokens(generatedTokens, account);
            generatedTokens.userId = account.UserId;
            return generatedTokens;
        }
        private AccountResponses GenerateTokens(IEnumerable<Claim> authClaims)
        {
            var section = Configuration.GetSection("Blogifier");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["SymmetricSecurityKey"]));
            var token = new JwtSecurityToken(
                issuer: section["Issuer"],
                audience: section["Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(section["Expires"])),
                claims: authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            AccountResponses signInResponse = new AccountResponses()
            {
                token = tokenString,
                tokenExpirationTime = token.ValidTo,
                refreshToken = GenerateRefreshToken(),
                refreshTokenExpirationTime = DateTime.Now.AddDays(1)
            };

            return signInResponse;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private void SaveUserAccountTokens(AccountResponses generatedTokens, Accounts account)
        {
            account.Token = generatedTokens.token;
            account.TokenExpirationTime = generatedTokens.tokenExpirationTime;
            account.RefreshToken = generatedTokens.refreshToken;
            account.RefreshTokenExpirationTime = generatedTokens.refreshTokenExpirationTime;
            _dBContext.SaveChanges();
        }
        private Principal GetPrincipalFromExpiredToken(string token)
        {
            var section = Configuration.GetSection("Blogifier");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = section["Audience"],
                ValidateIssuer = true,
                ValidIssuer = section["Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["SymmetricSecurityKey"])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            var email = principal.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Select(x => x.Value).FirstOrDefault();
            Principal prinicipalModel = new Principal()
            {
                email = email,
                claims = principal.Claims.ToList()
            };
            return prinicipalModel;
        }
        #endregion
    }
}
