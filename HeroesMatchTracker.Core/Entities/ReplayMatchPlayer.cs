using Heroes.StormReplayParser.Player;
using Heroes.StormReplayParser.Replay;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ReplayMatchPlayer : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long MatchPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the player id (foreign key).
        /// </summary>
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the player's team.
        /// </summary>
        public StormTeam Team { get; set; }

        /// <summary>
        /// Gets or sets the player's control type.
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Gets or sets the player's index.
        /// </summary>
        public int PlayerNumber { get; set; }

        /// <summary>
        /// Gets or sets the hero name.
        /// </summary>
        public string? HeroName { get; set; }

        /// <summary>
        /// Gets or sets the hero id.
        /// </summary>
        public string? HeroId { get; set; }

        /// <summary>
        /// Gets or sets the hero level.
        /// </summary>
        public int? HeroLevel { get; set; }

        /// <summary>
        /// Gets or sets the hero unit id.
        /// </summary>
        public string? HeroUnitId { get; set; }

        /// <summary>
        /// Gets or sets the hero attribute id.
        /// </summary>
        public string? HeroAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the player's account level.
        /// </summary>
        public int? AccountLevel { get; set; }

        /// <summary>
        /// Gets or sets the player's party value. Those in the same party have the same value.
        /// </summary>
        public long? PartyValue { get; set; }

        /// <summary>
        /// Gets or sets the size of the player's party. This value will be 1 if the player is solo. If the value is <see langword="null"/>
        /// the size of the party is unknown.
        /// </summary>
        public int? PartySize { get; set; }

        /// <summary>
        /// Gets or sets the computer player difficulty.
        /// </summary>
        public string? Difficulty { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the player is auto select or not.
        /// </summary>
        public bool? IsAutoSelect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player has been given the silence penalty.
        /// </summary>
        public bool IsSilenced { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player has been given the voice silence penalty.
        /// </summary>
        public bool? IsVoiceSilenced { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player won the game.
        /// </summary>
        public bool? IsWinner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is Blizzard staff.
        /// </summary>
        public bool? IsBlizzardStaff { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player has an active boost.
        /// </summary>
        public bool? HasActiveBoost { get; set; }

        public virtual ReplayMatch? Replay { get; set; }

        public virtual ReplayPlayer? ReplayPlayer { get; set; }

        public virtual ReplayMatchPlayerScoreResult? ReplayMatchPlayerScoreResult { get; set; }

        public virtual ReplayMatchPlayerTalent? ReplayMatchPlayerTalent { get; set; }

        public virtual ReplayMatchPlayerLoadout? ReplayMatchPlayerLoadout { get; set; }

        public virtual ICollection<ReplayMatchAward>? ReplayMatchAward { get; set; }

        public virtual ReplayMatchDraftPick? ReplayMatchDraftPick { get; set; }
    }
}
