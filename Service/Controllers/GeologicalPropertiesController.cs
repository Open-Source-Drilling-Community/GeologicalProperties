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
    public class GeologicalPropertiesController : ControllerBase
    {
        private readonly ILogger<GeologicalPropertiesManager> _logger;
        private readonly GeologicalPropertiesManager _geologicalPropertiesManager;

        public GeologicalPropertiesController(ILogger<GeologicalPropertiesManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _geologicalPropertiesManager = GeologicalPropertiesManager.GetInstance(_logger, connectionManager);
        }

        /// <summary>
        /// Returns the list of Guid of all GeologicalProperties present in the microservice database at endpoint GeologicalProperties/api/GeologicalProperties
        /// </summary>
        /// <returns>the list of Guid of all GeologicalProperties present in the microservice database at endpoint GeologicalProperties/api/GeologicalProperties</returns>
        [HttpGet(Name = "GetAllGeologicalPropertiesId")]
        public ActionResult<IEnumerable<Guid>> GetAllGeologicalPropertiesId()
        {
            var ids = _geologicalPropertiesManager.GetAllGeologicalPropertiesId();
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
        /// Returns the list of MetaInfo of all GeologicalProperties present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/MetaInfo
        /// </summary>
        /// <returns>the list of MetaInfo of all GeologicalProperties present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/MetaInfo</returns>
        [HttpGet("MetaInfo", Name = "GetAllGeologicalPropertiesMetaInfo")]
        public ActionResult<IEnumerable<MetaInfo>> GetAllGeologicalPropertiesMetaInfo()
        {
            var vals = _geologicalPropertiesManager.GetAllGeologicalPropertiesMetaInfo();
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
        /// Returns the GeologicalProperties identified by its Guid from the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/MetaInfo/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the GeologicalProperties identified by its Guid from the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/MetaInfo/id</returns>
        [HttpGet("{id}", Name = "GetGeologicalPropertiesById")]
        public ActionResult<GeologicalProperties.Model.GeologicalProperties?> GetGeologicalPropertiesById(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                var val = _geologicalPropertiesManager.GetGeologicalPropertiesById(id);
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
        /// Returns the list of all GeologicalPropertiesLight present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/LightData
        /// </summary>
        /// <returns>the list of all GeologicalPropertiesLight present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/LightData</returns>
        [HttpGet("LightData", Name = "GetAllGeologicalPropertiesLight")]
        public ActionResult<IEnumerable<Model.GeologicalPropertiesLight>> GetAllGeologicalPropertiesLight()
        {
            var vals = _geologicalPropertiesManager.GetAllGeologicalPropertiesLight();
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
        /// Returns the list of all GeologicalProperties present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/HeavyData
        /// </summary>
        /// <returns>the list of all GeologicalProperties present in the microservice database, at endpoint GeologicalProperties/api/GeologicalProperties/HeavyData</returns>
        [HttpGet("HeavyData", Name = "GetAllGeologicalProperties")]
        public ActionResult<IEnumerable<Model.GeologicalProperties?>> GetAllGeologicalProperties()
        {
            var vals = _geologicalPropertiesManager.GetAllGeologicalProperties();
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
        /// Performs calculation on the given GeologicalProperties and adds it to the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalProperties has been added successfully to the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties</returns>
        [HttpPost(Name = "PostGeologicalProperties")]
        public ActionResult PostGeologicalProperties([FromBody] GeologicalProperties.Model.GeologicalProperties? data)
        {
            // Check if geologicalProperties exists in the database through ID
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID != Guid.Empty)
            {
                var existingData = _geologicalPropertiesManager.GetGeologicalPropertiesById(data.MetaInfo.ID);
                if (existingData == null)
                {   
                    //  If geologicalProperties was not found, call AddGeologicalProperties, where the geologicalProperties.Calculate()
                    // method is called. 
                    if (_geologicalPropertiesManager.AddGeologicalProperties(data))
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
                    _logger.LogWarning("The given GeologicalProperties already exists and will not be added");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalProperties is null, badly formed, or its ID is empty");
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs calculation on the given GeologicalProperties and updates it in the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties/id
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalProperties has been updated successfully to the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties/id</returns>
        [HttpPut("{id}", Name = "PutGeologicalPropertiesById")]
        public ActionResult PutGeologicalPropertiesById(Guid id, [FromBody] Model.GeologicalProperties? data)
        {
            // Check if GeologicalProperties is in the data base
            if (data != null && data.MetaInfo != null && data.MetaInfo.ID.Equals(id))
            {
                var existingData = _geologicalPropertiesManager.GetGeologicalPropertiesById(id);
                if (existingData != null)
                {
                    if (_geologicalPropertiesManager.UpdateGeologicalPropertiesById(id, data))
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
                    _logger.LogWarning("The given GeologicalProperties has not been found in the database");
                    return NotFound();
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalProperties is null, badly formed, or its does not match the ID to update");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the GeologicalProperties of given ID from the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties/id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the GeologicalProperties was deleted from the microservice database, at the endpoint GeologicalProperties/api/GeologicalProperties/id</returns>
        [HttpDelete("{id}", Name = "DeleteGeologicalPropertiesById")]
        public ActionResult DeleteGeologicalPropertiesById(Guid id)
        {
            if (_geologicalPropertiesManager.GetGeologicalPropertiesById(id) != null)
            {
                if (_geologicalPropertiesManager.DeleteGeologicalPropertiesById(id))
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
                _logger.LogWarning("The GeologicalProperties of given ID does not exist");
                return NotFound();
            }
        }
    }
}
