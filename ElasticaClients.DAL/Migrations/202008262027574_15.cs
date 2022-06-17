namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Int(nullable: false),
                        GymId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gyms", t => t.GymId, cascadeDelete: true)
                .Index(t => t.GymId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incomes", "GymId", "dbo.Gyms");
            DropIndex("dbo.Incomes", new[] { "GymId" });
            DropTable("dbo.Incomes");
        }
    }
}
