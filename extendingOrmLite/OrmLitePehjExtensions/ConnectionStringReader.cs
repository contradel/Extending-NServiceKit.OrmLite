using System;
using System.IO;

namespace OrmLitePehjExtensions
{
	public static class ConnectionStringReader
	{
		public static string GetConnectionString(string path)
		{
			using (var file = new StreamReader(path))
			{
				string line;
				while ((line = file.ReadLine()) != null)
				{
					if (!line.Contains("#"))
						return line;
				}
			}
			throw new Exception("No suitable connection string in file");
		}
	}
}
