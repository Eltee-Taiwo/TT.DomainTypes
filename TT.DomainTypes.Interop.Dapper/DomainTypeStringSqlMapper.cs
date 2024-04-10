using System;
using System.Data;
using Dapper;

namespace TT.DomainTypes.Interop.Dapper
{
    public class DomainTypeStringSqlMapper<T> : SqlMapper.TypeHandler<T> where T : DomainTypeString
    {
        public override void SetValue(IDbDataParameter parameter, T? value)
        {
            parameter.Value = value?.Value;
        }

        public override T Parse(object value)
        {
            return (string.IsNullOrWhiteSpace(value.ToString()) ? null : (T)Activator.CreateInstance(typeof(T),  value.ToString())!)!;
        }
    }
}
