using HeroesMatchTracker.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeroesMatchTracker.Infrastructure.Database.Contexts
{
    public class HeroesReplaysDbContext : DbContext
    {
        public HeroesReplaysDbContext(DbContextOptions<HeroesReplaysDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ReplayMatch> Replays { get; set; } = null!;

        public virtual DbSet<ServerReplayUpload> ServerReplayUploads { get; set; } = null!;

        public virtual DbSet<ReplayMatchPlayer> ReplayMatchPlayers { get; set; } = null!;

        public virtual DbSet<ReplayPlayer> ReplayPlayers { get; set; } = null!;

        public virtual DbSet<ReplayPlayerToon> ReplayPlayerToons { get; set; } = null!;

        public virtual DbSet<ReplayOldPlayerInfo> ReplayOldPlayerInfos { get; set; } = null!;

        public virtual DbSet<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; } = null!;

        public virtual DbSet<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; } = null!;

        //public virtual DbSet<ReplayHotsApiUpload> ReplayHotsApiUploads { get; set; } = null!;

        //public virtual DbSet<ReplayMatchAward> ReplayMatchAwards { get; set; } = null!;

        //public virtual DbSet<ReplayMatchDraftPick> ReplayMatchDraftPicks { get; set; } = null!;

        //public virtual DbSet<ReplayMatchMessage> ReplayMatchMessages { get; set; } = null!;


        //public virtual DbSet<ReplayMatchPlayerLoadout> ReplayMatchPlayerLoadouts { get; set; } = null!;





        //public virtual DbSet<ReplayMatchTeamBan> ReplayMatchTeamBans { get; set; } = null!;

        //public virtual DbSet<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; } = null!;

        //public virtual DbSet<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; } = null!;



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        //        .UseLazyLoadingProxies()
        //        .UseSqlite(DbConnectionString.HeroesReplays);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<ReplayMatch>()
                .HasIndex(x => x.Hash)
                .IsUnique();

            modelBuilder.Entity<ReplayPlayer>()
                .HasOne(x => x.ReplayPlayerToon)
                .WithOne(x => x!.ReplayPlayer!)
                .HasForeignKey<ReplayPlayerToon>(x => x.PlayerId);

            modelBuilder.Entity<ReplayPlayerToon>()
                .HasIndex(x => new { x.Region, x.ProgramId, x.Realm, x.Id });

            modelBuilder.Entity<ReplayMatchPlayer>()
                .HasOne(x => x.ReplayMatchPlayerScoreResult)
                .WithOne(x => x!.ReplayMatchPlayer!)
                .HasForeignKey<ReplayMatchPlayerScoreResult>(x => x.MatchPlayerId);

            modelBuilder.Entity<ReplayMatchPlayer>()
                .HasOne(x => x.ReplayMatchPlayerTalent)
                .WithOne(x => x!.ReplayMatchPlayer!)
                .HasForeignKey<ReplayMatchPlayerTalent>(x => x.MatchPlayerId);

            //modelBuilder.Entity<ReplayMatch>()
            //    .HasMany(x => x.ReplayMatchAwards)
            //    .WithOne(x => x.ReplayMatch)
            //    .HasForeignKey(x => x.ReplayId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ReplayPlayer>()
            //    .HasMany(x => x.ReplayMatchAwards)
            //    .WithOne(x => x.ReplayPlayer)
            //    .HasForeignKey(x => x.PlayerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ReplayPlayer>()
            //    .HasMany(x => x.ReplayMatchPlayers)
            //    .WithOne(x => x.ReplayPlayer)
            //    .HasForeignKey(x => x.PlayerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ReplayPlayer>()
            //    .HasMany(x => x.ReplayMatchPlayerScoreResults)
            //    .WithOne(x => x.ReplayPlayer)
            //    .HasForeignKey(x => x.PlayerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ReplayPlayer>()
            //    .HasMany(x => x.ReplayMatchPlayerTalents)
            //    .WithOne(x => x.ReplayPlayer)
            //    .HasForeignKey(x => x.PlayerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ReplayPlayer>()
            //    .HasOne(x => x.ReplayRenamedPlayer)
            //    .WithOne(x => x.ReplayPlayer)
            //    .HasForeignKey<ReplayRenamedPlayer>(x => x.PlayerId)
            //    .OnDelete(DeleteBehavior.Cascade);



            //modelBuilder.Entity<ReplayMatchPlayer>()
            //    .HasOne(x => x.ReplayMatchPlayerLoadout)
            //    .WithOne(x => x.ReplayMatchPlayer)
            //    .HasForeignKey<ReplayMatchPlayerLoadout>(x => x.MatchPlayerId);
        }
    }
}
