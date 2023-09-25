using System;
using System.Collections.Generic;

namespace AppSpace.Domain.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Population { get; set; }

    public virtual ICollection<Cinema> Cinemas { get; set; } = new List<Cinema>();
}
