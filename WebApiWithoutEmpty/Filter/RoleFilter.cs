using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Filter
{
    public class RoleFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string _allowedroles;

        public RoleFilter(string allowedroles)
        {
            _allowedroles = allowedroles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceprovider = context.HttpContext.RequestServices;
            using var scope = serviceprovider.CreateScope();
            var _shopcontext = scope.ServiceProvider.GetRequiredService<ShopContext>();


            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var precision = configuration.GetValue<string>("AppSettings:Token");

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizenheader))
            {
                var token = authorizenheader.ToString().Replace("Bearer ", "");

                var tokenhandler = new JwtSecurityTokenHandler();
                var validateparameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(precision)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                try
                {
                    var principal = tokenhandler.ValidateToken(token, validateparameters, out var validatedToken);
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));

                    if (userIdClaim != null)
                    {
                        var userid = userIdClaim.Value;

                        var rolesfromdb = _shopcontext.UserRoles.Include(x => x.role).Where(x => x.UserId == int.Parse(userid)).Select(x => x.role.Name.ToLower()).ToArray();

                        var normalizedallowedroles = _allowedroles.ToLower();
                        var filteredRolesArray = normalizedallowedroles.Split(",");

                        var matchCount = rolesfromdb.Intersect(filteredRolesArray).Count();

                        if (matchCount == 0)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
