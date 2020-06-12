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
        private IImpElementParser impElementParser;
        private IImportProcessing importProcessing;
            
        public ImportationController(IImportationLogic importationLogic, IImpElementParser impElementParser,
            IImportProcessing importProcessing)
        {
            this.importationLogic = importationLogic;
            this.impElementParser = impElementParser;
            this.importProcessing = importProcessing;
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
                List<Area> realElemnts = impElementParser.ParseElements(importedElements);
                importProcessing.ProcessImportedElements(realElemnts);
                return Ok();
            }
            catch (Exception e)
            when (e is ImportException || e is BusinessLogicException || e is DataAccessException)
            {
                return BadRequest(e.Message);
            }
        }
    }
}