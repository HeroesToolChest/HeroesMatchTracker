using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.HMT2Contexts;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.IO;

namespace HeroesMatchTracker.Infrastructure.Database
{
    public class DatabaseInit : IDatabaseInit, IEnableLogger
    {
        private const string _backupDirectory = "_backup";
        private const string _replaySqlite = "Replays.sqlite";

        private readonly HMT2ReplaysDbContext _hmt2ReplaysDbContext;
        private readonly HeroesReplaysDbContext _heroesReplaysDbContext;

        public DatabaseInit(
            HMT2ReplaysDbContext hmt2ReplaysDbContext,
            HeroesReplaysDbContext heroesReplaysDbContext)
        {
            _hmt2ReplaysDbContext = hmt2ReplaysDbContext;
            _heroesReplaysDbContext = heroesReplaysDbContext;
        }

        public void HMT2ReplayDbCheck()
        {
            if (File.Exists(_replaySqlite))
            {
                this.Log().Info($"Found existing {_replaySqlite} (HMT2) file.");
                this.Log().Info($"Creating backup of {_replaySqlite} (HMT2) file at {_backupDirectory} directory");

                try
                {
                    Directory.CreateDirectory(_backupDirectory);
                    File.Copy(_replaySqlite, Path.Join(_backupDirectory, $"{_replaySqlite}"));
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex, $"Failed to create back up file. Update to HMT3 failed.");
                    return;
                }

                this.Log().Info($"Performing HMT3 update...");
                _hmt2ReplaysDbContext.Database.ExecuteSqlRaw(@"
                    BEGIN;

                    CREATE TABLE __EFMigrationsHistory(
                    MigrationId TEXT PRIMARY KEY NOT NULL,
                    ProductVersion TEXT NOT NULL);

                    INSERT INTO __EFMigrationsHistory VALUES ('20200711043321_InitialCreate', '3.1.5');

                    COMMIT");

                this.Log().Info("Executed migration history successfully.");

                string newFileName = DbConnectionString.HeroesReplays.Substring(DbConnectionString.HeroesReplays.IndexOf('=', StringComparison.OrdinalIgnoreCase) + 1);

                try
                {
                    File.Move(_replaySqlite, newFileName);
                    this.Log().Info($"File renamed from {_replaySqlite} file to {newFileName}");
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex, $"Unable to rename {_replaySqlite} file to {newFileName}");
                }
            }
        }

        public void InitHeroesReplaysDb()
        {
            this.Log().Info($"Migrating {DbConnectionString.HeroesReplays} database...");
            _heroesReplaysDbContext.Database.Migrate();
            this.Log().Info($"{DbConnectionString.HeroesReplays} migration completed.");


            //ReplayMatch replayMatch = new ReplayMatch();
            //replayMatch.FileName = "somename";
            //replayMatch.Frames = 55;
            //replayMatch.GameSpeed = "fast";
            //replayMatch.Hash = "sdfsdfsdfsdf";
            //replayMatch.IsGameEventsParsed = true;
            //replayMatch.MapName = "Sky Temple";
            //replayMatch.RandomValue = 234324;
            //replayMatch.ReplayBuild = 45454;
            //replayMatch.ReplayLengthTicks = 234234234234;
            //replayMatch.ReplayVersion = "4.44.44.444";
            //replayMatch.TeamSize = "5v5";
            //replayMatch.TimeStamp = DateTime.Now;
            //replayMatch.ReplayId = 5;
            //replayMatch.GameMode = Heroes.StormReplayParser.Replay.GameMode.HeroLeague;

            //replayMatch.ReplayMatchPlayers = new List<ReplayMatchPlayer>();
            //replayMatch.ReplayMatchPlayers.Add(new ReplayMatchPlayer()
            //{
            //    AccountLevel = 324,
            //    Character = "Sd",
            //    CharacterLevel = 43,
            //    Difficulty = "sf",
            //    Handicap = 4,
            //    HasActiveBoost = false,
            //    IsAutoSelect = false,
            //    IsBlizzardStaff = false,
            //    IsSilenced = false,
            //    IsVoiceSilenced = false,
            //    IsWinner = true,
            //    MountAndMountTint = "sdf",
            //    PartySize = 4,
            //    PartyValue = 234,
            //    PlayerNumber = 2,
            //    SkinAndSkinTint = "Sdfr",
            //    Team = 0,
            //    ReplayId = 5,
            //    ReplayAllHotsPlayer = new ReplayAllHotsPlayer()
            //    {
            //        AccountLevel = 324,
            //        BattleNetId = 32,
            //        BattleNetRegionId = 1,
            //        BattleNetSubId = 1,
            //        BattleNetTId = "sdf",
            //        BattleTagName = "ssdf#324",
            //        LastSeen = DateTime.Now,
            //        LastSeenBefore = DateTime.Now,
            //        PlayerId = 3,
            //    },
            //});


            //_heroesReplaysDbContext.Replays.Add(replayMatch);

            //ReplayAllHotsPlayer replayAllHotsPlayer = new ReplayAllHotsPlayer();
            //replayAllHotsPlayer.AccountLevel = 324;
            //replayAllHotsPlayer.BattleNetId = 32;
            //replayAllHotsPlayer.BattleNetRegionId = 1;
            //replayAllHotsPlayer.BattleNetSubId = 1;
            //replayAllHotsPlayer.BattleNetTId = "sdf";
            //replayAllHotsPlayer.BattleTagName = "ssdf#324";
            //replayAllHotsPlayer.LastSeen = DateTime.Now;
            //replayAllHotsPlayer.LastSeenBefore = DateTime.Now;
            //replayAllHotsPlayer.PlayerId = 3;
            //_heroesReplaysDbContext.ReplayMatchPlayers.Add(replayMatchAward);

            //ReplayMatchAward replayMatchAward = new ReplayMatchAward();
            //replayMatchAward.Award = "Bulwark";
            //replayMatchAward.ReplayId = 5;
            //replayMatchAward.PlayerId = 3;
            //_heroesReplaysDbContext.ReplayMatchAwards.Add(replayMatchAward);

            _heroesReplaysDbContext.SaveChanges();
        }
    }
}
