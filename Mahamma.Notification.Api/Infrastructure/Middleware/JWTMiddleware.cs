﻿using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.AppService.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Infrastructure.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTSetting _appSettings;

        public JWTMiddleware(RequestDelegate next, JWTSetting appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _appSettings.Issuer,
                    ValidateAudience = false,
                    ValidAudience = _appSettings.Audience,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                long userId = default;
                long.TryParse(jwtToken.Claims.First(x => x.Type == "UserId").Value, out userId);

                // attach user to context on successful jwt validation
                context.Items["UserId"] = userId.ToString();
            }
            catch(Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                return;
            }
        }
    }
}
