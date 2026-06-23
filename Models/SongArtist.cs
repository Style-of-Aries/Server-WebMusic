using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class SongArtist
{
    public long SongId { get; set; }

    public long ArtistId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
