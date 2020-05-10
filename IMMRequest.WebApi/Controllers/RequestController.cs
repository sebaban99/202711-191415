using Microsoft.AspNetCore.Mvc;
using IMMRequest.BusinessLogic;
using IMMRequest.DataAccess;
using System;
using System.Collections.Generic;
using IMMRequest.Domain;
using Microsoft.AspNetCore.Http;

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
            catch (BusinessLogicException e)
            {
                return NotFound(e.Message);
            }
            catch (DataAccessException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpGet("{requestNumber}")]
        public IActionResult Get(int requestNumber)
        {
            try
            {
                Request request = requestLogic.GetByCondition(r => r.RequestNumber == requestNumber);
                RequestDTO reqToReturn = new RequestDTO(request);
                return Ok(reqToReturn);
            }
            catch (BusinessLogicException e)
            {
                return NotFound(e.Message);
            }
            catch (DataAccessException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestDTO requestDTO)
        {
            try
            {
                Request requestToCreate = requestDTO.ToEntity();
                int requestNumber = requestLogic.Create(requestToCreate);
                return Ok(requestNumber);
            }
            catch(BusinessLogicException e)
            {
                return BadRequest(e.Message);
            }
            catch(DataAccessException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}