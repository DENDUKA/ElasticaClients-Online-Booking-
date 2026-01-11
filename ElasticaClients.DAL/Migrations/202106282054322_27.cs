namespace ElasticaClients.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "SettingsBranchId", c => c.Int(nullable: false, defaultValueSql: "0"));
        }

        public override void Down()
        {
            DropColumn("dbo.Accounts", "SettingsBranchId");
        }
    }
}
