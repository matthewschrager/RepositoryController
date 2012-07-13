using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;

namespace RepositoryController
{
    public abstract class RequireApiKeyAttribute : RequireAuthorizationAttribute 
    {
        //===============================================================
        protected RequireApiKeyAttribute(String headerName = "ApiKey")
        {
            HeaderName = headerName;
        }
        //===============================================================
        public RequireApiKeyAttribute(bool enabled, String headerName = "ApiKey")
            : base(enabled)
        {
            HeaderName = headerName;
        }
        //===============================================================
        public String HeaderName { get; private set; }
        //===============================================================
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (BypassAuthorization(actionContext))
                return;

            // Find the api key header
            var header = actionContext.Request.Headers.GetValues(HeaderName).FirstOrDefault();
            if (header == null || !ValidateApiKey(header))
                HandleUnauthorizedRequest(actionContext);
        }
        //===============================================================
        protected abstract bool ValidateApiKey(String apiKey);
        //===============================================================
    }
}
