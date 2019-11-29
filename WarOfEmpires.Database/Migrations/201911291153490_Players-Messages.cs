namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Players.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Body = c.String(),
                        Recipient_Id = c.Int(nullable: false),
                        Sender_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("Players.Players", t => t.Sender_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Players.Messages", "Sender_Id", "Players.Players");
            DropForeignKey("Players.Messages", "Recipient_Id", "Players.Players");
            DropIndex("Players.Messages", new[] { "Sender_Id" });
            DropIndex("Players.Messages", new[] { "Recipient_Id" });
            DropTable("Players.Messages");
        }
    }
}
