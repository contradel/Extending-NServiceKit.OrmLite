using System;

namespace OrmLitePehjExtensions
{
	public interface ISqlProvider
	{
		string TableInformation { get; }
		string TableColumn { get; }
		string DropColumn { get; }
		string DropTable { get; }
		string AlterTable { get; }
		string SpecialQuotes { get; }

		string AddForeignKeyConstraint(string tableName, string fkName, string foreignTableName, string foreignKeyName,
			string onDeleteOption, string onUpdateOption);

		string DropForeignKey(string tableName, string fkKeyName);
		string GetForeignKeyConstraintName(string tableName, string columnName);
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

		public string AddForeignKeyConstraint(string tableName, string fkName, string foreignTableName, string foreignKeyName,
			string onDeleteOption, string onUpdateOption)
		{
			//ALTER TABLE products
			//ADD FOREIGN KEY fk_vendor(vdr_id)
			//REFERENCES vendors(vdr_id)
			//ON DELETE NO ACTION
			//ON UPDATE CASCADE;

			string foreignTableWithoutQuotes = new SqlModifier().RemoveFirstAndLastCharacter(foreignTableName);

			string l1 = AlterTable + " " + tableName + " ";
			string l2 = "ADD FOREIGN KEY fk_" + foreignTableWithoutQuotes + "(" + fkName + ") ";
			string l3 = "REFERENCES " + foreignTableName + "(" + foreignKeyName + ") ";
			string l4 = "ON DELETE " + onDeleteOption + " ";
			string l5 = "ON UPDATE " + onUpdateOption;

			return l1 + l2 + l3 + l4 + l5;
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

			public string AddForeignKeyConstraint(string tableName, string fkName, string foreignTableName, string foreignKeyName,
				string onDeleteOption, string onUpdateOption)
			{
				throw new NotImplementedException();
			}

			public string DropForeignKey(string tableName, string fkKeyName)
			{
				throw new NotImplementedException();
			}

			public string GetForeignKeyConstraintName(string tableName, string columnName)
			{
				throw new NotImplementedException();
			}

			public string SpecialQuotes
			{
				get { return "'"; }
			}
		}
	}
}