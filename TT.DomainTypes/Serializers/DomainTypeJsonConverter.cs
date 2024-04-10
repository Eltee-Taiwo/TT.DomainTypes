using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TT.DomainTypes.Serializers;

internal class DomainTypeJsonConverter<TDomainType, TValue> : JsonConverter<TDomainType>
    where TDomainType : DomainType<TValue>
    where TValue : notnull
{
    public override TDomainType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        var value = JsonSerializer.Deserialize<TValue>(ref reader, options);
        if (value == null) { return null; }

        var factory = DomainType.GetFactory<TValue>(typeToConvert);

        return (TDomainType)factory(value);
    }

    public override void Write(Utf8JsonWriter writer, TDomainType? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}