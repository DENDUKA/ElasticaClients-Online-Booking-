namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incomes", "GymId", "dbo.Gyms");
            DropForeignKey("dbo.Subscriptions", "GymId", "dbo.Gyms");
            DropIndex("dbo.Subscriptions", new[] { "GymId" });
            DropIndex("dbo.Incomes", new[] { "GymId" });
            CreateTable(
                "dbo.Branches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Subscriptions", "BranchId", c => c.Int(nullable: false));
            AddColumn("dbo.Incomes", "BranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "BranchId");
            CreateIndex("dbo.Incomes", "BranchId");
            AddForeignKey("dbo.Incomes", "BranchId", "dbo.Branches", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Subscriptions", "BranchId", "dbo.Branches", "Id", cascadeDelete: true);
            DropColumn("dbo.Subscriptions", "GymId");
            DropColumn("dbo.Incomes", "GymId");
        }

        public override void Down()
        {
            AddColumn("dbo.Incomes", "GymId", c => c.Int(nullable: false));
            AddColumn("dbo.Subscriptions", "GymId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Subscriptions", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Incomes", "BranchId", "dbo.Branches");
            DropIndex("dbo.Incomes", new[] { "BranchId" });
            DropIndex("dbo.Subscriptions", new[] { "BranchId" });
            DropColumn("dbo.Incomes", "BranchId");
            DropColumn("dbo.Subscriptions", "BranchId");
            DropTable("dbo.Branches");
            CreateIndex("dbo.Incomes", "GymId");
            CreateIndex("dbo.Subscriptions", "GymId");
            AddForeignKey("dbo.Subscriptions", "GymId", "dbo.Gyms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Incomes", "GymId", "dbo.Gyms", "Id", cascadeDelete: true);
        }
    }
}
