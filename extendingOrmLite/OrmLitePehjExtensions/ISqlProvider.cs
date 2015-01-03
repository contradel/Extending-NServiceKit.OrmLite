using NServiceKit.DataAnnotations;

namespace OrmLitePehjExtensions
{
	public class ToNullableTest
	{
		[AutoIncrement]
		public int Id { get; set; }
		public int Test { get; set; }
	}

	public interface ISqlProvider
	{
		//Extracting information from database
		string TableInformation { get; }
		string ColumnName { get; }
		string ColumnType { get; }
		string Nullable { get; }
		string Extra { get; }
		string IsAutoIncrement { get; }
		string PrimaryKey { get; }

		string NotNullValue { get; }
		string DropColumn { get; }
		string DropTable { get; }
		string AlterTable { get; }

		string Quote(string sql);

		string AddForeignKeyConstraint(string tableName, string fkName, string foreignTableName, string foreignKeyName,
			string onDeleteOption, string onUpdateOption);

		string DropForeignKey(string tableName, string fkConstraintName);
		string GetForeignKeyConstraintName(string tableName, string columnName);

		//Test purposes
		string CreateSmallTestClass { get; }
		string CreateBigTestClass { get; }
		string CreateTestClassWithFk { get; }
		string CreateTestClassWithOutFk { get; }
		string CreateTestClassWithNotNullInt { get; }
		string CreateTestClassFromIntToDouble { get; }
		string CreateTestClassWithNullDouble { get; }
		string CreateTestClassWithoutAutoIncrementId { get; }
		string DropTestClasses { get; }
		string CheckIfColumnExists(string tableName, string columnName);
	}
}