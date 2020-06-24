using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IMMRequest.Domain;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminLogic adminLogic;

        public AdminController (IAdminLogic adminLogic)
        {
            this.adminLogic = adminLogic;
        }

        [AuthenticationFilter()]
        [HttpPost("ReportA")]
        public IActionResult GetReportA([FromBody] ReportAData reportData)
        {
            try
            {
                DateTime until = DateTime.Parse(reportData.Until);
                DateTime from = DateTime.Parse(reportData.From);
                IEnumerable<ReportTypeAElement> report = adminLogic.GenerateReportA(from, until, reportData.Email);
                return Ok(report);
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

        [AuthenticationFilter()]
        [HttpPost("ReportB")]
        public IActionResult GetReportB([FromBody] ReportBData reportData)
        {
            try
            {
                DateTime until = DateTime.Parse(reportData.Until);
                DateTime from = DateTime.Parse(reportData.From);
                IEnumerable<ReportTypeBElement> report = adminLogic.GenerateReportB(from, until);
                return Ok(report);
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

        [AuthenticationFilter()]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable <Admin> adminsInBD = adminLogic.GetAll();
                List<AdminDTO> adminsToReturn = new List<AdminDTO>();
                foreach (Admin admin in adminsInBD)
                {
                    AdminDTO adminDTO = new AdminDTO(admin);
                    adminsToReturn.Add(adminDTO);
                }
                return Ok(adminsToReturn);
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

        [AuthenticationFilter()]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                Admin admin = adminLogic.Get(id);
                AdminDTO adminToReturn = new AdminDTO(admin);
                return Ok(adminToReturn);
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

        [AuthenticationFilter()]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                Admin adminToDelete = adminLogic.Get(id);
                adminLogic.Remove(adminToDelete);
                return Ok(adminToDelete.Id);
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

        [AuthenticationFilter()]
        [HttpPost]
        public IActionResult Post([FromBody] AdminDTO admin)
        {
            try
            {
                Admin adminToCreate = admin.ToEntity();
                Admin createdAdmin = adminLogic.Create(adminToCreate);
                AdminDTO adminToReturn = new AdminDTO(createdAdmin);
                return Ok(adminToReturn);
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

        [AuthenticationFilter()]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] AdminDTO adminDTO)
        {
            try
            {
                Admin adminToUpdate = adminDTO.ToEntity();
                adminToUpdate.Id = id;
                Admin updatedAdmin = adminLogic.Update(adminToUpdate);
                AdminDTO adminToReturn = new AdminDTO(updatedAdmin);
                return Ok(adminToReturn);
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
