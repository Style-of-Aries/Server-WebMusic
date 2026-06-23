using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class Artist
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<SongArtist> SongArtists { get; set; } = new List<SongArtist>();
}
