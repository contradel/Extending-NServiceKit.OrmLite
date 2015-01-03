namespace OrmLitePehjExtensions
{
	public class MySqlProvider : ISqlProvider
	{
		public string SpecialQuotes
		{
			get { return "'"; }
		}

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

		public string Extra
		{
			get { return "EXTRA"; }
		}

		public string IsAutoIncrement
		{
			get { return "auto_increment"; }
		}

		public string PrimaryKey
		{
			get { return "PRIMARY KEY"; }
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

			var foreignTableWithoutQuotes = new SqlModifier().RemoveFirstAndLastCharacter(foreignTableName);

			var l1 = AlterTable + " " + tableName + " ";
			var l2 = "ADD FOREIGN KEY fk_" + foreignTableWithoutQuotes + "(" + fkName + ") ";
			var l3 = "REFERENCES " + foreignTableName + "(" + foreignKeyName + ") ";
			var l4 = "ON DELETE " + onDeleteOption + " ";
			var l5 = "ON UPDATE " + onUpdateOption;

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
			get { return "CREATE TABLE `ToNullableTest` (`Id` int(11) PRIMARY KEY AUTO_INCREMENT, `Test` int(11) NOT NULL );"; }
		}

		public string CreateTestClassFromIntToDouble
		{
			get
			{
				return
					"CREATE TABLE `FromIntToDoubleTest` (`Id` int(11) PRIMARY KEY AUTO_INCREMENT, `IntToDouble` int(11) NOT NULL );";
			}
		}

		public string CreateTestClassWithNullDouble
		{
			get { return "CREATE TABLE `ToNonNullableTest` (`Id` int(11) PRIMARY KEY AUTO_INCREMENT, `NonNullable` DOUBLE NULL );"; }
		}

		public string CreateTestClassWithoutAutoIncrementId
		{
			get
			{
				return "CREATE TABLE `IdWithoutAutoIncrement` (`Id` int(11) PRIMARY KEY, `AutoIncrementTest` VARCHAR(255) NULL);";
			}
		}

		public string DropTestClasses
		{
			get
			{
				return
					"DROP TABLE `IdWithoutAutoIncrement`; DROP TABLE `ToNonNullableTest`; DROP TABLE `FromIntToDoubleTest`; DROP TABLE `ToNullableTest`; DROP TABLE `TestClassWithFk`; DROP TABLE `TestClassWithoutFk`; DROP TABLE `BigTestClass`; DROP TABLE `SmallTestClass`";
			}
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
	}
}