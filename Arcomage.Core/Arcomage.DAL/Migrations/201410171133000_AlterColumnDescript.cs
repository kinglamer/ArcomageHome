namespace Arcomage.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnDescript : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cards", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cards", "description");
        }
    }
}
