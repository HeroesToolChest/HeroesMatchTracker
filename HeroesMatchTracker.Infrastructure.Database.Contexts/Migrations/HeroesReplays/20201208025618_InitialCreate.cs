using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeroesMatchTracker.Infrastructure.Database.Contexts.Migrations.HeroesReplays
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
                name: "ReplayOldPlayerInfos",
                columns: table => new
                {
                    ReplayOldPlayerInfoId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    BattleTagName = table.Column<string>(type: "TEXT", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayOldPlayerInfos", x => x.ReplayOldPlayerInfoId);
                    table.ForeignKey(
                        name: "FK_ReplayOldPlayerInfos_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Replays",
                columns: table => new
                {
                    ReplayId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerPlayerId = table.Column<long>(type: "INTEGER", nullable: true),
                    RandomValue = table.Column<long>(type: "INTEGER", nullable: true),
                    Hash = table.Column<string>(type: "TEXT", nullable: false),
                    MapName = table.Column<string>(type: "TEXT", nullable: false),
                    MapId = table.Column<string>(type: "TEXT", nullable: true),
                    ReplayVersion = table.Column<string>(type: "TEXT", nullable: false),
                    ReplayLengthTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    GameMode = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HasAI = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasObservers = table.Column<bool>(type: "INTEGER", nullable: false),
                    Region = table.Column<int>(type: "INTEGER", nullable: false),
                    WinningTeam = table.Column<int>(type: "INTEGER", nullable: false),
                    ReplayFilePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_Replays_ReplayPlayers_OwnerPlayerId",
                        column: x => x.OwnerPlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayers",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    Team = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerType = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroName = table.Column<string>(type: "TEXT", nullable: true),
                    HeroId = table.Column<string>(type: "TEXT", nullable: true),
                    HeroLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    HeroUnitId = table.Column<string>(type: "TEXT", nullable: true),
                    HeroAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    AccountLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    PartyValue = table.Column<long>(type: "INTEGER", nullable: true),
                    PartySize = table.Column<int>(type: "INTEGER", nullable: true),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: true),
                    IsAutoSelect = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsSilenced = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsVoiceSilenced = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWinner = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsBlizzardStaff = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasActiveBoost = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayers", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayers_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerLoadout",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    SkinAndSkinTintId = table.Column<string>(type: "TEXT", nullable: true),
                    SkinAndSkinTintAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    MountAndMountTintId = table.Column<string>(type: "TEXT", nullable: true),
                    MountAndMountTintAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    BannerId = table.Column<string>(type: "TEXT", nullable: true),
                    BannerAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    SprayId = table.Column<string>(type: "TEXT", nullable: true),
                    SprayAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    AnnouncerPackId = table.Column<string>(type: "TEXT", nullable: true),
                    AnnouncerPackAttributeId = table.Column<string>(type: "TEXT", nullable: true),
                    VoiceLineId = table.Column<string>(type: "TEXT", nullable: true),
                    VoiceLineAttributeId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerLoadout", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerLoadout_ReplayMatchPlayers_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "ReplayMatchPlayers",
                        principalColumn: "MatchPlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerScoreResults",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    SoloKills = table.Column<int>(type: "INTEGER", nullable: true),
                    TakeDowns = table.Column<int>(type: "INTEGER", nullable: true),
                    Assists = table.Column<int>(type: "INTEGER", nullable: true),
                    Deaths = table.Column<int>(type: "INTEGER", nullable: true),
                    SiegeDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    CreepDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    MinionDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    SummonDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    StructureDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    HeroDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    DamageTaken = table.Column<int>(type: "INTEGER", nullable: true),
                    DamageSoaked = table.Column<int>(type: "INTEGER", nullable: true),
                    Healing = table.Column<int>(type: "INTEGER", nullable: true),
                    SelfHealing = table.Column<int>(type: "INTEGER", nullable: true),
                    ExperienceContribution = table.Column<int>(type: "INTEGER", nullable: true),
                    MetaExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    MercCampCaptures = table.Column<int>(type: "INTEGER", nullable: true),
                    TownKills = table.Column<int>(type: "INTEGER", nullable: true),
                    WatchTowerCaptures = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeSpentDeadTicks = table.Column<long>(type: "INTEGER", nullable: true),
                    SpellDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    PhysicalDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    OnFireTimeonFireTicks = table.Column<long>(type: "INTEGER", nullable: true),
                    MinionKills = table.Column<int>(type: "INTEGER", nullable: true),
                    RegenGlobes = table.Column<int>(type: "INTEGER", nullable: true),
                    HighestKillStreak = table.Column<int>(type: "INTEGER", nullable: true),
                    ProtectionGivenToAllies = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeCCdEnemyHeroesTicks = table.Column<long>(type: "INTEGER", nullable: true),
                    TimeRootingEnemyHeroesTicks = table.Column<long>(type: "INTEGER", nullable: true),
                    TimeStunningEnemyHeroesTicks = table.Column<long>(type: "INTEGER", nullable: true),
                    ClutchHealsPerformed = table.Column<int>(type: "INTEGER", nullable: true),
                    EscapesPerformed = table.Column<int>(type: "INTEGER", nullable: true),
                    VengeancesPerformed = table.Column<int>(type: "INTEGER", nullable: true),
                    OutnumberedDeaths = table.Column<int>(type: "INTEGER", nullable: true),
                    TeamfightEscapesPerformed = table.Column<int>(type: "INTEGER", nullable: true),
                    TeamfightHealingDone = table.Column<int>(type: "INTEGER", nullable: true),
                    TeamfightDamageTaken = table.Column<int>(type: "INTEGER", nullable: true),
                    TeamfightHeroDamage = table.Column<int>(type: "INTEGER", nullable: true),
                    Multikill = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerScoreResults", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerScoreResults_ReplayMatchPlayers_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "ReplayMatchPlayers",
                        principalColumn: "MatchPlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerTalents",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    TalentId1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected1Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId4 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected4Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId7 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected7Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId10 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected10Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId13 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected13Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId16 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected16Ticks = table.Column<long>(type: "INTEGER", nullable: true),
                    TalentId20 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TimeSpanSelected20Ticks = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerTalents", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerTalents_ReplayMatchPlayers_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "ReplayMatchPlayers",
                        principalColumn: "MatchPlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_PlayerId",
                table: "ReplayMatchPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_ReplayId",
                table: "ReplayMatchPlayers",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayOldPlayerInfos_PlayerId",
                table: "ReplayOldPlayerInfos",
                column: "PlayerId");

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
                name: "IX_Replays_OwnerPlayerId",
                table: "Replays",
                column: "OwnerPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerReplayUploads_ReplayId",
                table: "ServerReplayUploads",
                column: "ReplayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerLoadout");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerScoreResults");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerTalents");

            migrationBuilder.DropTable(
                name: "ReplayOldPlayerInfos");

            migrationBuilder.DropTable(
                name: "ReplayPlayerToons");

            migrationBuilder.DropTable(
                name: "ServerReplayUploads");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayers");

            migrationBuilder.DropTable(
                name: "Replays");

            migrationBuilder.DropTable(
                name: "ReplayPlayers");
        }
    }
}
