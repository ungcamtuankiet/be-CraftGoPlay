using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CGP.WebAPI.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public RequireRoleAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Error = 1,
                    Message = "You do not have access authority!",
                });
                return;
            }

            var roleClaim = user.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != _role)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
