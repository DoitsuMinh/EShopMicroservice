using FluentMigrator;

namespace Odering.Infrastructure.Migrations;

[Migration(20250529000000)]
public class AlterCreateAndModifiedColumns : Migration
{
    public override void Up()
    {
        Alter.Table("Orders")
            .AddColumn("CreatedAt").AsDateTime().Nullable()
            .AddColumn("CreatedBy").AsString(100).Nullable()
            .AddColumn("LastModifiedDate").AsDateTime().Nullable()
            .AddColumn("LastModifiedBy").AsString(100).Nullable();

        Alter.Table("Products")
            .AddColumn("CreatedAt").AsDateTime().Nullable()
            .AddColumn("CreatedBy").AsString(100).Nullable()
            .AddColumn("LastModified").AsDateTime().Nullable()
            .AddColumn("LastModifiedBy").AsString(100).Nullable();

        Alter.Table("Customer")
            .AddColumn("CreatedAt").AsDateTime().Nullable()
            .AddColumn("CreatedBy").AsString(100).Nullable()
            .AddColumn("LastModifiedDate").AsDateTime().Nullable()
            .AddColumn("LastModifiedBy").AsString(100).Nullable();

        Alter.Table("OrderItems")
            .AddColumn("CreatedAt").AsDateTime().Nullable()
            .AddColumn("CreatedBy").AsString(100).Nullable()
            .AddColumn("LastModified").AsDateTime().Nullable()
            .AddColumn("LastModifiedBy").AsString(100).Nullable();
    }

    public override void Down()
    {
        Delete.Column("CreatedAt").FromTable("Orders").InSchema("public");
        Delete.Column("CreateBy").FromTable("Orders").InSchema("public");
        Delete.Column("LastModifiedDate").FromTable("Orders").InSchema("public");
        Delete.Column("LastModifiedBy").FromTable("Orders").InSchema("public");
        Delete.Column("CreatedAt").FromTable("Products").InSchema("public");
        Delete.Column("CreatedBy").FromTable("Products").InSchema("public");
        Delete.Column("LastModified").FromTable("Products").InSchema("public");
        Delete.Column("LastModifiedBy").FromTable("Products").InSchema("public");
        Delete.Column("CreatedAt").FromTable("Customer").InSchema("public");
        Delete.Column("CreatedBy").FromTable("Customer").InSchema("public");
        Delete.Column("LastModifiedDate").FromTable("Customer").InSchema("public");
        Delete.Column("LastModifiedBy").FromTable("Customer").InSchema("public");
        Delete.Column("CreatedAt").FromTable("OrderItems").InSchema("public");
        Delete.Column("CreatedBy").FromTable("OrderItems").InSchema("public");
        Delete.Column("LastModified").FromTable("OrderItems").InSchema("public");
        Delete.Column("LastModifiedBy").FromTable("OrderItems").InSchema("public");
    }
}
