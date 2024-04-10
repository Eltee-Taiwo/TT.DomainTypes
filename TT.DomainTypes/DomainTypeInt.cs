using System.ComponentModel;
using TT.DomainTypes.Converters;

namespace TT.DomainTypes;

[TypeConverter(typeof(DomainTypeConverter))]
public record DomainTypeInt(int Value) : DomainType<int>(Value)
{
    public static implicit operator int(DomainTypeInt domainType) => domainType.Value;
    public static explicit operator DomainTypeInt(int value) => new (value);
}