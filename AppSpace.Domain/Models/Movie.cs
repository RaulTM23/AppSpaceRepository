using System;
using System.Collections.Generic;

namespace AppSpace.Domain.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string OriginalTitle { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public string? OriginalLanguage { get; set; }

    public bool Adult { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
