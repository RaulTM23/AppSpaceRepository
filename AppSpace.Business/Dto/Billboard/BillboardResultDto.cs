using AppSpace.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Billboard
{
    public class BillboardResultDto
    {
        public int Screen { get; set; }
        public string Movie { get; set; }
        public List<string> Genres { get; set; }
        public int RoomId { get; set; }
    }
}
