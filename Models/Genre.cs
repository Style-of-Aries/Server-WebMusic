using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class Genre
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
