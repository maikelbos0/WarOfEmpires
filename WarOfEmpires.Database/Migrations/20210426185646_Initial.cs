using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Alliances");

            migrationBuilder.EnsureSchema(
                name: "Attacks");

            migrationBuilder.EnsureSchema(
                name: "Empires");

            migrationBuilder.EnsureSchema(
                name: "Markets");

            migrationBuilder.EnsureSchema(
                name: "Auditing");

            migrationBuilder.EnsureSchema(
                name: "Players");

            migrationBuilder.EnsureSchema(
                name: "Events");

            migrationBuilder.EnsureSchema(
                name: "Siege");

            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.CreateTable(
                name: "AttackResults",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttackTypes",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingTypes",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandExecutions",
                schema: "Auditing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommandType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CommandData = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ElapsedMilliseconds = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandExecutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MerchandiseTypes",
                schema: "Markets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchandiseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonAggressionPacts",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonAggressionPacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueryExecutions",
                schema: "Auditing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueryType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    QueryData = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ElapsedMilliseconds = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryExecutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiegeWeaponTypes",
                schema: "Siege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiegeWeaponTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskExecutionModes",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskExecutionModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TroopTypes",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TroopTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEventTypes",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatus",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTypes",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTasks",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Interval = table.Column<TimeSpan>(type: "time", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPaused = table.Column<bool>(type: "bit", nullable: false),
                    ExecutionMode = table.Column<int>(type: "int", nullable: false),
                    LastExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledTasks_TaskExecutionModes_ExecutionMode",
                        column: x => x.ExecutionMode,
                        principalSchema: "Events",
                        principalTable: "TaskExecutionModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserStatus_Id = table.Column<byte>(type: "tinyint", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ActivationCode = table.Column<int>(type: "int", nullable: true),
                    Password_Salt = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: true),
                    Password_Hash = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: true),
                    Password_HashIterations = table.Column<int>(type: "int", nullable: true),
                    PasswordResetToken_ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken_Salt = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: true),
                    PasswordResetToken_Hash = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: true),
                    PasswordResetToken_HashIterations = table.Column<int>(type: "int", nullable: true),
                    NewEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NewEmailConfirmationCode = table.Column<int>(type: "int", nullable: true),
                    LastOnline = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserStatus_UserStatus_Id",
                        column: x => x.UserStatus_Id,
                        principalSchema: "Security",
                        principalTable: "UserStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvents",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEventType_Id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEvents_UserEventTypes_UserEventType_Id",
                        column: x => x.UserEventType_Id,
                        principalSchema: "Security",
                        principalTable: "UserEventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllianceNonAggressionPacts",
                schema: "Alliances",
                columns: table => new
                {
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    NonAggressionPactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllianceNonAggressionPacts", x => new { x.AllianceId, x.NonAggressionPactId });
                    table.ForeignKey(
                        name: "FK_AllianceNonAggressionPacts_NonAggressionPacts_NonAggressionPactId",
                        column: x => x.NonAggressionPactId,
                        principalSchema: "Alliances",
                        principalTable: "NonAggressionPacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    AllianceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Body = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonAggressionPactRequests",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonAggressionPactRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CurrentRecruitingEffort = table.Column<int>(type: "int", nullable: false),
                    Peasants = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<long>(type: "bigint", nullable: true),
                    Food = table.Column<long>(type: "bigint", nullable: true),
                    Wood = table.Column<long>(type: "bigint", nullable: true),
                    Stone = table.Column<long>(type: "bigint", nullable: true),
                    Ore = table.Column<long>(type: "bigint", nullable: true),
                    BankedGold = table.Column<long>(type: "bigint", nullable: true),
                    BankedFood = table.Column<long>(type: "bigint", nullable: true),
                    BankedWood = table.Column<long>(type: "bigint", nullable: true),
                    BankedStone = table.Column<long>(type: "bigint", nullable: true),
                    BankedOre = table.Column<long>(type: "bigint", nullable: true),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    AttackTurns = table.Column<int>(type: "int", nullable: false),
                    BankTurns = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    HasUpkeepRunOut = table.Column<bool>(type: "bit", nullable: false),
                    HasNewMarketSales = table.Column<bool>(type: "bit", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<int>(type: "int", nullable: false),
                    AllianceId = table.Column<int>(type: "int", nullable: true),
                    AllianceRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alliances",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaderId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alliances_Players_LeaderId",
                        column: x => x.LeaderId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attacks",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    AttackerId = table.Column<int>(type: "int", nullable: false),
                    DefenderId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Turns = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<long>(type: "bigint", nullable: true),
                    Food = table.Column<long>(type: "bigint", nullable: true),
                    Wood = table.Column<long>(type: "bigint", nullable: true),
                    Stone = table.Column<long>(type: "bigint", nullable: true),
                    Ore = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AttackType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attacks_AttackResults_Result",
                        column: x => x.Result,
                        principalSchema: "Attacks",
                        principalTable: "AttackResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attacks_AttackTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Attacks",
                        principalTable: "AttackTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attacks_Players_AttackerId",
                        column: x => x.AttackerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attacks_Players_DefenderId",
                        column: x => x.DefenderId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_BuildingTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Empires",
                        principalTable: "BuildingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buildings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caravans",
                schema: "Markets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caravans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caravans_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Body = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Players_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Players_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SiegeWeapons",
                schema: "Siege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiegeWeapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiegeWeapons_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiegeWeapons_SiegeWeaponTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Siege",
                        principalTable: "SiegeWeaponTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Markets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PlayerId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_MerchandiseTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Markets",
                        principalTable: "MerchandiseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Players_PlayerId1",
                        column: x => x.PlayerId1,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Troops",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Soldiers = table.Column<int>(type: "int", nullable: false),
                    Mercenaries = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Troops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Troops_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Troops_TroopTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Attacks",
                        principalTable: "TroopTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workers_WorkerTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Empires",
                        principalTable: "WorkerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanInvite = table.Column<bool>(type: "bit", nullable: false),
                    CanManageRoles = table.Column<bool>(type: "bit", nullable: false),
                    CanDeleteChatMessages = table.Column<bool>(type: "bit", nullable: false),
                    CanKickMembers = table.Column<bool>(type: "bit", nullable: false),
                    CanManageNonAggressionPacts = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "Alliances",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttackRounds",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TroopType = table.Column<int>(type: "int", nullable: false),
                    IsAggressor = table.Column<bool>(type: "bit", nullable: false),
                    Troops = table.Column<int>(type: "int", nullable: false),
                    Damage = table.Column<long>(type: "bigint", nullable: false),
                    AttackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttackRounds_Attacks_AttackId",
                        column: x => x.AttackId,
                        principalSchema: "Attacks",
                        principalTable: "Attacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttackRounds_TroopTypes_TroopType",
                        column: x => x.TroopType,
                        principalSchema: "Attacks",
                        principalTable: "TroopTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Merchandise",
                schema: "Markets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CaravanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchandise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merchandise_Caravans_CaravanId",
                        column: x => x.CaravanId,
                        principalSchema: "Markets",
                        principalTable: "Caravans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Merchandise_MerchandiseTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Markets",
                        principalTable: "MerchandiseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Casualties",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TroopType = table.Column<int>(type: "int", nullable: false),
                    Soldiers = table.Column<int>(type: "int", nullable: false),
                    Mercenaries = table.Column<int>(type: "int", nullable: false),
                    AttackRoundId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casualties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Casualties_AttackRounds_AttackRoundId",
                        column: x => x.AttackRoundId,
                        principalSchema: "Attacks",
                        principalTable: "AttackRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllianceNonAggressionPacts_NonAggressionPactId",
                schema: "Alliances",
                table: "AllianceNonAggressionPacts",
                column: "NonAggressionPactId");

            migrationBuilder.CreateIndex(
                name: "IX_Alliances_LeaderId",
                schema: "Alliances",
                table: "Alliances",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AttackRounds_AttackId",
                schema: "Attacks",
                table: "AttackRounds",
                column: "AttackId");

            migrationBuilder.CreateIndex(
                name: "IX_AttackRounds_TroopType",
                schema: "Attacks",
                table: "AttackRounds",
                column: "TroopType");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_AttackerId",
                schema: "Attacks",
                table: "Attacks",
                column: "AttackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_DefenderId",
                schema: "Attacks",
                table: "Attacks",
                column: "DefenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_Result",
                schema: "Attacks",
                table: "Attacks",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_Type",
                schema: "Attacks",
                table: "Attacks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_PlayerId",
                schema: "Empires",
                table: "Buildings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Type",
                schema: "Empires",
                table: "Buildings",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Caravans_PlayerId",
                schema: "Markets",
                table: "Caravans",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Casualties_AttackRoundId",
                schema: "Attacks",
                table: "Casualties",
                column: "AttackRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_AllianceId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_AllianceId",
                schema: "Alliances",
                table: "Invites",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_PlayerId",
                schema: "Alliances",
                table: "Invites",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchandise_CaravanId",
                schema: "Markets",
                table: "Merchandise",
                column: "CaravanId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchandise_Type",
                schema: "Markets",
                table: "Merchandise",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                schema: "Players",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                schema: "Players",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_NonAggressionPactRequests_RecipientId",
                schema: "Alliances",
                table: "NonAggressionPactRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_NonAggressionPactRequests_SenderId",
                schema: "Alliances",
                table: "NonAggressionPactRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AllianceId",
                schema: "Players",
                table: "Players",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AllianceRoleId",
                schema: "Players",
                table: "Players",
                column: "AllianceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                schema: "Players",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AllianceId",
                schema: "Alliances",
                table: "Roles",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_ExecutionMode",
                schema: "Events",
                table: "ScheduledTasks",
                column: "ExecutionMode");

            migrationBuilder.CreateIndex(
                name: "IX_SiegeWeapons_PlayerId",
                schema: "Siege",
                table: "SiegeWeapons",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SiegeWeapons_Type",
                schema: "Siege",
                table: "SiegeWeapons",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PlayerId",
                schema: "Markets",
                table: "Transactions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PlayerId1",
                schema: "Markets",
                table: "Transactions",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Type",
                schema: "Markets",
                table: "Transactions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Troops_PlayerId",
                schema: "Attacks",
                table: "Troops",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Troops_Type",
                schema: "Attacks",
                table: "Troops",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserEventType_Id",
                schema: "Security",
                table: "UserEvents",
                column: "UserEventType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserId",
                schema: "Security",
                table: "UserEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Security",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserStatus_Id",
                schema: "Security",
                table: "Users",
                column: "UserStatus_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_PlayerId",
                schema: "Empires",
                table: "Workers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Type",
                schema: "Empires",
                table: "Workers",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceNonAggressionPacts_Alliances_AllianceId",
                schema: "Alliances",
                table: "AllianceNonAggressionPacts",
                column: "AllianceId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Alliances_AllianceId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "AllianceId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Players_PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "PlayerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Alliances_AllianceId",
                schema: "Alliances",
                table: "Invites",
                column: "AllianceId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Players_PlayerId",
                schema: "Alliances",
                table: "Invites",
                column: "PlayerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NonAggressionPactRequests_Alliances_RecipientId",
                schema: "Alliances",
                table: "NonAggressionPactRequests",
                column: "RecipientId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NonAggressionPactRequests_Alliances_SenderId",
                schema: "Alliances",
                table: "NonAggressionPactRequests",
                column: "SenderId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Alliances_AllianceId",
                schema: "Players",
                table: "Players",
                column: "AllianceId",
                principalSchema: "Alliances",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Roles_AllianceRoleId",
                schema: "Players",
                table: "Players",
                column: "AllianceRoleId",
                principalSchema: "Alliances",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Alliances_AllianceId",
                schema: "Players",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Alliances_AllianceId",
                schema: "Alliances",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "AllianceNonAggressionPacts",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "Buildings",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "Casualties",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "CommandExecutions",
                schema: "Auditing");

            migrationBuilder.DropTable(
                name: "Invites",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "Merchandise",
                schema: "Markets");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Players");

            migrationBuilder.DropTable(
                name: "NonAggressionPactRequests",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "QueryExecutions",
                schema: "Auditing");

            migrationBuilder.DropTable(
                name: "ScheduledTasks",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "SiegeWeapons",
                schema: "Siege");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Markets");

            migrationBuilder.DropTable(
                name: "Troops",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "UserEvents",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Workers",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "NonAggressionPacts",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "BuildingTypes",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "AttackRounds",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "Caravans",
                schema: "Markets");

            migrationBuilder.DropTable(
                name: "TaskExecutionModes",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "SiegeWeaponTypes",
                schema: "Siege");

            migrationBuilder.DropTable(
                name: "MerchandiseTypes",
                schema: "Markets");

            migrationBuilder.DropTable(
                name: "UserEventTypes",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "WorkerTypes",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "Attacks",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "TroopTypes",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "AttackResults",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "AttackTypes",
                schema: "Attacks");

            migrationBuilder.DropTable(
                name: "Alliances",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "Players",
                schema: "Players");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserStatus",
                schema: "Security");
        }
    }
}
