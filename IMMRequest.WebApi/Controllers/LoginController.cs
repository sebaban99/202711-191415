using Microsoft.AspNetCore.Mvc;
using System;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private ISessionLogic session;
        private IAdminLogic admin;
        private ILogLogic log;

        public LoginController(ISessionLogic session, IAdminLogic admin, ILogLogic log) : base()
        {
            this.session = session;
            this.admin = admin;
            this.log = log;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO model) 
        {    
            
            if (!session.ValidateLogin(model.Email, model.Password)) 
            {  
                return BadRequest("Login error: Incorrect email or password");
            }
            else
            {
                Log l= new Log();
                l.Admin = Session.LoggedAdmin;
                l.Email = Session.LoggedAdmin.Email;
                l.Date = DateTime.Now;
                l.ActionType = "login";
                
                var ret = new LogDTO(l);
                this.log.Add(l);
                
                return Ok(admin.GetByCondition(a => a.Email == model.Email));
            }
        }
    }
}
