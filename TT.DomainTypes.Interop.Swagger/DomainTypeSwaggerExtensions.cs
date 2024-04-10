using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TT.DomainTypes.Extensions;

namespace TT.DomainTypes.Interop.Swagger
{
    public static class DomainTypeSwaggerExtensions
    {
        public static void RegisterDomainTypesForSwagger(this SwaggerGenOptions self, IEnumerable<AssemblyName> assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                var domainTypes = assembly.GetTypes().Where(type => type.IsDomainType());

                foreach (var domainType in domainTypes)
                {
                    if (domainType.GetTypeInfo().IsSubclassOf(typeof(DomainTypeString)))
                    {
                        self.MapType(domainType,
                            () => new OpenApiSchema { Type = "string", Example = new OpenApiString("chicken") });
                    }
                    else if (domainType.GetTypeInfo().IsSubclassOf(typeof(DomainTypeInt)))
                    {
                        self.MapType(domainType,
                            () => new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(-1) });
                    }
                }
            }
        }
    }
}
