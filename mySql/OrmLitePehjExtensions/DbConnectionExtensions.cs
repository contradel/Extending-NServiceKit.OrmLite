using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NServiceKit.OrmLite;

namespace OrmLitePehjExtensions
{
	namespace System.Data
	{
		public class DatabaseColumnInfo
		{
			public string Name { get; set; }
			public string SqlType { get; set; }
			public bool Nullable { get; set; }
		}

		//Modified code from http://stackoverflow.com/questions/14142433/with-ormlite-is-there-a-way-to-automatically-update-table-schema-when-my-poco-i
		public static class DbConnectionExtensions
		{
			//Gets a list of strings of the columns that exists in the table on the database with tableName
			private static List<DatabaseColumnInfo> GetColumnInformation(this IDbConnection db, string tableName, ISqlProvider sqlProvider)
			{
				var columns = new List<DatabaseColumnInfo>();
				using (IDbCommand cmd = db.CreateCommand())
				{
					//create sql that gets table information
					cmd.CommandText = sqlProvider.TableInformation + " " + db.GetDialectProvider().GetQuotedName(tableName);
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						var tempColumn = new DatabaseColumnInfo
						{
							Name = reader[sqlProvider.ColumnName].ToString(),
							SqlType = reader[sqlProvider.ColumnType].ToString(),
							Nullable = reader[sqlProvider.Nullable].ToString() == sqlProvider.NotNullValue ? false : true
						};

						columns.Add(tempColumn);
					}
					reader.Close();
				}
				return columns;
			}

			//Updates the table on the database to reflect the model
			public static void UpdateTable<T>(this IDbConnection db, ISqlProvider sqlProvider, bool deleteColumns)
				where T : new()
			{
				//Use orm-lite to get a nice definition of our model, this is gold, this is the reason why this code is easy
				ModelDefinition model = ModelDefinition<T>.Definition;

				//Create the table if it doesn't already exist
				if (!db.TableExists(model.ModelName))
				{
					db.CreateTable<T>(false);
					Console.WriteLine(db.GetLastSql());
					return;
				}

				//		ADD FIELDS THAT EXISTS IN MODEL BUT NOT ON DB
				//Find each of the missing fields on the database
				var dbColumns = GetColumnInformation(db, model.ModelName, sqlProvider);
				var missingOnDb = new List<FieldDefinition>();

				foreach (FieldDefinition fieldDefinition in model.FieldDefinitions)
				{
					var found = dbColumns.Any(databaseColumnInfo => databaseColumnInfo.Name == fieldDefinition.FieldName);
					if (!found)
						missingOnDb.Add(fieldDefinition);
				}


				//Add a new column for each missing field
				foreach (FieldDefinition field in missingOnDb)
				{
					//Add the column
					string addSql = db.GetDialectProvider().ToAddColumnStatement(typeof (T), field);
					Console.WriteLine(addSql);
					db.ExecuteSql(addSql);

					//Check if it's foreign key
					if (field.ForeignKey == null)
						continue;

					Console.WriteLine("Found foreign key constraint");

					//Build the foreign key constraint
					string onDeleteFkSql = field.ForeignKey.OnDelete;
					string onUpdateFkSql = field.ForeignKey.OnUpdate;
					string foreignKeyConstraintName = db.GetDialectProvider().GetQuotedColumnName(field.ForeignKey.ForeignKeyName);

					string tableName = db.GetDialectProvider().GetQuotedTableName(model);
					string fkName = db.GetDialectProvider().GetQuotedColumnName(field.FieldName);
					string foreignTableName = db.GetDialectProvider().GetQuotedTableName(field.ForeignKey.ReferenceType.Name);
					string foreignKeyName = db.GetDialectProvider().GetQuotedColumnName("Id");	//we have to assume we want to reference Id

					string addFkConstraintSql = sqlProvider.AddForeignKeyConstraint(tableName, fkName, foreignTableName, foreignKeyName,
						onDeleteFkSql, onUpdateFkSql);

					Console.WriteLine(addFkConstraintSql);
					db.ExecuteSql(addFkConstraintSql);

					//Tried to use ToAddForeignKeyStatement, couldn't get it to work!
				}

				//If safety bool is false, return
				if (!deleteColumns)
					return;

				//		DELETE FIELDS THAT EXISTS ON DB BUT NOT IN MODEL
				List<string> modelFields = model.FieldDefinitionsArray.Select(x => x.FieldName).ToList();
				List<DatabaseColumnInfo> extraOnDb = dbColumns.Where(x => !modelFields.Contains(x.Name)).ToList();

				foreach (DatabaseColumnInfo columnInfo in extraOnDb)
				{
					var extraName = columnInfo.Name;
					string tableNameQ = db.GetDialectProvider().GetQuotedTableName(model);
					string deleteSql = sqlProvider.AlterTable + " " + tableNameQ + " " + sqlProvider.DropColumn + " " + db.GetDialectProvider().GetQuotedColumnName(extraName);
					Console.WriteLine(deleteSql);
					try
					{
						db.ExecuteSql(deleteSql);
					}
					catch (Exception e)
					{
						Console.WriteLine("Could not drop column.");
						Console.WriteLine("Trying to find and delete foreign key constraint, then drop.");
						try
						{
							//Table name with special quotes for special DB command
							string tableName = sqlProvider.SpecialQuotes + model.Name + sqlProvider.SpecialQuotes;
							string columnName = sqlProvider.SpecialQuotes + extraName + sqlProvider.SpecialQuotes;

							//Find foreign key constraint name
							string constraintNameSql = sqlProvider.GetForeignKeyConstraintName(tableName, columnName);
							string constraintName = db.SqlList<string>(constraintNameSql)[0];

							//Create SQL that drops the foreign key constraint
							string dropFkSql = sqlProvider.DropForeignKey(tableNameQ, constraintName);

							Console.WriteLine(dropFkSql);
							Console.WriteLine(deleteSql);

							db.ExecuteSql(dropFkSql);
							db.ExecuteSql(deleteSql);
						}
						catch (Exception err)
						{
							Console.WriteLine(err);
							throw new Exception("Could not delete the column");
						}
					}
				}
			}
		}
	}
}