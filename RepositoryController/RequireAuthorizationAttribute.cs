using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RepositoryController
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireAuthorizationAttribute : AuthorizeAttribute 
    {
        //===============================================================
        public RequireAuthorizationAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
        //===============================================================
        public bool Enabled { get; set; }
        //===============================================================
        protected bool AllowAnonymousDetected(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)
                return true;
            if (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)
                return true;

            return false;
        }
        //===============================================================
        protected bool BypassAuthorization(HttpActionContext actionContext)
        {
            return !Enabled || AllowAnonymousDetected(actionContext);
        }
        //===============================================================
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (BypassAuthorization(actionContext))
                return;

            base.OnAuthorization(actionContext);
        }
        //===============================================================
    }
}
