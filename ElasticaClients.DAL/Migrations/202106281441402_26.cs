namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subscriptions", "PriceId", "dbo.Prices");
            DropIndex("dbo.Subscriptions", new[] { "PriceId" });
            DropColumn("dbo.Subscriptions", "PriceId");
            DropTable("dbo.Prices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Subscriptions", "PriceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "PriceId");
            AddForeignKey("dbo.Subscriptions", "PriceId", "dbo.Prices", "Id", cascadeDelete: true);
        }
    }
}
