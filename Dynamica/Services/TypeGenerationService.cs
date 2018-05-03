using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynamica.Services
{
    public sealed class TypeGenerationService
    {
        public TypeGenerationService() //add custom generated type provider (loader)
        {
            var supportedTypes = new[]{
                typeof(string),
                typeof(int),
                typeof(DateTime),
                typeof(bool),
                typeof(object),
                typeof(Array)
                // TODO add custom types
            };

            _supportedPropertyTypes = supportedTypes.ToDictionary(type => type.Name, type => type);
        }

        public Type GenerateTypeFromPropertyInfo(IEnumerable<KeyValuePair<string, Type>> properties)
        {
            // TODO 
            throw new NotImplementedException();
        }

        public Type ResolveTypeByName(string typeName)
        {
            if (!_supportedPropertyTypes.ContainsKey(typeName))
                throw new Exception($"Unsupported property type : {typeName}");

            return _supportedPropertyTypes[typeName];
        }

        private readonly Dictionary<string, Type> _supportedPropertyTypes;
    }
}
