namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "Razovoe", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "RazovoeCost", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainingItems", "RazovoeCost");
            DropColumn("dbo.TrainingItems", "Razovoe");
        }
    }
}
