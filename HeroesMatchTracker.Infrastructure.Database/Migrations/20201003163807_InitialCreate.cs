using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeroesMatchTracker.Infrastructure.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplayPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattleTagName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ShortcutId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    AccountLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSeenBefore = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Seen = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayPlayers", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Replays",
                columns: table => new
                {
                    ReplayId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomValue = table.Column<long>(type: "INTEGER", nullable: true),
                    Hash = table.Column<string>(type: "TEXT", nullable: false),
                    MapName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MapId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ReplayVersion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ReplayLengthTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    GameMode = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 260, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.ReplayId);
                });

            migrationBuilder.CreateTable(
                name: "ReplayOldPlayerInfos",
                columns: table => new
                {
                    ReplayOldPlayerInfoId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    BattleTagName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReplayPlayerPlayerId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayOldPlayerInfos", x => x.ReplayOldPlayerInfoId);
                    table.ForeignKey(
                        name: "FK_ReplayOldPlayerInfos_ReplayPlayers_ReplayPlayerPlayerId",
                        column: x => x.ReplayPlayerPlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplayPlayerToons",
                columns: table => new
                {
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    Region = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgramId = table.Column<long>(type: "INTEGER", maxLength: 50, nullable: false),
                    Realm = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayPlayerToons", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayPlayerToons_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayers",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    BattleTagName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Team = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerType = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HeroId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HeroLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    PartyValue = table.Column<long>(type: "INTEGER", nullable: true),
                    PartySize = table.Column<int>(type: "INTEGER", nullable: true),
                    Difficulty = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    IsAutoSelect = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsSilenced = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsVoiceSilenced = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWinner = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsBlizzardStaff = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasActiveBoost = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReplayPlayerPlayerId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayers", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayers_ReplayPlayers_ReplayPlayerPlayerId",
                        column: x => x.ReplayPlayerPlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayers_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerReplayUploads",
                columns: table => new
                {
                    ServerReplayUploadId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(type: "INTEGER", nullable: false),
                    ServerName = table.Column<int>(type: "INTEGER", nullable: false),
                    UploadedTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerReplayUploads", x => x.ServerReplayUploadId);
                    table.ForeignKey(
                        name: "FK_ServerReplayUploads_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_ReplayId",
                table: "ReplayMatchPlayers",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_ReplayPlayerPlayerId",
                table: "ReplayMatchPlayers",
                column: "ReplayPlayerPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayOldPlayerInfos_ReplayPlayerPlayerId",
                table: "ReplayOldPlayerInfos",
                column: "ReplayPlayerPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayPlayerToons_Region_ProgramId_Realm_Id",
                table: "ReplayPlayerToons",
                columns: new[] { "Region", "ProgramId", "Realm", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Replays_Hash",
                table: "Replays",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerReplayUploads_ReplayId",
                table: "ServerReplayUploads",
                column: "ReplayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayMatchPlayers");

            migrationBuilder.DropTable(
                name: "ReplayOldPlayerInfos");

            migrationBuilder.DropTable(
                name: "ReplayPlayerToons");

            migrationBuilder.DropTable(
                name: "ServerReplayUploads");

            migrationBuilder.DropTable(
                name: "ReplayPlayers");

            migrationBuilder.DropTable(
                name: "Replays");
        }
    }
}
