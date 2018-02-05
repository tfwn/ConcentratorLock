using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_KeyIdUpdate
    {
        public int KIU_Id { get; set; }
        public string KIU_GateWayNo { get; set; }
        public int KIU_LC_Id { get; set; }
        public string KIU_LockNo { get; set; }
        public int KIU_OldKeyId { get; set; }
        public int KIU_NewKeyId { get; set; }
        public int KIU_OldKey { get; set; }
        public int KIU_NewKey { get; set; }

        public int KIU_Round { get; set; }
       public string KIU_Sign { get; set; }
       public int KIU_Status { get; set; }
       public DateTime KIU_CreateTime { get; set; }
       public DateTime? KIU_UpdateTime { get; set; }
    }
}
