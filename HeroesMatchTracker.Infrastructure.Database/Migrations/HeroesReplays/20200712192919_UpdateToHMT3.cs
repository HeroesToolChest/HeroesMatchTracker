using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HeroesMatchTracker.Infrastructure.Database.Migrations.HeroesReplays
{
    public partial class UpdateToHMT3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
                throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "ReplayHotsApiUploads");

            migrationBuilder.CreateTable(
                name: "ReplayPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattleTagName = table.Column<string>(maxLength: 50, nullable: true),
                    BattleTagId = table.Column<string>(maxLength: 50, nullable: true),
                    AccountLevel = table.Column<int>(nullable: false),
                    LastSeen = table.Column<DateTime>(nullable: false),
                    LastSeenBefore = table.Column<DateTime>(nullable: true),
                    Seen = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayPlayers", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "ReplayPlayerToons",
                columns: table => new
                {
                    PlayerId = table.Column<long>(nullable: false),
                    Region = table.Column<int>(nullable: false),
                    ProgramId = table.Column<string>(maxLength: 50, nullable: true),
                    Realm = table.Column<int>(nullable: false),
                    Id = table.Column<long>(nullable: false),
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

            migrationBuilder.Sql(@"INSERT INTO ReplayPlayers
                SELECT PlayerId, BattleTagName, BattleNetTID, AccountLevel, LastSeen, LastSeenBefore, Seen, Notes FROM ReplayAllHotsPlayers
                WHERE EXISTS(SELECT 1 FROM ReplayAllHotsPlayers);

                INSERT INTO ReplayPlayerToons
                SELECT PlayerId, BattleNetRegionId, '1869768008', BattleNetSubId, BattleNetId FROM ReplayAllHotsPlayers
                WHERE EXISTS(SELECT 1 FROM ReplayAllHotsPlayers);");

            migrationBuilder.RenameTable("Replays", null, "Replays_old", null);
            migrationBuilder.RenameTable("ReplayMatchAwards", null, "ReplayMatchAwards_old", null);
            migrationBuilder.RenameTable("ReplayMatchPlayers", null, "ReplayMatchPlayers_old", null);
            migrationBuilder.RenameTable("ReplayMatchPlayerScoreResults", null, "ReplayMatchPlayerScoreResults_old", null);
            migrationBuilder.RenameTable("ReplayMatchPlayerTalents", null, "ReplayMatchPlayerTalents_old", null);
            migrationBuilder.RenameTable("ReplayRenamedPlayers", null, "ReplayRenamedPlayers_old", null);
            migrationBuilder.RenameTable("ReplayMatchTeamBans", null, "ReplayMatchTeamBans_old", null);
            migrationBuilder.RenameTable("ReplayMatchTeamExperiences", null, "ReplayMatchTeamExperiences_old", null);
            migrationBuilder.RenameTable("ReplayMatchTeamLevels", null, "ReplayMatchTeamLevels_old", null);

            migrationBuilder.DropTable(
                name: "ReplayMatchMessages");
            migrationBuilder.DropTable(
                name: "ReplayMatchDraftPicks");

            migrationBuilder.CreateTable(
                name: "Replays",
                columns: table => new
                {
                    ReplayId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomValue = table.Column<long>(nullable: true),
                    Hash = table.Column<string>(nullable: false),
                    MapName = table.Column<string>(maxLength: 50, nullable: false),
                    MapId = table.Column<string>(maxLength: 50, nullable: true),
                    ReplayVersion = table.Column<string>(maxLength: 20, nullable: false),
                    ReplayLengthTicks = table.Column<long>(nullable: false),
                    GameMode = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 260, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.ReplayId);
                });

            migrationBuilder.Sql(@"UPDATE Replays_old
                SET ReplayVersion = substr((SELECT ReplayVersion || '.' || ReplayBuild), 3)
                WHERE ReplayVersion LIKE '1.%'");

            migrationBuilder.Sql(@"INSERT INTO Replays
                SELECT ReplayId, RandomValue, Hash, MapName, NULL, ReplayVersion, ReplayLengthTicks, GameMode, TimeStamp, FileName FROM Replays_old 
                WHERE EXISTS(SELECT 1 FROM Replays_old);");

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayers",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    PlayerNumber = table.Column<int>(nullable: false),
                    HeroName = table.Column<string>(maxLength: 50, nullable: false),
                    HeroId = table.Column<string>(maxLength: 50, nullable: false),
                    HeroLevel = table.Column<int>(nullable: false),
                    AccountLevel = table.Column<int>(nullable: false),
                    PartyValue = table.Column<long>(nullable: true),
                    PartySize = table.Column<int>(nullable: true),
                    Difficulty = table.Column<string>(maxLength: 25, nullable: false),
                    IsAutoSelect = table.Column<bool>(nullable: false),
                    IsSilenced = table.Column<bool>(nullable: false),
                    IsVoiceSilenced = table.Column<bool>(nullable: false),
                    IsWinner = table.Column<bool>(nullable: false),
                    IsBlizzardStaff = table.Column<bool>(nullable: false),
                    HasActiveBoost = table.Column<bool>(nullable: false),
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

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchPlayers
                SELECT MatchPlayerId, ReplayId, PlayerId, Team, PlayerNumber, Character, '', CharacterLevel, AccountLevel, PartyValue, PartySize, Difficulty, IsAutoSelect, IsSilenced, IsVoiceSilenced, IsWinner, IsBlizzardStaff, HasActiveBoost FROM ReplayMatchPlayers_old 
                WHERE EXISTS(SELECT 1 FROM ReplayMatchPlayers_old);

                UPDATE ReplayMatchPlayers 
                SET PartyValue = NULL
                WHERE PartyValue = 0;

                UPDATE ReplayMatchPlayers
                SET PartySize = NULL
                WHERE PartySize = 0;");

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerLoadouts",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(nullable: false),
                    SkinAndSkinTintId = table.Column<string>(maxLength: 50, nullable: true),
                    MountAndMountTintId = table.Column<string>(maxLength: 50, nullable: true),
                    BannerId = table.Column<string>(maxLength: 50, nullable: true),
                    SprayId = table.Column<string>(maxLength: 50, nullable: true),
                    AnnouncerPackId = table.Column<string>(maxLength: 50, nullable: true),
                    VoiceLineId = table.Column<string>(maxLength: 50, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerLoadouts", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerLoadouts_ReplayMatchPlayers_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "ReplayMatchPlayers",
                        principalColumn: "MatchPlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchPlayerLoadouts
                SELECT MatchPlayerId, MountAndMountTint, SkinAndSkinTint, NULL, NULL, NULL, NULL FROM ReplayMatchPlayers_old 
                WHERE EXISTS(SELECT 1 FROM ReplayMatchPlayers_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchAwards",
                columns: table => new
                {
                    MatchAwardId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    AwardId = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchAwards", x => x.MatchAwardId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchAwards_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchAwards_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchAwards
                SELECT MatchAwardId, ReplayId, PlayerId, Award FROM ReplayMatchAwards_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchAwards_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerScoreResults",
                columns: table => new
                {
                    MatchPlayerScoreResultId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    SoloKills = table.Column<int>(nullable: true),
                    TakeDowns = table.Column<int>(nullable: true),
                    Assists = table.Column<int>(nullable: true),
                    Deaths = table.Column<int>(nullable: true),
                    SiegeDamage = table.Column<int>(nullable: true),
                    CreepDamage = table.Column<int>(nullable: true),
                    MinionDamage = table.Column<int>(nullable: true),
                    SummonDamage = table.Column<int>(nullable: true),
                    StructureDamage = table.Column<int>(nullable: true),
                    HeroDamage = table.Column<int>(nullable: true),
                    DamageTaken = table.Column<int>(nullable: true),
                    DamageSoaked = table.Column<int>(nullable: true),
                    Healing = table.Column<int>(nullable: true),
                    SelfHealing = table.Column<int>(nullable: true),
                    ExperienceContribution = table.Column<int>(nullable: true),
                    MetaExperience = table.Column<int>(nullable: true),
                    MercCampCaptures = table.Column<int>(nullable: true),
                    TownKills = table.Column<int>(nullable: true),
                    WatchTowerCaptures = table.Column<int>(nullable: true),
                    TimeSpentDeadTicks = table.Column<long>(nullable: true),
                    SpellDamage = table.Column<int>(nullable: true),
                    PhysicalDamage = table.Column<int>(nullable: true),
                    OnFireTimeonFireTicks = table.Column<long>(nullable: true),
                    MinionKills = table.Column<int>(nullable: true),
                    RegenGlobes = table.Column<int>(nullable: true),
                    HighestKillStreak = table.Column<int>(nullable: true),
                    ProtectionGivenToAllies = table.Column<int>(nullable: true),
                    TimeCCdEnemyHeroesTicks = table.Column<long>(nullable: true),
                    TimeRootingEnemyHeroesTicks = table.Column<long>(nullable: true),
                    TimeStunningEnemyHeroesTicks = table.Column<long>(nullable: true),
                    ClutchHealsPerformed = table.Column<int>(nullable: true),
                    EscapesPerformed = table.Column<int>(nullable: true),
                    VengeancesPerformed = table.Column<int>(nullable: true),
                    OutnumberedDeaths = table.Column<int>(nullable: true),
                    TeamfightEscapesPerformed = table.Column<int>(nullable: true),
                    TeamfightHealingDone = table.Column<int>(nullable: true),
                    TeamfightDamageTaken = table.Column<int>(nullable: true),
                    TeamfightHeroDamage = table.Column<int>(nullable: true),
                    Multikill = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerScoreResults", x => x.MatchPlayerScoreResultId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerScoreResults_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerScoreResults_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchPlayerScoreResults 
                SELECT MatchPlayerScoreResultId, ReplayId, PlayerId, SoloKills, TakeDowns, Assists, Deaths, SiegeDamage, CreepDamage, MinionDamage, SummonDamage, StructureDamage, HeroDamage,
                DamageTaken, NULL, Healing, SelfHealing, ExperienceContribution, MetaExperience, MercCampCaptures, TownKills, WatchTowerCaptures, TimeSpentDeadTicks,
                SpellDamage, PhysicalDamage, NULL, NULL, NULL, NULL, NULL, TimeCCdEnemyHeroes, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL FROM ReplayMatchPlayerScoreResults_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchPlayerScoreResults_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerTalents",
                columns: table => new
                {
                    MatchPlayerTalentId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    TalentId1 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected1Ticks = table.Column<long>(nullable: true),
                    TalentId4 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected4Ticks = table.Column<long>(nullable: true),
                    TalentId7 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected7Ticks = table.Column<long>(nullable: true),
                    TalentId10 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected10Ticks = table.Column<long>(nullable: true),
                    TalentId13 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected13Ticks = table.Column<long>(nullable: true),
                    TalentId16 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected16Ticks = table.Column<long>(nullable: true),
                    TalentId20 = table.Column<string>(maxLength: 75, nullable: true),
                    TimeSpanSelected20Ticks = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerTalents", x => x.MatchPlayerTalentId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerTalents_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerTalents_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchPlayerTalents 
                SELECT MatchPlayerTalentId, ReplayId, PlayerId,
                TalentName1, TimeSpanSelected1Ticks, 
                TalentName4, TimeSpanSelected4Ticks,
                TalentName7, TimeSpanSelected7Ticks,
                TalentName10, TimeSpanSelected10Ticks, 
                TalentName13, TimeSpanSelected13Ticks,
                TalentName16, TimeSpanSelected16Ticks, 
                TalentName20, TimeSpanSelected20Ticks
                FROM ReplayMatchPlayerTalents_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchPlayerTalents_old)");

            migrationBuilder.CreateTable(
                name: "ReplayRenamedPlayers",
                columns: table => new
                {
                    RenamedPlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<long>(nullable: false),
                    BattleTagName = table.Column<string>(maxLength: 50, nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayRenamedPlayers", x => x.RenamedPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayRenamedPlayers_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayRenamedPlayers 
                SELECT RenamedPlayerId, PlayerId, BattleTagName, DateAdded FROM ReplayRenamedPlayers_old
                WHERE EXISTS(SELECT 1 FROM ReplayRenamedPlayers_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchMessages",
                columns: table => new
                {
                    MessageId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    MessageEventType = table.Column<int>(nullable: false),
                    TimeStampTicks = table.Column<long>(nullable: false),
                    MessageTarget = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchMessages_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchMessages_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchDraftPicks",
                columns: table => new
                {
                    DraftPickId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    HeroSelected = table.Column<string>(nullable: false),
                    PickType = table.Column<int>(nullable: false),
                    Team = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchDraftPicks", x => x.DraftPickId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchDraftPicks_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchDraftPicks_ReplayPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchTeamBans",
                columns: table => new
                {
                    ReplayId = table.Column<long>(nullable: false),
                    Team0Ban0 = table.Column<string>(maxLength: 50, nullable: true),
                    Team0Ban1 = table.Column<string>(maxLength: 50, nullable: true),
                    Team0Ban2 = table.Column<string>(maxLength: 50, nullable: true),
                    Team1Ban0 = table.Column<string>(maxLength: 50, nullable: true),
                    Team1Ban1 = table.Column<string>(maxLength: 50, nullable: true),
                    Team1Ban2 = table.Column<string>(maxLength: 50, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchTeamBans", x => x.ReplayId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchTeamBans_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchTeamBans 
                SELECT * FROM ReplayMatchTeamBans_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchTeamBans_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchTeamExperiences",
                columns: table => new
                {
                    MatchTeamExperienceId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    TimeTicks = table.Column<long>(nullable: true),
                    Team0CreepXP = table.Column<int>(nullable: true),
                    Team0HeroXP = table.Column<int>(nullable: true),
                    Team0MinionXP = table.Column<int>(nullable: true),
                    Team0StructureXP = table.Column<int>(nullable: true),
                    Team0TeamLevel = table.Column<int>(nullable: true),
                    Team0TrickleXP = table.Column<int>(nullable: true),
                    Team1CreepXP = table.Column<int>(nullable: true),
                    Team1HeroXP = table.Column<int>(nullable: true),
                    Team1MinionXP = table.Column<int>(nullable: true),
                    Team1StructureXP = table.Column<int>(nullable: true),
                    Team1TeamLevel = table.Column<int>(nullable: true),
                    Team1TrickleXP = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchTeamExperiences", x => x.MatchTeamExperienceId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchTeamExperiences_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchTeamExperiences 
                SELECT * FROM ReplayMatchTeamExperiences_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchTeamExperiences_old)");

            migrationBuilder.CreateTable(
                name: "ReplayMatchTeamLevels",
                columns: table => new
                {
                    MatchTeamLevelId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    Team0Level = table.Column<int>(nullable: true),
                    TeamTime0Ticks = table.Column<long>(nullable: true),
                    Team1Level = table.Column<int>(nullable: true),
                    TeamTime1Ticks = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchTeamLevels", x => x.MatchTeamLevelId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchTeamLevels_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"INSERT INTO ReplayMatchTeamLevels 
                SELECT * FROM ReplayMatchTeamLevels_old
                WHERE EXISTS(SELECT 1 FROM ReplayMatchTeamLevels_old)");

            migrationBuilder.RenameColumn(
                name: "Team1TrickleXP",
                table: "ReplayMatchTeamExperiences",
                newName: "Team1PassiveXP");

            migrationBuilder.RenameColumn(
                name: "Team0TrickleXP",
                table: "ReplayMatchTeamExperiences",
                newName: "Team0PassiveXP");

            migrationBuilder.DropTable(
                name: "ReplayMatchAwards_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchPlayers_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerScoreResults_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerTalents_old");
            migrationBuilder.DropTable(
                name: "ReplayRenamedPlayers_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchTeamBans_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchTeamExperiences_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchTeamLevels_old");
            migrationBuilder.DropTable(
                name: "ReplayMatchTeamObjectives");
            migrationBuilder.DropTable(
                name: "ReplayAllHotsPlayers");

            migrationBuilder.DropTable(
                name: "Replays_old");

            migrationBuilder.Sql(@"DROP TABLE IF EXISTS SchemaInfoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException();
        }
    }
}
