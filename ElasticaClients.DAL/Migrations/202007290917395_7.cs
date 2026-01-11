namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrainingItems", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.TrainingItems", new[] { "Account_Id" });
            RenameColumn(table: "dbo.TrainingItems", name: "Account_Id", newName: "AccountId");
            AlterColumn("dbo.TrainingItems", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.TrainingItems", "AccountId");
            AddForeignKey("dbo.TrainingItems", "AccountId", "dbo.Accounts", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.TrainingItems", "AccountId", "dbo.Accounts");
            DropIndex("dbo.TrainingItems", new[] { "AccountId" });
            AlterColumn("dbo.TrainingItems", "AccountId", c => c.Int());
            RenameColumn(table: "dbo.TrainingItems", name: "AccountId", newName: "Account_Id");
            CreateIndex("dbo.TrainingItems", "Account_Id");
            AddForeignKey("dbo.TrainingItems", "Account_Id", "dbo.Accounts", "Id");
        }
    }
}
