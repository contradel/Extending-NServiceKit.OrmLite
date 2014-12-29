using System;
using System.Data.SqlTypes;

namespace OrmLitePehjExtensions
{
	public static class SqlTypeConverter
	{
		public static Type GetNullableType(Type TypeToConvert)
		{
			// Abort if no type supplied
			if (TypeToConvert == null)
				return null;

			// If the type is a ValueType and is not System.Void, convert it to a Nullable<Type>
			if (TypeToConvert.IsValueType && TypeToConvert != typeof(void))
				return typeof(Nullable<>).MakeGenericType(TypeToConvert);

			// Done - no conversion
			return null;
		}

		public static Type GetTypeFromSqlString(string sqlType)
		{
			Type returnType = null;
			if (sqlType.Contains("bigint"))
			{
				returnType = typeof (ulong);
			}
			else if (sqlType.Contains("bit"))
			{
				returnType = typeof (bool);
			}
			else if (sqlType.Contains("date"))
			{
				returnType = typeof(DateTime);
			}
			else if (sqlType.Contains("decimal"))
			{
				returnType = typeof(decimal);
			}
			else if (sqlType.Contains("int"))
			{
				returnType = typeof(int);
			}
			else if (sqlType.Contains("money"))
			{
				returnType = typeof(decimal);
			}
			else if (sqlType.Contains("nchar"))
			{
				returnType = typeof(string);
			}
			else if (sqlType.Contains("numeric"))
			{
				returnType = typeof(decimal);
			}
			else if (sqlType.Contains("nvarchar"))
			{
				returnType = typeof(string);
			}
			else if (sqlType.Contains("uniqueidentifier"))
			{
				returnType = typeof(Guid);
			}
			else if (sqlType.Contains("varchar"))
			{
				returnType = typeof(string);
			}
			else if (sqlType.Contains("double"))
			{
				returnType = typeof(double);
			}
			else if (sqlType.Contains("float"))
			{
				returnType = typeof(double);	//see http://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
			}

			if (!sqlType.Contains("NULL"))
			{
				if (returnType != null) 
					return returnType;
			}
			else
			{
				if (returnType != null) 
					return GetNullableType(returnType);
			}

			throw new SqlNullValueException("Cannot convert SQL type to .NET type. Sql type was: " + sqlType);
		}
	}
}