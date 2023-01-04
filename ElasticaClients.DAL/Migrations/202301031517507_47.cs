namespace ElasticaClients.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingItems", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.TrainingItems", "Armpits", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "LegsFull", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Shin", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "DeepBikini", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "HandsFull", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "ClassicBikini", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Hips", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Areol", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Face", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainingItems", "Lip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainingItems", "Lip");
            DropColumn("dbo.TrainingItems", "Face");
            DropColumn("dbo.TrainingItems", "Areol");
            DropColumn("dbo.TrainingItems", "Hips");
            DropColumn("dbo.TrainingItems", "ClassicBikini");
            DropColumn("dbo.TrainingItems", "HandsFull");
            DropColumn("dbo.TrainingItems", "DeepBikini");
            DropColumn("dbo.TrainingItems", "Shin");
            DropColumn("dbo.TrainingItems", "LegsFull");
            DropColumn("dbo.TrainingItems", "Armpits");
            DropColumn("dbo.TrainingItems", "Discount");
        }
    }
}
