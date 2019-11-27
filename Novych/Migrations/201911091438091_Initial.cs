namespace Novych.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Class = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ParniCistic_Reservations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateDesc = c.String(),
                        DeleteDate = c.DateTime(),
                        DeleteDesc = c.String(),
                        IpAddress = c.String(),
                        Identifier = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Citerka_Songs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        HeaderText = c.String(),
                        Notes = c.String(),
                        FooterText = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        Identifier = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visitors");
            DropTable("dbo.Citerka_Songs");
            DropTable("dbo.ParniCistic_Reservations");
            DropTable("dbo.Log");
        }
    }
}
