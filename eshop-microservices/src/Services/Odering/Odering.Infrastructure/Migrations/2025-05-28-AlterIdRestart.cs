using FluentMigrator;

namespace Odering.Infrastructure.Migrations;

[Migration(20250528000000)]
public class AlterIdRestart: Migration
{
    public override void Up()
    {
        // Alter the Id column in the Customer table to restart from 1000
        Execute.Sql("ALTER TABLE public.\"Customer\" ALTER COLUMN \"Id\" RESTART WITH 1000;");

        // Alter the Id column in the Products table to restart from 100
        Execute.Sql("ALTER TABLE public.\"Products\" ALTER COLUMN \"Id\" RESTART WITH 1000;");


        // Alter the Id column in the Orders table to restart from 100
        Execute.Sql("ALTER TABLE public.\"Orders\" ALTER COLUMN \"Id\" RESTART WITH 1000;");
    }
    public override void Down()
    {
        // No rollback logic needed for this migration
    }
}
