using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    public class JwtToken
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <returns></returns>
        public static dynamic BuildJwtToken()
        {
            //可以在Claim中自定义添加用户信息，或权限信息,由方法参数传入
            string iss = PermissionRequirement.Issuer;
            string aud = PermissionRequirement.Audience;
            double expiration = PermissionRequirement.Expiration.TotalSeconds;
            Claim[] claims = new Claim[] {
            new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
            new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
            new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(expiration)).ToUnixTimeSeconds()}"),
            new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(expiration).ToString()),
            new Claim(JwtRegisteredClaimNames.Iss,iss),
            new Claim(JwtRegisteredClaimNames.Aud,aud)};
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: iss,
                audience:aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(PermissionRequirement.Expiration),
                signingCredentials: PermissionRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new
            {
                success = true,
                token = "Bearer " + encodedJwt,
                expires_in = expiration,
                token_type = "Bearer"
            };
            return responseJson;
        }
    }
}
