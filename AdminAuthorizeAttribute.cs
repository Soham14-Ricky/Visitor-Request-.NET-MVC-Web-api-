using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VisitorMVC.Filters
{
    public class AdminAuthorizeAttribute
        : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var role =
                context.HttpContext.Session
                .GetString("UserRole");

            if (role != "Admin")
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