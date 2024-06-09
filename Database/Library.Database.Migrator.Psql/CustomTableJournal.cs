using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Postgresql;
using DbUp.Support;

namespace Library.Database.Migrator;

public class CustomTableJournal : TableJournal
{
    internal CustomTableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string tableName)
        : base(connectionManager, logger, new PostgresqlObjectParser(), schema, tableName)
    {
    }

    protected override string GetInsertJournalEntrySql(string scriptName, string applied)
    {
        return "insert into " + FqSchemaTableName + " (ScriptName, Applied) values (" + scriptName + ", " + applied + ")";
    }

    protected override string GetJournalEntriesSql()
    {
        return "select ScriptName from " + FqSchemaTableName + " order by ScriptName";
    }

    protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
    {
        return "CREATE SCHEMA IF NOT EXISTS " + SchemaTableSchema + "; " +
               "CREATE TABLE " + FqSchemaTableName + "\n(\nschemaversionsid serial NOT NULL,\nscriptname character varying(255) NOT NULL,\napplied timestamp without time zone NOT NULL,\nCONSTRAINT " + quotedPrimaryKeyName + " PRIMARY KEY (schemaversionsid)\n)";
    }
}