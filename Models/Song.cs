using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class Song
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string FileSong { get; set; } = null!;

    public string FileImage { get; set; } = null!;

    public int Duration { get; set; }

    public int ListenCount { get; set; }

    public long? AlbumId { get; set; }

    public long? GenreId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Album? Album { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<ListenHistory> ListenHistories { get; set; } = new List<ListenHistory>();

    public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    public virtual ICollection<SongArtist> SongArtists { get; set; } = new List<SongArtist>();
}
