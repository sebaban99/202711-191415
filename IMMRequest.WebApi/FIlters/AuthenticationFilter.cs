using System;
using IMMRequest.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IMMRequest.BusinessLogic;

namespace IMMRequest.WebApi
{
    public class AutenticationFilter : Attribute, IActionFilter
    {
        private const string NULL_TOKEN_FORMAT = "00000000-0000-0000-0000-000000000000"; 
        public AutenticationFilter()
        {

        }
        public void OnActionExecuting(ActionExecutingContext controlToken)
        {
            // OBTENEMOS EL TOKEN DEL HEADER
            string token = controlToken.HttpContext.Request.Headers["token"];
            // SI EL TOKEN ES NULL CORTAMOS LA PIPELINE Y RETORNAMOS UN RESULTADO
            if (string.IsNullOrEmpty(token) || token == NULL_TOKEN_FORMAT)
            {
                controlToken.Result = new ContentResult()
                {
                    Content = "A valid token is needed",
                };
            }           
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
