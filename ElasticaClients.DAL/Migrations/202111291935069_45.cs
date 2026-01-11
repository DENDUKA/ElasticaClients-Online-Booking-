namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Bonuses", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Accounts", "Bonuses");
        }
    }
}
