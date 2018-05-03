using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynamica
{
    internal sealed class ItemsService
    {
        public void SaveItems(IEnumerable<object> items, Type type = null)
        {
            if (!items.Any())
                return;

            type = type ?? items.First().GetType();
            JArray arr = JArray.FromObject(items);

            string filename = $"{type.Name}.json";
            System.IO.File.WriteAllText(filename, arr.ToString());
        }

        public IEnumerable<object> GetItems(Type type)
        {
            string filename = $"{type.Name}.json";

            if (!System.IO.File.Exists(filename))
                throw new Exception($"file {filename} does not exist");

            var itemArrayStr = System.IO.File.ReadAllText(filename);
            var items = JArray.Parse(itemArrayStr).Children().Select(x => x.ToObject(type));
            return items;
        }
    }
}
