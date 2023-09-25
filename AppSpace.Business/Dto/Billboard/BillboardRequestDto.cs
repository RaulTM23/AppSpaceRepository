using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Billboard
{
    public class BillBoardRequestDto
    {
        public int WeeksPeriod { get; set; }
        public int ScreensBigRoom { get; set; }
        public int ScreensSmallRoom { get; set; }
    }
}
