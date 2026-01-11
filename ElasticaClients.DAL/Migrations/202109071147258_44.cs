namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _44 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "Type", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Incomes", "Type");
        }
    }
}
