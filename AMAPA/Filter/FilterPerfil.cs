using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace AMAPA.Filter
{
    public class FilterPerfil
    {
        public class Filtro : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.HttpContext.Session.TryGetValue("user", out _))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                }

                base.OnActionExecuting(context);
            }
        }

    }
}
