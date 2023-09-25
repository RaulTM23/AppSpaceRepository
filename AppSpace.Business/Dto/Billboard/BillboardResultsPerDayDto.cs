using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Billboard
{
    public class BillboardResultsPerDayDto
    {
        public List<BillboardResultDto> BigRoomList { get; set; } = new List<BillboardResultDto>();
        public List<BillboardResultDto> SmallRoomList { get; set; } = new List<BillboardResultDto>();
        public int WeekDay { get; set; }
        public int Week { get; set; }
    }
}
