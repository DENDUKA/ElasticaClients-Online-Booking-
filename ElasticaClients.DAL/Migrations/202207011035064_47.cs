namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "TelegramId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "TelegramId");
        }
    }
}
