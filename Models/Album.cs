using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class Album
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string CoverImageUrl { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    public long ArtistId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
