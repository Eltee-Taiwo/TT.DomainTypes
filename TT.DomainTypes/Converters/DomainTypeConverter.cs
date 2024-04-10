using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using TT.DomainTypes.Extensions;

namespace TT.DomainTypes.Converters
{
	/// <summary>
	/// Wrapper for the type converter <see cref="DomainTypeGenericConverter{TValue}"/> of a generic domain type <see cref="DomainType{TValue}"/>
	/// </summary>
	internal class DomainTypeConverter : TypeConverter
	{
		private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();
		private readonly TypeConverter _innerConverter;

		public DomainTypeConverter(Type domainType)
		{
			_innerConverter = ActualConverters.GetOrAdd(domainType, CreateActualConverter);
		}

		/// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
			_innerConverter.CanConvertFrom(context, sourceType);

		/// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
		public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
			_innerConverter.CanConvertTo(context, destinationType);

		/// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
			_innerConverter.ConvertFrom(context, culture, value);

		/// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/> 
		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) =>
			_innerConverter.ConvertTo(context, culture, value, destinationType);


		private static TypeConverter CreateActualConverter(Type type)
		{
			if (!type.IsDomainType(out var valueType))
			{
				throw new InvalidOperationException($"The type '{type}' is not a domain type");
			}

			var actualConverterType = typeof(DomainTypeGenericConverter<>).MakeGenericType(valueType!);
			return (TypeConverter)Activator.CreateInstance(actualConverterType, type)!;
		}
	}
}
