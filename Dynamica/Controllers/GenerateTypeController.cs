using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dynamica.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Dynamica.Controllers
{
    [Route("api/[controller]")]
    public class GenerateTypeController : Controller
    {
        private readonly TypeGenerationService _service;

        public GenerateTypeController(TypeGenerationService service) // add IoC
        {
            _service = service ?? throw new NullReferenceException($"Missing argument {nameof(service)}");
        }


        // TODO also add previously generated datatypes
        [HttpGet("supportedDefaultTypes")]
        public IActionResult GetSupportedTypes()
            => Ok(new[]
            {
                typeof(string).Name,
                typeof(int).Name,
                typeof(DateTime).Name,
                typeof(bool).Name,
                typeof(object).Name,
                typeof(Array).Name
            });

        // TODO provide all registered custom types with endpoints.
        [HttpGet]
        public IActionResult Get() => throw new NotImplementedException();

        [HttpPost("{endpointName}")]
        public IActionResult Post(string endpointName, [FromBody]object typeDefinition)
        {
            JObject jsonTypeDefinition = JObject.FromObject(typeDefinition);

            var rawPropsData = jsonTypeDefinition
                .Properties()
                .Select(jProperty => new KeyValuePair<string, string>(jProperty.Name, jProperty.Value.ToString()));

            var propsInfo = rawPropsData
                .Select(rawPropData => new KeyValuePair<string, Type>(rawPropData.Key, _service.ResolveTypeByName(rawPropData.Value)));

            Type itemType = _service.GenerateTypeFromPropertyInfo(propsInfo);
            
            // TODO register created type here

            return Ok(itemType);
        }
    }
}
