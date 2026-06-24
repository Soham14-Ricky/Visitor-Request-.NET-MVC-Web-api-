using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VisitorMVC.Filters
{
    public class SessionAuthorizeAttribute
        : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var token =
                context.HttpContext.Session
                .GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                context.Result =
                    new RedirectToActionResult(
                        "Login",
                        "Account",
                        null);
            }

            base.OnActionExecuting(context);
        }
    }
}