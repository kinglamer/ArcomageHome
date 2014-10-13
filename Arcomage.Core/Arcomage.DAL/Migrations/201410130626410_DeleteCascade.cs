namespace Arcomage.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCascade : DbMigration
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
                        Card_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Cards", t => t.Card_id, cascadeDelete: true)
                .Index(t => t.Card_id);
            
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
