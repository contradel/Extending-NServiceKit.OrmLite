using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using mySql;
using NServiceKit.OrmLite;
using NServiceKit.OrmLite.MySql;
using NUnit.Framework;
using OrmLitePehjExtensions;

namespace mySqlTests
{
	[TestFixture]
	class DbConnectionExtensionsTest
	{
		private static string connectionString_;
		public static ISqlProvider CustomProvider = new MySqlProvider();

		class TestClass
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		[SetUp]
		public void Init()
		{
			const string connectionStringPath = @"\\psf\Dropbox\Dox\Mac-Git\connectionString.txt";
			connectionString_ = ConnectionStringReader.GetConnectionString(connectionStringPath);
			var dbFactory = new OrmLiteConnectionFactory(connectionString_, MySqlDialectProvider.Instance);
			using (var db = dbFactory.OpenDbConnection())
			{
				db.CreateTable<TestClass>(true);
			}
		}

		[TearDown]
		public void Dispose()
		{
			var dbFactory = new OrmLiteConnectionFactory(connectionString_, MySqlDialectProvider.Instance);
			using (var db = dbFactory.OpenDbConnection())
			{
				var model = ModelDefinition<TestClass>.Definition;
				var sql = CustomProvider.DropTable + " " + db.GetDialectProvider().GetQuotedTableName(model);
				db.ExecuteSql(sql);
			}
		}

		[Test]
		public static void SetupTest()
		{
			var dbFactory = new OrmLiteConnectionFactory(connectionString_, MySqlDialectProvider.Instance);

			using (var db = dbFactory.OpenDbConnection())
			{
				Assert.DoesNotThrow(() =>
				{
					db.CreateTable<TestClass>();
					db.Insert(new TestClass
					{
						Name = "Test",
						Age = 50
					});
					var test = db.Select<TestClass>().FirstOrDefault();
					Assert.NotNull(test);
					test.Name = "TestName";
					test.Age = 50;
				});				
			}
		}
	}
}
