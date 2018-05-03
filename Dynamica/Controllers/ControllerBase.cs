using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dynamica.Models;

namespace Dynamica.Controllers
{
    // TODO make it generic, custom type controllers should be derived from this type
    [Route("api/[controller]")]
    public class ControllerBase : Controller {
        private readonly ItemsService _itemsService;
        private readonly Type _itemType;

        public ControllerBase(ItemsService itemsService, Type itemType)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _itemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
        }

        [HttpGet]
        public IActionResult Get() => Ok(_itemsService.GetItems(_itemType));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _itemsService
                .GetItems(_itemType)
                .FirstOrDefault(
                item => (item as BaseModel)?.Id == id) ?? throw new Exception("Is not BaseModel type"); // TODO add ability to query type if it has ID property

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]object value)
        {
            var items = _itemsService.GetItems(_itemType).ToList();

            var isDerivedFromBaseModel = value is BaseModel;

            if (isDerivedFromBaseModel)
            {
                var maxId = items.Any() ? 0 : items.Max(x => (x as BaseModel).Id);
                var nextId = maxId + 1;
                var baseModelDescendent = value as BaseModel;
                baseModelDescendent.Id = maxId;
                value = baseModelDescendent;
            }

            items.Add(value);
            _itemsService.SaveItems(items);

            return Ok(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
