using System;
using System.Collections.Generic;

namespace AppSpace.Domain.Models;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Size { get; set; } = null!;

    public int Seats { get; set; }

    public int CinemaId { get; set; }

    public virtual Cinema Cinema { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
