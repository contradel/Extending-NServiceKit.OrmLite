namespace OrmLitePehjExtensions
{
	public class MySqlProvider : ISqlProvider
	{
		public string TableInformation
		{
			get { return "DESCRIBE"; }
		}

		public string ColumnName
		{
			get { return "FIELD"; }
		}

		public string ColumnType
		{
			get { return "TYPE"; }
		}

		public string Nullable
		{
			get { return "NULL"; }
		}

		public string NotNullValue
		{
			get { return "NO"; }
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

		public string Quote(string sql)
		{
			return "'" + sql + "'";
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

		public string DropForeignKey(string tableName, string fkConstraintName)
		{
			return AlterTable + " " + tableName + " DROP FOREIGN KEY " + fkConstraintName;
		}

		public string GetForeignKeyConstraintName(string tableName, string columnName)
		{
			return "SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE WHERE TABLE_NAME = " +
			       tableName + " AND COLUMN_NAME = " + columnName;
		}

		public string CreateSmallTestClass
		{
			get
			{
				return
					"CREATE TABLE `BigTestClass` (`Id` int(11) PRIMARY KEY AUTO_INCREMENT, `Time` DATETIME NOT NULL, `Name` VARCHAR(255) NULL, `Amount` decimal(38,6) NOT NULL);";
			}
		}

		public string CreateBigTestClass
		{
			get
			{
				return
					"CREATE TABLE `SmallTestClass` ( `Id` int(11) PRIMARY KEY AUTO_INCREMENT,  `Time` DATETIME NOT NULL,  `Name` VARCHAR(255) NULL,  `Amount` decimal(38,6) NOT NULL,  `Precision` DOUBLE NOT NULL,  `TargetDate` DATETIME NOT NULL );";
			}
		}

		public string CreateTestClassWithFk
		{
			get
			{
				return
					"CREATE TABLE `TestClassWithFk` ( `Id` int(11) PRIMARY KEY AUTO_INCREMENT,  `SmallTestClassId` int(11) NOT NULL,  CONSTRAINT `FK_TestClassWithFk_SmallTestClass_SmallTestClassId` FOREIGN KEY (`SmallTestClassId`) REFERENCES `SmallTestClass` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION );";
			}
		}

		public string CreateTestClassWithOutFk
		{
			get { return "CREATE TABLE `TestClassWithoutFk` ( `Id` int(11) PRIMARY KEY AUTO_INCREMENT );"; }
		}

		public string CreateTestClassWithNotNullInt
		{
			get { return "CREATE TABLE `TestWithInteger` (`Id` int(11) PRIMARY KEY AUTO_INCREMENT, `Test` int(11) NOT NULL );"; }
		}

		public string DropTestClasses()
		{
			return "DROP TABLE `TestWithInteger`; DROP TABLE `TestClassWithFk`; DROP TABLE `TestClassWithoutFk`; DROP TABLE `BigTestClass`; DROP TABLE `SmallTestClass`";
		}

		public string CheckIfColumnExists(string tableName, string columnName)
		{
//SELECT * 
//FROM information_schema.COLUMNS 
//WHERE TABLE_NAME = 'table_name' 
//AND COLUMN_NAME = 'column_name'

			return "SELECT * FROM information_schema.COLUMNS WHERE TABLE_NAME = " + tableName +
			       " AND COLUMN_NAME = " +
			       columnName;
		}

		public string SpecialQuotes
		{
			get { return "'"; }
		}

		//public class MsSqlProvider : ISqlProvider
		//{
		//	public string TableInformation
		//	{
		//		get { return "EXEC SP_COLUMNS"; }
		//	}

		//	public string TableColumn
		//	{
		//		get { return "COLUMN_NAME"; }
		//	}

		//	public string DropColumn
		//	{
		//		get { return "DROP COLUMN"; }
		//	}

		//	public string DropTable
		//	{
		//		get { return "DROP TABLE"; }
		//	}

		//	public string AlterTable
		//	{
		//		get { return "ALTER TABLE"; }
		//	}

		//	public string AddForeignKeyConstraint(string tableName, string fkName, string foreignTableName, string foreignKeyName,
		//		string onDeleteOption, string onUpdateOption)
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string DropForeignKey(string tableName, string fkConstraintName)
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string GetForeignKeyConstraintName(string tableName, string columnName)
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string CreateSmallTestClass()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string CreateBigTestClass()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string CreateTestClassWithFk()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string DropTestClasses()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string CheckIfColumnExists(string tableName, string columnName)
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public string SpecialQuotes
		//	{
		//		get { return "'"; }
		//	}
		//}
	}
}