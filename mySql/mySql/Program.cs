using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using mySql.System.Data;
using NServiceKit.OrmLite;
using NServiceKit.OrmLite.MySql;

namespace mySql
{
	class Program
	{
		static void Main(string[] args)
		{
			const string connectionStringPath = @"\\psf\Dropbox\Dox\Mac-Git\connectionString.txt";
			string connectionString = ConnectionStringReader.GetConnectionString(connectionStringPath);

			var dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialectProvider.Instance);
			ISqlProvider customProvider = new MySqlProvider();

			using (var db = dbFactory.OpenDbConnection())
			{
				db.UpdateTable<Customer>(customProvider, true);
				db.UpdateTable<Order>(customProvider, true);

				//db.Insert(new Order
				//{
				//	Freight = 2,
				//	OrderDate = DateTime.Now,
				//	RequiredDate = DateTime.Now + new TimeSpan(1),
				//	Total = 999,
				//	Hue = 2
				//});



				Console.ReadKey();
			}
		}
	}
}