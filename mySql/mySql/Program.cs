using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
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

				//db.Insert(new Customer
				//{
				//	Name = "Hansi",
				//	Company = "KMD",
				//	Birthday = new DateTime(1979, 3, 8)
				//});

				//db.Insert(new Customer
				//{
				//	Name = "Karsten",
				//	Company = "Fakta",
				//	Birthday = new DateTime(1990, 12, 29)
				//});

				//db.Insert(new Order
				//{
				//	CustomerId = db.Select<Customer>(x => x.Name == "Hansi").FirstOrDefault().Id,
				//	Freight = 2032.32m,
				//	Total = 2323.23m,
				//	OrderDate = DateTime.Now,
				//	RequiredDate = DateTime.Now.AddDays(7),
				//	ShipVia = 232
				//});



				Console.ReadKey();
			}
		}
	}
}