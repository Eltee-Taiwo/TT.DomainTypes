using System.Data;
using Dapper;

namespace TT.DomainTypes.Interop.Dapper;

public class DomainTypeIntSqlMapper<T> : SqlMapper.TypeHandler<T> where T : DomainTypeInt
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value?.Value;
    }

    public override T Parse(object value)
    {
        return (int.TryParse(value.ToString(), out var intValue) ? (T)Activator.CreateInstance(typeof(T), intValue)! : null)!;
    }
}