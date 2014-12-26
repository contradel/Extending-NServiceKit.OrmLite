using System;
using NServiceKit.DataAnnotations;

namespace OrmLitePehjExtensions
{
	public class TestWithInteger
	{
		[AutoIncrement]
		public int Id { get; set; }
		public int Test { get; set; }
	}

	public interface ISqlProvider
	{
		string TableInformation { get; }
		string ColumnName { get; }
		string ColumnType { get; }
		string Nullable { get; }
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

		string DropTestClasses();

		string CheckIfColumnExists(string tableName, string columnName);
	}
}