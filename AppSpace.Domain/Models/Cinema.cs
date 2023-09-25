using System;
using System.Collections.Generic;

namespace AppSpace.Domain.Models;

public partial class Cinema
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime OpenSince { get; set; }

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
