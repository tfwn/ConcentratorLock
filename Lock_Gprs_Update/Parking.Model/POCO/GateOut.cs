using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_GateOut
    {

        public int GO_Id { get; set; }
        public int GO_AI_Id { get; set; }
        public string GO_AccountNo { get; set; }
        public string GO_Token { get; set; }
        public string GO_BleMac { get; set; }
        public DateTime GO_CreateTime { get; set; }
    }
}
