using Google.Apis.Auth;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.User.Entity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.Helper
{
    public class JWTHelper : IJWTHelper
    {
        private readonly JWTSetting _jWTSetting;
        private readonly GoogleAuthSettings _goolgeSettings;
        public JWTHelper(JWTSetting jWTSetting, GoogleAuthSettings goolgeSettings)
        {
            _jWTSetting = jWTSetting;
            _goolgeSettings = goolgeSettings;
        }
        public string GenerateNewJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString())
            };
            var token = new JwtSecurityToken(_jWTSetting.Issuer, _jWTSetting.Audience, claims,
              expires: DateTime.Now.AddMinutes(_jWTSetting.ExpirationPeriodInMinutes),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string provider, string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.clientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (Exception exception)
            {
                //log an exception
                return null;
            }
        }
    }
}
