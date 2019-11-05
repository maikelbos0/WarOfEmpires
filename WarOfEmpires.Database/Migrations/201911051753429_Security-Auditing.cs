namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecurityAuditing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Auditing.CommandExecutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CommandType = c.String(nullable: false, maxLength: 255),
                        CommandData = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Auditing.QueryExecutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        QueryType = c.String(nullable: false, maxLength: 255),
                        QueryData = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Security.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserStatus_Id = c.Byte(nullable: false),
                        Email = c.String(nullable: false, maxLength: 255),
                        ActivationCode = c.Int(),
                        Password_Salt = c.Binary(nullable: false, maxLength: 20),
                        Password_Hash = c.Binary(nullable: false, maxLength: 20),
                        Password_HashIterations = c.Int(nullable: false),
                        PasswordResetToken_ExpiryDate = c.DateTime(),
                        PasswordResetToken_Salt = c.Binary(maxLength: 20),
                        PasswordResetToken_Hash = c.Binary(maxLength: 20),
                        PasswordResetToken_HashIterations = c.Int(),
                        NewEmail = c.String(maxLength: 255),
                        NewEmailConfirmationCode = c.Int(),
                        DisplayName = c.String(maxLength: 50),
                        Description = c.String(),
                        ShowEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.UserStatus", t => t.UserStatus_Id, cascadeDelete: true)
                .Index(t => t.UserStatus_Id)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "Security.UserEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        UserEventType_Id = c.Byte(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("Security.UserEventTypes", t => t.UserEventType_Id, cascadeDelete: true)
                .Index(t => t.UserEventType_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "Security.UserEventTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Security.UserStatus",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.Users", "UserStatus_Id", "Security.UserStatus");
            DropForeignKey("Security.UserEvents", "UserEventType_Id", "Security.UserEventTypes");
            DropForeignKey("Security.UserEvents", "User_Id", "Security.Users");
            DropIndex("Security.UserEvents", new[] { "User_Id" });
            DropIndex("Security.UserEvents", new[] { "UserEventType_Id" });
            DropIndex("Security.Users", new[] { "Email" });
            DropIndex("Security.Users", new[] { "UserStatus_Id" });
            DropTable("Security.UserStatus");
            DropTable("Security.UserEventTypes");
            DropTable("Security.UserEvents");
            DropTable("Security.Users");
            DropTable("Auditing.QueryExecutions");
            DropTable("Auditing.CommandExecutions");
        }
    }
}
