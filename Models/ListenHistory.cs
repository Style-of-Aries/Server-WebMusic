using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class ListenHistory
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long SongId { get; set; }

    public DateTime? ListenedAt { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
