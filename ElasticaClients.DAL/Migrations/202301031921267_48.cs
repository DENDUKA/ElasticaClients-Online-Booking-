namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "ServisesList", c => c.String());
            DropColumn("dbo.TrainingItems", "Armpits");
            DropColumn("dbo.TrainingItems", "LegsFull");
            DropColumn("dbo.TrainingItems", "Shin");
            DropColumn("dbo.TrainingItems", "DeepBikini");
            DropColumn("dbo.TrainingItems", "HandsFull");
            DropColumn("dbo.TrainingItems", "ClassicBikini");
            DropColumn("dbo.TrainingItems", "Hips");
            DropColumn("dbo.TrainingItems", "Areol");
            DropColumn("dbo.TrainingItems", "Face");
            DropColumn("dbo.TrainingItems", "Lip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrainingItems", "Lip", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Face", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Areol", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Hips", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "ClassicBikini", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "HandsFull", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "DeepBikini", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Shin", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "LegsFull", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Armpits", c => c.Boolean(nullable: false));
            DropColumn("dbo.TrainingItems", "ServisesList");
        }
    }
}
