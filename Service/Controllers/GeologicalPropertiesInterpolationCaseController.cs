using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using GeologicalProperties.Service.Managers;
using GeologicalProperties.Model;

namespace GeologicalProperties.Service.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class GeologicalPropertiesInterpolationCaseController : ControllerBase
    {
        private readonly ILogger<GeologicalPropertiesInterpolationCaseManager> _logger;
        private readonly GeologicalPropertiesInterpolationCaseManager _geologicalPropertiesManagerInterpolationCase;

        public GeologicalPropertiesInterpolationCaseController(ILogger<GeologicalPropertiesInterpolationCaseManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _geologicalPropertiesManagerInterpolationCase = GeologicalPropertiesInterpolationCaseManager.GetInstance(_logger, connectionManager);
        }

        /// <summary>
        /// Returns the list of Guid of all GeologicalPropertiesInterpolationCase present in the microservice database at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase
        /// </summary>
        /// <returns>the list of Guid of all GeologicalPropertiesInterpolationCase present in the microservice database at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase</returns>
        [HttpGet(Name = "GetAllGeologicalPropertiesInterpolationCaseId")]
        public ActionResult<IEnumerable<Guid>> GetAllGeologicalPropertiesInterpolationCaseId()
        {
            var ids = _geologicalPropertiesManagerInterpolationCase.GetAllGeologicalPropertiesInterpolationCaseId();
            if (ids != null)
            {
                return Ok(ids);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the list of MetaInfo of all GeologicalPropertiesInterpolationCase present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/MetaInfo
        /// </summary>
        /// <returns>the list of MetaInfo of all GeologicalPropertiesInterpolationCase present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/MetaInfo</returns>
        [HttpGet("MetaInfo", Name = "GetAllGeologicalPropertiesInterpolationCaseMetaInfo")]
        public ActionResult<IEnumerable<MetaInfo>> GetAllGeologicalPropertiesInterpolationCaseMetaInfo()
        {
            var vals = _geologicalPropertiesManagerInterpolationCase.GetAllGeologicalPropertiesInterpolationCaseMetaInfo();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the GeologicalPropertiesInterpolationCase identified by its Guid from the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/MetaInfo/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the GeologicalPropertiesInterpolationCase identified by its Guid from the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/MetaInfo/id</returns>
        [HttpGet("{id}", Name = "GetGeologicalPropertiesInterpolationCaseById")]
        public ActionResult<GeologicalPropertiesInterpolationCase?> GetGeologicalPropertiesInterpolationCaseById(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                var val = _geologicalPropertiesManagerInterpolationCase.GetGeologicalPropertiesInterpolationCaseById(id);
                if (val != null)
                {
                    return Ok(val);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the list of all GeologicalPropertiesInterpolationCaseLight present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/LightData
        /// </summary>
        /// <returns>the list of all GeologicalPropertiesInterpolationCaseLight present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/LightData</returns>
        [HttpGet("LightData", Name = "GetAllGeologicalPropertiesInterpolationCaseLight")]
        public ActionResult<IEnumerable<GeologicalPropertiesInterpolationCaseLight>> GetAllGeologicalPropertiesInterpolationCaseLight()
        {
            var vals = _geologicalPropertiesManagerInterpolationCase.GetAllGeologicalPropertiesInterpolationCaseLight();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the list of all GeologicalPropertiesInterpolationCase present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/HeavyData
        /// </summary>
        /// <returns>the list of all GeologicalPropertiesInterpolationCase present in the microservice database, at endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/HeavyData</returns>
        [HttpGet("HeavyData", Name = "GetAllGeologicalPropertiesInterpolationCase")]
        public ActionResult<IEnumerable<Model.GeologicalPropertiesInterpolationCase?>> GetAllGeologicalPropertiesInterpolationCase()
        {
            var vals = _geologicalPropertiesManagerInterpolationCase.GetAllGeologicalPropertiesInterpolationCase();
            if (vals != null)
            {
                return Ok(vals);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Performs calculation on the given GeologicalPropertiesInterpolationCase and adds it to the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalPropertiesInterpolationCase has been added successfully to the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase</returns>
        [HttpPost(Name = "PostGeologicalPropertiesInterpolationCase")]
        public ActionResult PostGeologicalPropertiesInterpolationCase([FromBody] GeologicalPropertiesInterpolationCase? data)
        {
            // Check if geologicalProperties exists in the database through ID
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID != Guid.Empty)
            {
                var existingData = _geologicalPropertiesManagerInterpolationCase.GetGeologicalPropertiesInterpolationCaseById(data.MetaInfo.ID);
                if (existingData == null)
                {
                    //  If geologicalPropertiesInterpolationCase was not found, call AddGeologicalPropertiesInterpolationCase, where the geologicalPropertiesInterpolationCase.Calculate()
                    // method is called. 
                    if (_geologicalPropertiesManagerInterpolationCase.AddGeologicalPropertiesInterpolationCase(data))
                    {
                        return Ok(); // status=OK is used rather than status=Created because NSwag auto-generated controllers use 200 (OK) rather than 201 (Created) as return codes
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    _logger.LogWarning("The given GeologicalPropertiesInterpolationCase already exists and will not be added");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalPropertiesInterpolationCase is null, badly formed, or its ID is empty");
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs calculation on the given GeologicalPropertiesInterpolationCase and updates it in the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/id
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalPropertiesInterpolationCase has been updated successfully to the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/id</returns>
        [HttpPut("{id}", Name = "PutGeologicalPropertiesInterpolationCaseById")]
        public ActionResult PutGeologicalPropertiesInterpolationCaseById(Guid id, [FromBody] Model.GeologicalPropertiesInterpolationCase? data)
        {
            // Check if GeologicalProperties is in the data base
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID.Equals(id))
            {
                var existingData = _geologicalPropertiesManagerInterpolationCase.GetGeologicalPropertiesInterpolationCaseById(id);
                if (existingData != null)
                {
                    if (_geologicalPropertiesManagerInterpolationCase.UpdateGeologicalPropertiesInterpolationCaseById(id, data))
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    _logger.LogWarning("The given GeologicalPropertiesInterpolationCase has not been found in the database");
                    return NotFound();
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalPropertiesInterpolationCase is null, badly formed, or its does not match the ID to update");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the GeologicalPropertiesInterpolationCase of given ID from the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the GeologicalProperties was deleted from the microservice database, at the endpoint GeologicalProperties/api/GeologicalPropertiesInterpolationCase/id</returns>
        [HttpDelete("{id}", Name = "DeleteGeologicalPropertiesInterpolationCaseById")]
        public ActionResult DeleteGeologicalPropertiesInterpolationCaseById(Guid id)
        {
            if (_geologicalPropertiesManagerInterpolationCase.GetGeologicalPropertiesInterpolationCaseById(id) != null)
            {
                if (_geologicalPropertiesManagerInterpolationCase.DeleteGeologicalPropertiesInterpolationCaseById(id))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _logger.LogWarning("The GeologicalPropertiesInterpolationCase of given ID does not exist");
                return NotFound();
            }
        }
    }
}
