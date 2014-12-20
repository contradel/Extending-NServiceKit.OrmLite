Extending-NServiceKit.OrmLite
=============================

This extension requires https://www.nuget.org/packages/ServiceStack.OrmLite
This extension can keep your database schema in sync with your model, just by doing this:

```csharp
var dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialectProvider.Instance);
			ISqlProvider customProvider = new MySqlProvider();

			using (var db = dbFactory.OpenDbConnection())
			{
				db.UpdateTable<Customer>(customProvider, true);
				db.UpdateTable<Order>(customProvider, true);
			}
```

where MySqlDialectProvider.Instance could be any DialectProvider given by OrmLite
where ISqlProvider is a simple interface you quickly can derive, 
but is already implemented and tested for MySql and implemented for MsSql

```csharp
	public interface ISqlProvider
	{
		string TableInformation { get; }
		string TableColumn { get; }
		string DropColumn { get; }
		string DropTable { get; }
		string AlterTable { get; }
	}
```

Still needs refining

Read more here https://github.com/NServiceKit/NServiceKit.OrmLite
