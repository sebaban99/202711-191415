using System;
using System.Collections.Generic;
using System.Linq;
using IMMRequest.Importer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IMMRequest.Exceptions;
using IMMRequest.Domain;
using IMMRequest.BusinessLogic.Interfaces;

namespace IMMRequest.WebApi
{
    [Route("api/[controller]")]
    public class ImportationController : ControllerBase
    {
        private IImportationLogic importationLogic;
        private IImpElementParser ImpElementParser;
            
        //Agregar inyeccion a clase de la logica ImportLogic que valide lo que se va a agregar y agregue
        public ImportationController(IImportationLogic importationLogic, IImpElementParser impElementParser)
        {
            this.importationLogic = importationLogic;
            this.ImpElementParser = impElementParser;
        }

        [HttpGet()]
        public IActionResult GetImporters()
        {
            try
            {
                List<IImporter> list = importationLogic.GetImportationsMethods(@"Importers");
                if(list.Count == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (ImportException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost()]
        public IActionResult Import([FromBody] ImportInfoDTO impInfo)
        {
            try
            {
                importationLogic.GetImportationsMethods(@"Importers");
                IImporter importer = importationLogic.GetImporter(impInfo.importationMethod);
                List<AreaImpModel> importedElements = importer.ImportFile(impInfo.filePath);
                List<Area> realElemnts = ImpElementParser.ParseElements(importedElements);
                //Llamar a clase de logica que valide y agregue o lance una excepcion si hay elementos invalidos
                return Ok();
            }
            catch (Exception e)
            when (e is ImportException || e is BusinessLogicException)
            {
                return BadRequest(e.Message);
            }
        }
    }
}