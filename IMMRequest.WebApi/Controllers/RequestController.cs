using Microsoft.AspNetCore.Mvc;
using IMMRequest.BusinessLogic;
using IMMRequest.DataAccess;
using System;
using System.Collections.Generic;
using IMMRequest.Domain;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private IRequestLogic requestLogic;

        public RequestController(IRequestLogic requestLogic)
        {
            this.requestLogic = requestLogic;
        }

        [AuthenticationFilter()]
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Request> requestsInBD = requestLogic.GetAll();
            List<RequestDTO> requestToReturn = new List<RequestDTO>();
            foreach (Request req in requestsInBD)
            {
                RequestDTO reqDTO = new RequestDTO(req);
                requestToReturn.Add(reqDTO);
            }
            return Ok(requestToReturn);
        }

        [AuthenticationFilter()]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                Request request = requestLogic.Get(id);
                RequestDTO reqToReturn = new RequestDTO(request);
                return Ok(reqToReturn);
            }
            catch (Exception e)
            when (e is BusinessLogicException || e is DataAccessException)
            {
                return NotFound(e.Message);
            }
            
        }
    }
}