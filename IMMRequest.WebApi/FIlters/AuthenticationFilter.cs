using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IMMRequest.BusinessLogic.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private ISessionLogic GetSessionLogic(AuthorizationFilterContext context)
        {
            var typeOfSessionsLogic = typeof(ISessionLogic);
            return context.HttpContext.RequestServices.GetService(typeOfSessionsLogic) as ISessionLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["token"];
            if (token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Invalid session token"
                };
                return;
            }

            var sessionLogic = GetSessionLogic(context);
            
            if (!sessionLogic.ValidateSession(new Guid(token)))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "AFValueItemDTO valid session is needed"
                };
                return;
            }
        }
    }
}