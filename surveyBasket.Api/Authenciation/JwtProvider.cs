
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace surveyBasket.Api.Authenciation
{
    public class JwtProvider(IOptions<JwtOptions>jwtoptions) : IJwtProvider
    {
        public JwtOptions _JwtOptions = jwtoptions.Value;
        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {





            Claim[] claims = 
                [
                new (JwtRegisteredClaimNames.Sub , user.Id), // it carry the  id 
                new (JwtRegisteredClaimNames.Email , user.Email!),
                new (JwtRegisteredClaimNames.GivenName , user.FirstName),
                new (JwtRegisteredClaimNames.FamilyName , user.LastName),
                new (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) //random Guied


                ];

            var SymemtricSecuritykey = new SymmetricSecurityKey // is key For Encoding of Token
                (Encoding.UTF8.GetBytes(_JwtOptions.key));
            //have key and algorthims
            var singingCredentials = new SigningCredentials(SymemtricSecuritykey,SecurityAlgorithms.HmacSha256);

            


            var token = new JwtSecurityToken
                (
                issuer: _JwtOptions?.Issuer, // men ely 3mel this version of token                
                audience: _JwtOptions?.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_JwtOptions.ExpiryMinutes!),
                signingCredentials: singingCredentials


                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token),expiresIn :_JwtOptions.ExpiryMinutes * 60);


        }

        public string? ValidateToken(string token)
        {


            var tokenHandler  = new JwtSecurityTokenHandler();


            var SymemtricSecuritykey = new SymmetricSecurityKey 
               (Encoding.UTF8.GetBytes(_JwtOptions.key));
            try
            {

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = SymemtricSecuritykey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;//have all claims

               return  jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

            }
            catch
            {


                return null;

            }



        }
    }
}
