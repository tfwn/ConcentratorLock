using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
   public  class ps_GateIn
    {
        public int GI_Id { get; set; }
        public int GI_AI_Id { get; set; }
        public string GI_AccountNo { get; set; }
        public string GI_Token { get; set; }
       public string GI_BleMac { get; set; }
       public DateTime GI_CreateTime { get; set; }
    }
}
