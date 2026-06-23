using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class Playlist
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? CoverImageUrl { get; set; }

    public bool IsPrivate { get; set; }

    public long UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    public virtual User User { get; set; } = null!;
}
