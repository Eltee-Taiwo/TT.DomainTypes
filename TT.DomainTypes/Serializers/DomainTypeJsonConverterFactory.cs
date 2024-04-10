using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using TT.DomainTypes.Extensions;

namespace TT.DomainTypes.Serializers;

/// <summary>
/// This allows Domain Types to be serialized to their underlying values as well as
/// allowing them to be deserialized from their native values.
/// </summary>
public class DomainTypeJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    /// <inheritdoc cref="JsonConverterFactory.CanConvert"/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsDomainType();
    }

    /// <inheritdoc cref="JsonConverterFactory.CreateConverter"/>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, CreateConverter);
    }

    private static JsonConverter CreateConverter(Type typeToConvert)
    {
        if (!typeToConvert.IsDomainType(out var valueType))
            throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");

        var type = typeof(DomainTypeJsonConverter<,>).MakeGenericType(typeToConvert, valueType!);

        return ((JsonConverter)Activator.CreateInstance(type)!);
    }
}