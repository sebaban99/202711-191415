using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Collections.Generic;
using IMMRequest.BusinessLogic;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private ISessionLogic session;
        private AdminLogic admin;
        private ILogLogic log;

        public LoginController(ISessionLogic session, AdminLogic admin, ILogLogic log) : base()
        {
            this.session = session;
            this.admin = admin;
            this.log = log;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model) 
        {    
            
            if (!session.ValidateEmailAndPassword(model.Email, model.Password)) 
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
                
                var ret = new LogModel(l);
                this.log.Add(l);
                
                return Ok(admin.GetByCondition(a => a.Email == model.Email));
            }
        }
    }
}
