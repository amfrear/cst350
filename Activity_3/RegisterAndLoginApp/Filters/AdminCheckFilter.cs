using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RegisterAndLoginApp.Models;
using ServiceStack.Text;

namespace RegisterAndLoginApp.Filters
{
    public class AdminCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if user info is available in session
            string userInfo = context.HttpContext.Session.GetString("User");
            if (userInfo == null)
            {
                // Redirect to login page if no user info
                context.Result = new RedirectResult("/User/Index");
                return;
            }

            // Check if the user is part of the Admin group
            try
            {
                UserModel user = JsonSerializer.DeserializeFromString<UserModel>(userInfo);
                if (!user.Groups.Contains("Admin"))
                {
                    // Redirect to login if user is not an admin
                    context.Result = new RedirectResult("/User/Index");
                    return;
                }
            }
            catch
            {
                // If deserialization fails, redirect to login
                context.Result = new RedirectResult("/User/Index");
                return;
            }
        }
    }
}
