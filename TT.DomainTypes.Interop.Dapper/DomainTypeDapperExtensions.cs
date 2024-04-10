using System.Reflection;
using Dapper;
using TT.DomainTypes.Extensions;

namespace TT.DomainTypes.Interop.Dapper;

public static class DomainTypeDapperExtensions
{
    public static void AddDapperTypeHandlers(IEnumerable<AssemblyName> assemblyNames)
    {
        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            var domainTypes = assembly.GetTypes().Where(type => type.IsDomainType());

            foreach (var domainType in domainTypes)
            {
                if (domainType.GetTypeInfo().IsSubclassOf(typeof(DomainTypeString)))
                {
                    var genericType = typeof(DomainTypeStringSqlMapper<>).MakeGenericType(domainType);
                    var handler = (SqlMapper.ITypeHandler)Activator.CreateInstance(genericType)!;
                    SqlMapper.AddTypeHandler(domainType, handler);
                }
                else if (domainType.GetTypeInfo().IsSubclassOf(typeof(DomainTypeInt)))
                {
                    var genericType = typeof(DomainTypeIntSqlMapper<>).MakeGenericType(domainType);
                    var instance = (SqlMapper.ITypeHandler)Activator.CreateInstance(genericType)!;
                    SqlMapper.AddTypeHandler(domainType, instance);
                }
            }
        }
    }
}