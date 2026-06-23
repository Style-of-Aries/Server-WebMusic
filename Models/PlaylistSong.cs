using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class PlaylistSong
{
    public long PlaylistId { get; set; }

    public long SongId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
