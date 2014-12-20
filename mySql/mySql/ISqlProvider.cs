namespace mySql
{
	public interface ISqlProvider
	{
		string TableInformation { get; }
		string TableColumn { get; }
		string DropColumn { get; }
		string DropTable { get; }
		string AlterTable { get; }
	}

	internal class MsSqlProvider : ISqlProvider
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
	}
}