namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _46 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "BonusesOff", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Accounts", "BonusesOff");
        }
    }
}
