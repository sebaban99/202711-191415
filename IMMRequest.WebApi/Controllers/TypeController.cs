using IMMRequest.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
        [AutenticationFilter()]
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
    }
}