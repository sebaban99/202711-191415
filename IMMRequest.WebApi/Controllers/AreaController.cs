using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMMRequest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private IAreaLogic areaLogic;

        public AreaController(IAreaLogic areaLogic)
        {
            this.areaLogic = areaLogic;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Area> areasInBD = areaLogic.GetAll();
                List<AreaDTO> areasToReturn = new List<AreaDTO>();
                foreach (Area area in areasInBD)
                {
                    AreaDTO areaDTO = new AreaDTO(area);
                    areasToReturn.Add(areaDTO);
                }
                return Ok(areasToReturn);
            }
            catch (BusinessLogicException e)
            {
                return BadRequest(e.Message);
            }
            catch (DataAccessException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}