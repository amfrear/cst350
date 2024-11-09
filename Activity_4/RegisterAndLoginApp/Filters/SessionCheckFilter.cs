using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RegisterAndLoginApp.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userString = context.HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(userString))
            {
                // Redirect to login if the user is not found in session
                context.Result = new RedirectToActionResult("Index", "User", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
