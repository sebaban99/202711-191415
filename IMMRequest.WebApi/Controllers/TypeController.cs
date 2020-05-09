﻿using IMMRequest.BusinessLogic;
using IMMRequest.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private ITypeLogic typeLogic;

        public TypeController(ITypeLogic typeLogic)
        {
            this.typeLogic = typeLogic;
        }

        [AuthenticationFilter()]
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Type> typesInBD = typeLogic.GetAll();
            List<TypeDTO> typesToReturn = new List<TypeDTO>();
            foreach (Type type in typesInBD)
            {
                TypeDTO tm = new TypeDTO(type);
                typesToReturn.Add(tm);
            }
            return Ok(typesToReturn);
        }

        [AuthenticationFilter()]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                Type type = typeLogic.Get(id);
                TypeDTO typeToReturn = new TypeDTO(type);
                return Ok(typeToReturn);
            }
            catch (Exception e)
            when (e is BusinessLogicException || e is DataAccessException)
            {
                return NotFound(e.Message);
            }
        }

        [AuthenticationFilter()]
        [HttpPost]
        public IActionResult Post([FromBody] TypeDTO typeDTO)
        {
            try
            {
                Type typeToCreate = typeDTO.ToEntity();
                Type createdType = typeLogic.Create(typeToCreate);
                TypeDTO typeToReturn = new TypeDTO(createdType);
                return Ok(typeToReturn);
            }
            catch (Exception e)
            when (e is BusinessLogicException || e is DataAccessException)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthenticationFilter()]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                Type typeToDelete = typeLogic.Get(id);
                typeLogic.Remove(typeToDelete);
                return Ok(typeToDelete.Id);
            }
            catch (Exception e)
            when (e is BusinessLogicException || e is DataAccessException)
            {
                return NotFound(e.Message);
            }
        }
    }
}