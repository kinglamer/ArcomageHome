namespace Arcomage.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardParams",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        key = c.Int(nullable: false),
                        value = c.Int(nullable: false),
                        card_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Cards", t => t.card_id, cascadeDelete: true)
                .Index(t => t.card_id);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardParams", "card_id", "dbo.Cards");
            DropIndex("dbo.CardParams", new[] { "card_id" });
            DropTable("dbo.Cards");
            DropTable("dbo.CardParams");
        }
    }
}
