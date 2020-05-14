using Microsoft.AspNetCore.Mvc;
using System;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private ISessionLogic sessionLogic;
        private IAdminLogic adminLogic;
        private ILogLogic logLogic;

        public LoginController(ISessionLogic session, IAdminLogic admin, ILogLogic log) : base()
        {
            this.sessionLogic = session;
            this.adminLogic = admin;
            this.logLogic = log;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO model) 
        {
            Admin loggedAdmin = sessionLogic.ValidateLogin(model.Email, model.Password);
            if (loggedAdmin == null) 
            {  
                return BadRequest("Login error: Incorrect email or password");
            }
            else
            {
                Session.LoggedAdmin = loggedAdmin;
                Log newLog = new Log();
                newLog.Admin = loggedAdmin;
                newLog.Email = Session.LoggedAdmin.Email;
                newLog.Date = DateTime.Now;
                newLog.ActionType = "login";
                
                this.logLogic.Add(newLog);
                
                return Ok(adminLogic.GetByCondition(a => a.Email == model.Email));
            }
        }
    }
}
