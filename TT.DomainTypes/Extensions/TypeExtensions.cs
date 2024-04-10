using System;
using System.Collections.Concurrent;

namespace TT.DomainTypes.Extensions
{
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, bool> KnownDomainTypes = new();
        public static bool IsDomainType(this Type typeToCheck) => IsDomainType(typeToCheck, out _);

        public static bool IsDomainType(this Type typeToCheck, out Type? underlingType)
        {
            return KnownDomainTypes.GetOrAdd(typeToCheck, DetermineIfDomainType(typeToCheck, out underlingType));
        }

        private static bool DetermineIfDomainType(this Type? typeToCheck, out Type? underlingType)
        {
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                var type = typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;
                if (type == typeof(DomainType<>))
                {
                    underlingType = typeToCheck.GetGenericArguments()[0];
                    return true;
                }

                typeToCheck = typeToCheck.BaseType!;
            }

            underlingType = null;
            return false;
        }
    }
}