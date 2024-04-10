using System;
using System.ComponentModel;
using System.Globalization;

namespace TT.DomainTypes.Converters;

/// <summary>
/// Type converter for  <see cref="DomainType{TValue}"/>. Useful for serialization and deserialization
/// as well as converting objects to and from.
/// <br />
/// <br />
/// Exposed via <see cref="DomainTypeConverter"/>
/// </summary>
/// <typeparam name="TValue"></typeparam>
internal class DomainTypeGenericConverter<TValue> : TypeConverter where TValue : notnull
{
    private readonly Type _type;
    //private static readonly TypeConverter IdValueConverter = GetIdValueConverter();

    public DomainTypeGenericConverter(Type type)
    {
        _type = type;
    }
		
    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) ||
               sourceType == typeof(TValue) ||
               base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) ||
               destinationType == typeof(TValue) ||
               base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
        {
            var domainTypeValueConverter = GetDomainTypeValueConverter();
            value = domainTypeValueConverter.ConvertFrom(s)!;
        }

        if (value is TValue domainTypeValue)
        {
            var factory = DomainType.GetFactory<TValue>(_type);
            return factory(domainTypeValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/> 
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var canBeNull = !destinationType.IsValueType || (Nullable.GetUnderlyingType(destinationType) != null);

        if (value is null && !canBeNull)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var domainType = (DomainType<TValue>)value!;

        if (destinationType == typeof(string))
        {
            return domainType.Value.ToString();
        }

        if (destinationType == typeof(TValue))
        {
            return canBeNull ? null : domainType.Value;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    private static TypeConverter GetDomainTypeValueConverter()
    {
        var converter = TypeDescriptor.GetConverter(typeof(TValue));

        if (!converter.CanConvertFrom(typeof(string))){
            throw new InvalidOperationException($"Type '{typeof(TValue)}' doesn't have a converter that can convert from string");
        }

        return converter;
    }
}