namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gyms", "BranchId", c => c.Int(nullable: false, defaultValueSql: "1"));
            CreateIndex("dbo.Gyms", "BranchId");
            AddForeignKey("dbo.Gyms", "BranchId", "dbo.Branches", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Gyms", "BranchId", "dbo.Branches");
            DropIndex("dbo.Gyms", new[] { "BranchId" });
            DropColumn("dbo.Gyms", "BranchId");
        }
    }
}
