using System;
using System.Collections.Generic;

namespace AppSpace.Domain.Models;

public partial class Session
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int MovieId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int? SeatsSold { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
