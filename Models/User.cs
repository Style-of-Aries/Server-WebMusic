using System;
using System.Collections.Generic;

namespace MusicAPI.Models;

public partial class User
{
    public long Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<ListenHistory> ListenHistories { get; set; } = new List<ListenHistory>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
