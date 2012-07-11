using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace RepositoryController
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
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
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!Enabled)
                return;

            base.OnAuthorization(actionContext);
        }
        //===============================================================
    }
}
