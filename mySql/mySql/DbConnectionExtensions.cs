using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NServiceKit;
using NServiceKit.OrmLite;

namespace mySql
{
	namespace System.Data
	{
		//Modified code from http://stackoverflow.com/questions/14142433/with-ormlite-is-there-a-way-to-automatically-update-table-schema-when-my-poco-i
		public static class DbConnectionExtensions
		{
			//Gets a list of strings of the columns that exists in the table on the database with tableName
			private static List<string> GetColumnNames(this IDbConnection db, string tableName, ISqlProvider sqlProvider)
			{
				var columns = new List<string>();
				using (IDbCommand cmd = db.CreateCommand())
				{
					//create sql that gets table information
					cmd.CommandText = sqlProvider.TableInformation + " " + db.GetDialectProvider().GetQuotedName(tableName);
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						//ordinal is the index of the field which contains the column name
						int ordinal = reader.GetOrdinal(sqlProvider.TableColumn);
						columns.Add(reader.GetString(ordinal));
					}
					reader.Close();
				}
				return columns;
			}

			//Updates the table on the database to reflect the model
			public static void UpdateTable<T>(this IDbConnection db, ISqlProvider sqlProvider, bool deleteColumns) where T : new()
			{
				//Use orm-lite to get a nice definition of our model, this is gold, this is the reason why this code is easy
				ModelDefinition model = ModelDefinition<T>.Definition;

				//Create the table if it doesn't already exist
				if (db.TableExists(model.ModelName) == false)
				{
					db.CreateTable<T>(false);
					Console.WriteLine(db.GetLastSql());
					return;
				}

				//		ADD FIELDS THAT EXISTS IN MODEL BUT NOT ON DB
				//Find each of the missing fields on the database
				List<string> dbColumns = GetColumnNames(db, model.ModelName, sqlProvider);
				List<FieldDefinition> missingOnDb = model.FieldDefinitions
					.Where(field => !dbColumns.Contains(field.FieldName))
					.ToList();

				//Add a new column for each missing field
				foreach (FieldDefinition field in missingOnDb)
				{
					//if (field != ForeignKey)
					var addSql = db.GetDialectProvider().ToAddColumnStatement(typeof (T), field);
					//else
					//addSql = db.GetDialectProvider().ToAddForeignKeyStatement(??,??,??,??,??);

					Console.WriteLine(addSql);
					db.ExecuteSql(addSql);
				}

				//If safety bool is false, return
				if (!deleteColumns)
					return;

				//		DELETE FIELDS THAT EXISTS ON DB BUT NOT IN MODEL
				List<string> modelFields = model.FieldDefinitionsArray.Select(x => x.FieldName).ToList();
				var extraOnDb = dbColumns.Where(x => !modelFields.Contains(x)).ToList();

				foreach (var extra in extraOnDb)
				{
					var deleteSql = sqlProvider.AlterTable + " " + db.GetDialectProvider().GetQuotedTableName(model) + " " +
					                sqlProvider.DropColumn +
					                " " + db.GetDialectProvider().GetQuotedColumnName(extra);
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
							var tableNameQ = db.GetDialectProvider().GetQuotedTableName(model);
							//Table name with special quotes for special DB command
							var tableName = sqlProvider.SpecialQuotes + model.Name + sqlProvider.SpecialQuotes;
							var columnName = sqlProvider.SpecialQuotes + extra + sqlProvider.SpecialQuotes;

							//Find foreign key constraint name
							var constraintNameSql = sqlProvider.GetForeignKeyConstraintName(tableName, columnName);
							var constraintName = db.SqlList<string>(constraintNameSql)[0];

							//Create SQL that drops the foreign key constraint
							var dropFkSql = sqlProvider.DropForeignKey(tableNameQ, constraintName);

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