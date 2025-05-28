using FluentMigrator;

namespace Odering.Infrastructure.Migrations;

[Migration(20250527000000)]
public class InitialCreate : Migration
{
    public override void Up()
    {
        Create.Table("Customer") // Fixed the issue by removing `Schema` call  
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique();
        //.WithColumn("CreatedAt").AsDateTime().WithDefaultValue(null)  
        //.WithColumn("CreatedBy").AsString(100).WithDefaultValue(null)  
        //.WithColumn("LastModifiedDate").AsDateTime().WithDefaultValue(null)  
        //.WithColumn("LastModifiedBy").AsString(100).WithDefaultValue(null);  

        // Products table  
        Create.Table("Products")
            .WithColumn("Id").AsInt64().Identity().PrimaryKey()
            //.WithColumn("CreatedAt").AsDateTime().Nullable()  
            //.WithColumn("CreatedBy").AsString().Nullable()  
            //.WithColumn("LastModified").AsDateTime().Nullable()  
            //.WithColumn("LastModifiedBy").AsString().Nullable()  
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable();

        // Orders table  
        Create.Table("Orders")
            .WithColumn("Id").AsInt64().Identity().PrimaryKey()
            //.WithColumn("CreatedAt").AsDateTime().Nullable()  
            //.WithColumn("CreatedBy").AsString().Nullable()  
            //.WithColumn("LastModified").AsDateTime().Nullable()  
            //.WithColumn("LastModifiedBy").AsString().Nullable()  
            .WithColumn("CustomerId").AsInt64().NotNullable()
            .WithColumn("Status").AsString().WithDefaultValue("Draft").NotNullable()
            .WithColumn("TotalPrice").AsDecimal(18, 2).NotNullable()
            // BillingAddress  
            .WithColumn("BillingAddress_AddressLine").AsString(180).NotNullable()
            .WithColumn("BillingAddress_Country").AsString(50).NotNullable()
            .WithColumn("BillingAddress_EmailAddress").AsString(50).Nullable()
            .WithColumn("BillingAddress_FirstName").AsString(50).NotNullable()
            .WithColumn("BillingAddress_LastName").AsString(50).NotNullable()
            .WithColumn("BillingAddress_State").AsString(50).NotNullable()
            .WithColumn("BillingAddress_ZipCode").AsString(5).NotNullable()
            // OrderName  
            .WithColumn("OrderName").AsString(100).NotNullable()
            // Payment  
            .WithColumn("Payment_CVV").AsString(3).NotNullable()
            .WithColumn("Payment_CardName").AsString(50).Nullable()
            .WithColumn("Payment_CardNumber").AsString(24).NotNullable()
            .WithColumn("Payment_Expiration").AsString(10).NotNullable()
            .WithColumn("Payment_PaymentMethod").AsInt32().NotNullable()
            // ShippingAddress  
            .WithColumn("ShippingAddress_AddressLine").AsString(180).NotNullable()
            .WithColumn("ShippingAddress_Country").AsString(50).NotNullable()
            .WithColumn("ShippingAddress_EmailAddress").AsString(50).Nullable()
            .WithColumn("ShippingAddress_FirstName").AsString(50).NotNullable()
            .WithColumn("ShippingAddress_LastName").AsString(50).NotNullable()
            .WithColumn("ShippingAddress_State").AsString(50).NotNullable()
            .WithColumn("ShippingAddress_ZipCode").AsString(5).NotNullable();

        Create.Index("IX_Orders_CustomerId").OnTable("Orders").OnColumn("CustomerId");

        // OrderItems table  
        Create.Table("OrderItems")
            .WithColumn("Id").AsInt64().Identity().PrimaryKey()
            //.WithColumn("CreatedAt").AsDateTime().Nullable()  
            //.WithColumn("CreatedBy").AsString().Nullable()  
            //.WithColumn("LastModified").AsDateTime().Nullable()  
            //.WithColumn("LastModifiedBy").AsString().Nullable()  
            .WithColumn("OrderId").AsInt64().NotNullable()
            .WithColumn("ProductId").AsInt64().NotNullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable();

        Create.Index("IX_OrderItems_OrderId").OnTable("OrderItems").OnColumn("OrderId");
        Create.Index("IX_OrderItems_ProductId").OnTable("OrderItems").OnColumn("ProductId");

        // Foreign keys  
        Create.ForeignKey("FK_Orders_Customers_CustomerId")
            .FromTable("Orders").ForeignColumn("CustomerId")
            .ToTable("Customer").PrimaryColumn("Id");

        Create.ForeignKey("FK_OrderItems_Orders_OrderId")
            .FromTable("OrderItems").ForeignColumn("OrderId")
            .ToTable("Orders").PrimaryColumn("Id");

        Create.ForeignKey("FK_OrderItems_Products_ProductId")
            .FromTable("OrderItems").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id");

    }

    public override void Down()
    {
        // Drop all table  
        Delete.Table("OrderItems");
        Delete.Table("Orders");
        Delete.Table("Products");
        Delete.Table("Customer");
    }
}
