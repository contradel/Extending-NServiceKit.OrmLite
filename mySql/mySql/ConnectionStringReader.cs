using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace mySql
{
	public static class ConnectionStringReader
	{
		public static string GetConnectionString(string path)
		{
			using (var sr = new StreamReader(path))
			{
				var line = sr.ReadToEnd();
				return line;
			}
		}
	}
}
