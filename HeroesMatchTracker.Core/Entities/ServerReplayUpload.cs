using System;
using System.ComponentModel.DataAnnotations;

namespace HeroesMatchTracker.Core.Entities
{
    public class ServerReplayUpload : IEntity
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [Key]
        public long ServerReplayUploadId { get; set; }

        /// <summary>
        /// Gets or sets the replay id (foreign key).
        /// </summary>
        public long ReplayId { get; set; }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <remarks>TODO: change to enum.</remarks>
        public int ServerName { get; set; }

        /// <summary>
        /// Gets or sets the datetime that the file was uploaded successfully.
        /// </summary>
        public DateTime? UploadedTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the current status of the upload.
        /// </summary>
        /// <remarks>TODO: change to enum.</remarks>
        public int Status { get; set; }

        public virtual ReplayMatch? Replay { get; set; }
    }
}
