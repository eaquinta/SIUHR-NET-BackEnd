namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessRulePermitAssignments",
                c => new
                    {
                        AccessRuleId = c.Int(nullable: false),
                        PermitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccessRuleId, t.PermitId })
                .ForeignKey("dbo.AccessRules", t => t.AccessRuleId, cascadeDelete: true)
                .Index(t => t.AccessRuleId);
            
            CreateTable(
                "dbo.AccessRules",
                c => new
                    {
                        AccessRuleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.AccessRuleId);
            
            CreateTable(
                "dbo.AccessRuleRoleAssignments",
                c => new
                    {
                        AccessRuleId = c.Int(nullable: false),
                        AppRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccessRuleId, t.AppRoleId })
                .ForeignKey("dbo.AccessRules", t => t.AccessRuleId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.AppRoleId, cascadeDelete: true)
                .Index(t => t.AccessRuleId)
                .Index(t => t.AppRoleId);
            
            CreateTable(
                "dbo.AppRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AppUserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AppAuditLogs",
                c => new
                    {
                        AuditLogID = c.Guid(nullable: false),
                        UserName = c.String(nullable: false),
                        Controller = c.String(),
                        Action = c.String(),
                        Duration = c.String(),
                        IpAddress = c.String(),
                        Client = c.String(),
                        Browser = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditLogID);
            
            CreateTable(
                "dbo.AppEntityChangeLogs",
                c => new
                    {
                        EntityChangeLogId = c.Guid(nullable: false),
                        EntityLogId = c.Guid(nullable: false),
                        ColumnName = c.String(nullable: false),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.EntityChangeLogId);
            
            CreateTable(
                "dbo.AppGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppRoleGroups",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.AppGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.Role_Id)
                .Index(t => t.GroupId)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.AppEntityLogs",
                c => new
                    {
                        EntityLogId = c.Guid(nullable: false),
                        EventType = c.String(nullable: false),
                        TableName = c.String(nullable: false),
                        RecordID = c.String(nullable: false),
                        Created_by = c.String(nullable: false),
                        Created_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EntityLogId);
            
            CreateTable(
                "dbo.AppUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AppGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AppUser", t => t.User_Id)
                .Index(t => t.GroupId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AppUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AppUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppYearlyCounters",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Year = c.Int(nullable: false),
                        LastNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Name, t.Year });
            
            CreateTable(
                "dbo.Encargados",
                c => new
                    {
                        EncargadoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        AppUserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.EncargadoId)
                .ForeignKey("dbo.AppUser", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.Facilitadors",
                c => new
                    {
                        FacilitadorId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FacilitadorId)
                .ForeignKey("dbo.Personas", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 250),
                        MiddleName = c.String(maxLength: 60),
                        ThirdName = c.String(maxLength: 60),
                        LastName = c.String(maxLength: 60),
                        MatriName = c.String(maxLength: 60),
                        MarriedName = c.String(maxLength: 60),
                        Birth = c.DateTime(),
                        Gender = c.Int(),
                        Name = c.String(),
                        Image = c.String(),
                        ImageDate = c.DateTime(),
                        EstadoCivil = c.Int(),
                        Regligion = c.Int(),
                        Etnia = c.Int(),
                        PersonIdRef = c.String(),
                        PersonIdRefOrigen = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.PacienteEventoHistorials",
                c => new
                    {
                        PacienteEventoHistoryId = c.Int(nullable: false, identity: true),
                        PacienteEventoId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Tipo = c.Int(nullable: false),
                        ServicioOrigenId = c.Int(),
                        ServicioDestinoId = c.Int(),
                        EstadoObservacion = c.String(),
                    })
                .PrimaryKey(t => t.PacienteEventoHistoryId)
                .ForeignKey("dbo.Servicios", t => t.ServicioDestinoId)
                .ForeignKey("dbo.Servicios", t => t.ServicioOrigenId)
                .ForeignKey("dbo.PacienteEventos", t => t.PacienteEventoId, cascadeDelete: true)
                .Index(t => t.PacienteEventoId)
                .Index(t => t.ServicioOrigenId)
                .Index(t => t.ServicioDestinoId);
            
            CreateTable(
                "dbo.Servicios",
                c => new
                    {
                        ServicioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 25),
                        Area = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServicioId);
            
            CreateTable(
                "dbo.PacienteEventos",
                c => new
                    {
                        PacienteEventoId = c.Int(nullable: false, identity: true),
                        Procedencia = c.String(),
                        NombrePaciente = c.String(),
                        Edad = c.Int(),
                        NombreFamiliar = c.String(),
                        FechaIngreso = c.DateTime(nullable: false),
                        FechaEgreso = c.DateTime(),
                        Cama = c.Int(),
                        Registro = c.String(),
                        RegistroMedicoId = c.String(maxLength: 20),
                        FacilitadorId = c.Int(),
                        ServicioId = c.Int(),
                        Diagnostico = c.String(),
                        Sintomas = c.String(),
                        Telefono = c.String(),
                        TieneTarjeta = c.Boolean(nullable: false),
                        Observaciones = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.PacienteEventoId)
                .ForeignKey("dbo.Facilitadors", t => t.FacilitadorId)
                .ForeignKey("dbo.RegistrosMedicos", t => t.RegistroMedicoId)
                .ForeignKey("dbo.Servicios", t => t.ServicioId)
                .Index(t => t.RegistroMedicoId)
                .Index(t => t.FacilitadorId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.RegistrosMedicos",
                c => new
                    {
                        RegistroMedicoId = c.String(nullable: false, maxLength: 20),
                        PersonId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.RegistroMedicoId)
                .ForeignKey("dbo.Personas", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.PacienteEventoTraslados",
                c => new
                    {
                        PacienteEventoTrasladoId = c.Int(nullable: false, identity: true),
                        PacienteEventoId = c.Int(nullable: false),
                        Motivo = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        Usuario = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.PacienteEventoTrasladoId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        Nit = c.String(nullable: false, maxLength: 10),
                        Descripcion = c.String(nullable: false, maxLength: 40),
                        Direccion = c.String(nullable: false, maxLength: 40),
                        Telefono = c.String(maxLength: 19),
                        Contacto = c.String(maxLength: 30),
                        DiasCredito = c.Int(),
                        Email = c.String(maxLength: 25),
                        Banco1 = c.String(maxLength: 20),
                        Cuenta1 = c.String(maxLength: 20),
                        Banco2 = c.String(maxLength: 20),
                        Cuenta2 = c.String(maxLength: 20),
                        Banco3 = c.String(maxLength: 20),
                        Cuenta3 = c.String(maxLength: 20),
                        PersonId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Nit)
                .ForeignKey("dbo.Personas", t => t.PersonId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proveedor", "PersonId", "dbo.Personas");
            DropForeignKey("dbo.PacienteEventos", "ServicioId", "dbo.Servicios");
            DropForeignKey("dbo.PacienteEventos", "RegistroMedicoId", "dbo.RegistrosMedicos");
            DropForeignKey("dbo.RegistrosMedicos", "PersonId", "dbo.Personas");
            DropForeignKey("dbo.PacienteEventoHistorials", "PacienteEventoId", "dbo.PacienteEventos");
            DropForeignKey("dbo.PacienteEventos", "FacilitadorId", "dbo.Facilitadors");
            DropForeignKey("dbo.PacienteEventoHistorials", "ServicioOrigenId", "dbo.Servicios");
            DropForeignKey("dbo.PacienteEventoHistorials", "ServicioDestinoId", "dbo.Servicios");
            DropForeignKey("dbo.Facilitadors", "PersonId", "dbo.Personas");
            DropForeignKey("dbo.Encargados", "AppUserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserGroups", "User_Id", "dbo.AppUser");
            DropForeignKey("dbo.AppUserRole", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserLogin", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserClaim", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserGroups", "GroupId", "dbo.AppGroups");
            DropForeignKey("dbo.AppRoleGroups", "Role_Id", "dbo.AppRole");
            DropForeignKey("dbo.AppRoleGroups", "GroupId", "dbo.AppGroups");
            DropForeignKey("dbo.AccessRuleRoleAssignments", "AppRoleId", "dbo.AppRole");
            DropForeignKey("dbo.AppUserRole", "RoleId", "dbo.AppRole");
            DropForeignKey("dbo.AccessRuleRoleAssignments", "AccessRuleId", "dbo.AccessRules");
            DropForeignKey("dbo.AccessRulePermitAssignments", "AccessRuleId", "dbo.AccessRules");
            DropIndex("dbo.Proveedor", new[] { "PersonId" });
            DropIndex("dbo.RegistrosMedicos", new[] { "PersonId" });
            DropIndex("dbo.PacienteEventos", new[] { "ServicioId" });
            DropIndex("dbo.PacienteEventos", new[] { "FacilitadorId" });
            DropIndex("dbo.PacienteEventos", new[] { "RegistroMedicoId" });
            DropIndex("dbo.PacienteEventoHistorials", new[] { "ServicioDestinoId" });
            DropIndex("dbo.PacienteEventoHistorials", new[] { "ServicioOrigenId" });
            DropIndex("dbo.PacienteEventoHistorials", new[] { "PacienteEventoId" });
            DropIndex("dbo.Facilitadors", new[] { "PersonId" });
            DropIndex("dbo.Encargados", new[] { "AppUserId" });
            DropIndex("dbo.AppUserLogin", new[] { "UserId" });
            DropIndex("dbo.AppUserClaim", new[] { "UserId" });
            DropIndex("dbo.AppUser", "UserNameIndex");
            DropIndex("dbo.AppUserGroups", new[] { "User_Id" });
            DropIndex("dbo.AppUserGroups", new[] { "GroupId" });
            DropIndex("dbo.AppRoleGroups", new[] { "Role_Id" });
            DropIndex("dbo.AppRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.AppUserRole", new[] { "UserId" });
            DropIndex("dbo.AppRole", "RoleNameIndex");
            DropIndex("dbo.AccessRuleRoleAssignments", new[] { "AppRoleId" });
            DropIndex("dbo.AccessRuleRoleAssignments", new[] { "AccessRuleId" });
            DropIndex("dbo.AccessRulePermitAssignments", new[] { "AccessRuleId" });
            DropTable("dbo.Proveedor");
            DropTable("dbo.PacienteEventoTraslados");
            DropTable("dbo.RegistrosMedicos");
            DropTable("dbo.PacienteEventos");
            DropTable("dbo.Servicios");
            DropTable("dbo.PacienteEventoHistorials");
            DropTable("dbo.Personas");
            DropTable("dbo.Facilitadors");
            DropTable("dbo.Encargados");
            DropTable("dbo.AppYearlyCounters");
            DropTable("dbo.AppUserLogin");
            DropTable("dbo.AppUserClaim");
            DropTable("dbo.AppUser");
            DropTable("dbo.AppUserGroups");
            DropTable("dbo.AppEntityLogs");
            DropTable("dbo.AppRoleGroups");
            DropTable("dbo.AppGroups");
            DropTable("dbo.AppEntityChangeLogs");
            DropTable("dbo.AppAuditLogs");
            DropTable("dbo.AppUserRole");
            DropTable("dbo.AppRole");
            DropTable("dbo.AccessRuleRoleAssignments");
            DropTable("dbo.AccessRules");
            DropTable("dbo.AccessRulePermitAssignments");
        }
    }
}
