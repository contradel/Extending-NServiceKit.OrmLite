using System.IO;

namespace OrmLitePehjExtensions
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
