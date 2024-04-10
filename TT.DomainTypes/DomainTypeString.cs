using System;
using System.ComponentModel;
using TT.DomainTypes.Converters;

namespace TT.DomainTypes;

[TypeConverter(typeof(DomainTypeConverter))]
public record DomainTypeString(string Value) : DomainType<string>(Value)
{
    /// <summary>
    /// Returns a value indicating whether a specified string occurs within this domain type string using the specified comparison rules.
    /// </summary>
    /// <param name="substring">THe string to seek</param>
    /// <param name="compareMethod"></param>
    /// <returns>true if the <param name="substring"></param> occurs within this string otherwise false</returns>
    public bool Contains(string substring, StringComparison compareMethod = StringComparison.InvariantCultureIgnoreCase)
    {
        return Value.Contains(substring, compareMethod);
    }


    /// <summary>
    /// Returns a value indicating whether a specified domain type string occurs within this domain type string using the specified comparison rules.
    /// </summary>
    /// <param name="substring">The domain type string to seek</param>
    /// <param name="compareMethod"></param>
    /// <returns>true if the <param name="substring"></param> occurs within this string otherwise false</returns>
    public bool Contains(DomainTypeString? substring, StringComparison compareMethod = StringComparison.InvariantCultureIgnoreCase)
    {
        return substring != null && Value.Contains(substring.Value, compareMethod);
    }

    public bool Equals(string otherValue, StringComparison compareMethod = StringComparison.InvariantCultureIgnoreCase)
    {
        return Value.Equals(otherValue, compareMethod);
    }

    public bool Equals(DomainTypeString? otherValue, StringComparison compareMethod = StringComparison.InvariantCultureIgnoreCase)
    {
        return otherValue != null && Value.Equals(otherValue.Value, compareMethod);
    }

    public static bool IsNullOrWhiteSpace(DomainTypeString? other)
    {
        return string.IsNullOrWhiteSpace(other?.Value);
    }
}