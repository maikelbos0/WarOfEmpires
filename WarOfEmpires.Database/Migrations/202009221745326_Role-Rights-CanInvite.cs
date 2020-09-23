﻿namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleRightsCanInvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Roles", "CanInvite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Alliances.Roles", "CanInvite");
        }
    }
}
