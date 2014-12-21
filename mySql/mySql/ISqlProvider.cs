namespace mySql
{
	public interface ISqlProvider
	{
		string TableInformation { get; }
		string TableColumn { get; }
		string DropColumn { get; }
		string DropTable { get; }
		string AlterTable { get; }
		string AddForeignKey(string tableName, string foreignTableName, string foreignKeyName, string referencedPkName);
		string DropForeignKey(string tableName, string fkKeyName);
		string GetForeignKeyConstraintName(string tableName, string columnName);
		string SpecialQuotes { get; }
	}

	public class MySqlProvider : ISqlProvider
	{
		public string TableInformation
		{
			get { return "DESCRIBE"; }
		}

		public string TableColumn
		{
			get { return "FIELD"; }
		}

		public string DropColumn
		{
			get { return "DROP COLUMN"; }
		}

		public string DropTable
		{
			get { return "DROP TABLE"; }
		}

		public string AlterTable
		{
			get { return "ALTER TABLE"; }
		}

		public string AddForeignKey(string tableName, string foreignTableName, string foreignKeyName, string referencedPkName)
		{
			return AlterTable + " " + tableName + " " + " ADD FOREIGN KEY (" + foreignKeyName + ") REFERENCES " +
			       foreignTableName + "(" + referencedPkName + ")";
		}

		public string DropForeignKey(string tableName, string fkKeyName)
		{
			return AlterTable + " " + tableName + " DROP FOREIGN KEY " + fkKeyName;
		}

		public string GetForeignKeyConstraintName(string tableName, string columnName)
		{
			return "SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = " +
			       tableName + " AND COLUMN_NAME = " + columnName;
		}

		public string SpecialQuotes
		{
			get { return "'"; }
		}

		public class MsSqlProvider : ISqlProvider
		{
			public string TableInformation
			{
				get { return "EXEC SP_COLUMNS"; }
			}

			public string TableColumn
			{
				get { return "COLUMN_NAME"; }
			}

			public string DropColumn
			{
				get { return "DROP COLUMN"; }
			}

			public string DropTable
			{
				get { return "DROP TABLE"; }
			}

			public string AlterTable
			{
				get { return "ALTER TABLE"; }
			}

			public string AddForeignKey(string tableName, string foreignTableName, string foreignKeyName, string referencedPkName)
			{
				return new MySqlProvider().AddForeignKey(tableName, foreignTableName, foreignKeyName, referencedPkName);
			}

			public string DropForeignKey(string tableName, string fkKeyName)
			{
				throw new global::System.NotImplementedException();
			}

			public string GetForeignKeyConstraintName(string tableName, string columnName)
			{
				throw new global::System.NotImplementedException();
			}

			public string SpecialQuotes
			{
				get { return "'"; }
			}
		}
	}
}