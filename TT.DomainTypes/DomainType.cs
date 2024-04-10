using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using TT.DomainTypes.Converters;
using TT.DomainTypes.Extensions;

namespace TT.DomainTypes;

[TypeConverter(typeof(DomainTypeConverter))]
public abstract record DomainType<T>(T Value) where T : notnull
{
    public virtual bool Equals(DomainType<T>? other)
    {
        return other != null && Value.Equals(other.Value);
    }
    public virtual bool Equals(T? other)
    {
        return other != null && Value.Equals(other);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Value);
    }

    public sealed override string? ToString()
    {
        return Value.ToString();
    }
}

internal static class DomainType
{
    private static readonly ConcurrentDictionary<Type, Delegate> DomainTypeFactories = new();

    public static Func<TValue, object> GetFactory<TValue>(Type domainType) where TValue : notnull
    {
        return (Func<TValue, object>)DomainTypeFactories.GetOrAdd(
            domainType,
            CreateFactory<TValue>
        );
    }

    private static Func<TValue, object> CreateFactory<TValue>(Type domainType) where TValue : notnull
    {
        if (!domainType.IsDomainType())
        {
            throw new ArgumentException($"Type '{domainType}' is not a strongly-typed id type", nameof(domainType));
        }

        var ctor = domainType.GetConstructor(new[] { typeof(TValue) });
        if (ctor is null)
        {
            throw new ArgumentException(
                $"Type '{domainType}' doesn't have a constructor with one parameter of type '{typeof(TValue)}'",
                nameof(domainType)
            );
        }

        var param = Expression.Parameter(typeof(TValue), "value");
        var body = Expression.New(ctor, param);
        var lambda = Expression.Lambda<Func<TValue, object>>(body, param);
        return lambda.Compile();
    }

}