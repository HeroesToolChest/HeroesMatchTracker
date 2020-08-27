using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeroesMatchTracker.Infrastructure.Database.Migrations.HeroesReplays
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
                throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.CreateTable(
                name: "ReplayAllHotsPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattleTagName = table.Column<string>(maxLength: 50, nullable: true),
                    BattleNetId = table.Column<int>(nullable: false),
                    BattleNetRegionId = table.Column<int>(nullable: false),
                    BattleNetSubId = table.Column<int>(nullable: false),
                    BattleNetTId = table.Column<string>(nullable: true),
                    AccountLevel = table.Column<int>(nullable: false),
                    LastSeen = table.Column<DateTime>(nullable: false),
                    LastSeenBefore = table.Column<DateTime>(nullable: true),
                    Seen = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayAllHotsPlayers", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Replays",
                columns: table => new
                {
                    ReplayId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomValue = table.Column<long>(nullable: true),
                    Hash = table.Column<string>(nullable: false),
                    MapName = table.Column<string>(maxLength: 50, nullable: false),
                    ReplayBuild = table.Column<int>(nullable: true),
                    ReplayVersion = table.Column<string>(maxLength: 20, nullable: false),
                    ReplayLengthTicks = table.Column<long>(nullable: false),
                    GameMode = table.Column<int>(nullable: false),
                    GameSpeed = table.Column<string>(maxLength: 50, nullable: false),
                    IsGameEventsParsed = table.Column<bool>(nullable: false),
                    Frames = table.Column<int>(nullable: true),
                    TeamSize = table.Column<string>(maxLength: 10, nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 260, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.ReplayId);
                });

            migrationBuilder.CreateTable(
                name: "ReplayRenamedPlayers",
                columns: table => new
                {
                    RenamedPlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<long>(nullable: false),
                    BattleTagName = table.Column<string>(maxLength: 50, nullable: true),
                    BattleNetId = table.Column<int>(nullable: false),
                    BattleNetRegionId = table.Column<int>(nullable: false),
                    BattleNetSubId = table.Column<int>(nullable: false),
                    BattleNetTId = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayRenamedPlayers", x => x.RenamedPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayRenamedPlayers_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayHotsApiUploads",
                columns: table => new
                {
                    ReplaysHotsApiUploadId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    ReplayFileTimeStamp = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayHotsApiUploads", x => x.ReplaysHotsApiUploadId);
                    table.ForeignKey(
                        name: "FK_ReplayHotsApiUploads_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchAwards",
                columns: table => new
                {
                    MatchAwardId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    Award = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchAwards", x => x.MatchAwardId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchAwards_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchAwards_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchDraftPicks",
                columns: table => new
                {
                    DraftPickId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerSlotId = table.Column<int>(nullable: false),
                    HeroSelected = table.Column<string>(nullable: false),
                    PickType = table.Column<int>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchMessages",
                columns: table => new
                {
                    MessageId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    MessageEventType = table.Column<string>(nullable: false),
                    TimeStampTicks = table.Column<long>(nullable: false),
                    MessageTarget = table.Column<string>(maxLength: 20, nullable: false),
                    PlayerName = table.Column<string>(maxLength: 20, nullable: false),
                    CharacterName = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayers",
                columns: table => new
                {
                    MatchPlayerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    Team = table.Column<int>(nullable: true),
                    PlayerNumber = table.Column<int>(nullable: false),
                    Character = table.Column<string>(maxLength: 50, nullable: false),
                    CharacterLevel = table.Column<int>(nullable: false),
                    AccountLevel = table.Column<int>(nullable: false),
                    PartyValue = table.Column<long>(nullable: false),
                    PartySize = table.Column<int>(nullable: false),
                    Difficulty = table.Column<string>(maxLength: 25, nullable: false),
                    Handicap = table.Column<int>(nullable: true),
                    IsAutoSelect = table.Column<bool>(nullable: false),
                    IsSilenced = table.Column<bool>(nullable: false),
                    IsVoiceSilenced = table.Column<bool>(nullable: false),
                    IsWinner = table.Column<bool>(nullable: false),
                    IsBlizzardStaff = table.Column<bool>(nullable: false),
                    HasActiveBoost = table.Column<bool>(nullable: false),
                    MountAndMountTint = table.Column<string>(maxLength: 50, nullable: false),
                    SkinAndSkinTint = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayers", x => x.MatchPlayerId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayers_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
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
                    Healing = table.Column<int>(nullable: true),
                    SelfHealing = table.Column<int>(nullable: true),
                    ExperienceContribution = table.Column<int>(nullable: true),
                    MetaExperience = table.Column<int>(nullable: true),
                    MercCampCaptures = table.Column<int>(nullable: true),
                    TownKills = table.Column<int>(nullable: true),
                    WatchTowerCaptures = table.Column<int>(nullable: true),
                    TimeCCdEnemyHeroes = table.Column<long>(nullable: true),
                    TimeSpentDeadTicks = table.Column<long>(nullable: true),
                    SpellDamage = table.Column<int>(nullable: true),
                    PhysicalDamage = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerScoreResults", x => x.MatchPlayerScoreResultId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerScoreResults_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerScoreResults_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplayMatchPlayerTalents",
                columns: table => new
                {
                    MatchPlayerTalentId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    Character = table.Column<string>(maxLength: 50, nullable: false),
                    TalentId1 = table.Column<int>(nullable: true),
                    TalentName1 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected1Ticks = table.Column<long>(nullable: true),
                    TalentId4 = table.Column<int>(nullable: true),
                    TalentName4 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected4Ticks = table.Column<long>(nullable: true),
                    TalentId7 = table.Column<int>(nullable: true),
                    TalentName7 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected7Ticks = table.Column<long>(nullable: true),
                    TalentId10 = table.Column<int>(nullable: true),
                    TalentName10 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected10Ticks = table.Column<long>(nullable: true),
                    TalentId13 = table.Column<int>(nullable: true),
                    TalentName13 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected13Ticks = table.Column<long>(nullable: true),
                    TalentId16 = table.Column<int>(nullable: true),
                    TalentName16 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected16Ticks = table.Column<long>(nullable: true),
                    TalentId20 = table.Column<int>(nullable: true),
                    TalentName20 = table.Column<string>(maxLength: 75, nullable: false),
                    TimeSpanSelected20Ticks = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchPlayerTalents", x => x.MatchPlayerTalentId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerTalents_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchPlayerTalents_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
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

            migrationBuilder.CreateTable(
                name: "ReplayMatchTeamObjectives",
                columns: table => new
                {
                    MatchTeamTeamObjectiveId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplayId = table.Column<long>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    PlayerId = table.Column<long>(nullable: true),
                    TeamObjectiveType = table.Column<string>(maxLength: 255, nullable: false),
                    TimeStampTicks = table.Column<long>(nullable: true),
                    Value = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayMatchTeamObjectives", x => x.MatchTeamTeamObjectiveId);
                    table.ForeignKey(
                        name: "FK_ReplayMatchTeamObjectives_ReplayAllHotsPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "ReplayAllHotsPlayers",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplayMatchTeamObjectives_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "ReplayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplayHotsApiUploads_ReplayId",
                table: "ReplayHotsApiUploads",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchAwards_PlayerId",
                table: "ReplayMatchAwards",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchAwards_ReplayId",
                table: "ReplayMatchAwards",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchDraftPicks_ReplayId",
                table: "ReplayMatchDraftPicks",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchMessages_ReplayId",
                table: "ReplayMatchMessages",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_PlayerId",
                table: "ReplayMatchPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayers_ReplayId",
                table: "ReplayMatchPlayers",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayerScoreResults_PlayerId",
                table: "ReplayMatchPlayerScoreResults",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayerScoreResults_ReplayId",
                table: "ReplayMatchPlayerScoreResults",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayerTalents_PlayerId",
                table: "ReplayMatchPlayerTalents",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchPlayerTalents_ReplayId",
                table: "ReplayMatchPlayerTalents",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchTeamExperiences_ReplayId",
                table: "ReplayMatchTeamExperiences",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchTeamLevels_ReplayId",
                table: "ReplayMatchTeamLevels",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchTeamObjectives_PlayerId",
                table: "ReplayMatchTeamObjectives",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayMatchTeamObjectives_ReplayId",
                table: "ReplayMatchTeamObjectives",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplayRenamedPlayers_PlayerId",
                table: "ReplayRenamedPlayers",
                column: "PlayerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
                throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "ReplayHotsApiUploads");

            migrationBuilder.DropTable(
                name: "ReplayMatchAwards");

            migrationBuilder.DropTable(
                name: "ReplayMatchDraftPicks");

            migrationBuilder.DropTable(
                name: "ReplayMatchMessages");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayers");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerScoreResults");

            migrationBuilder.DropTable(
                name: "ReplayMatchPlayerTalents");

            migrationBuilder.DropTable(
                name: "ReplayMatchTeamBans");

            migrationBuilder.DropTable(
                name: "ReplayMatchTeamExperiences");

            migrationBuilder.DropTable(
                name: "ReplayMatchTeamLevels");

            migrationBuilder.DropTable(
                name: "ReplayMatchTeamObjectives");

            migrationBuilder.DropTable(
                name: "ReplayRenamedPlayers");

            migrationBuilder.DropTable(
                name: "Replays");

            migrationBuilder.DropTable(
                name: "ReplayAllHotsPlayers");
        }
    }
}
