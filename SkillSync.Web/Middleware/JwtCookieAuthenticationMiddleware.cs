using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SkillSync.Web.Middleware
{
    public class JwtCookieAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtCookieName = "JwtToken";

        public JwtCookieAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (context.Request.Cookies.TryGetValue(_jwtCookieName, out var token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSettings = configuration.GetSection("Jwt");
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); // your JWT signing key

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = key
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                    // Set the HttpContext.User so User.Identity.IsAuthenticated becomes true
                    context.User = principal;
                }
                catch
                {
                    // Invalid token, do nothing or optionally clear cookie
                }
            }

            await _next(context);
        }
    }
}
