using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
   public class ps_LockCloseReq
    {
        public int LCR_Id { get; set; }
        public int LCR_AI_Id { get; set; }
        public string LCR_AccountNo { get; set; }
        public string LCR_Token { get; set; }
        public int LCR_LC_Id { get; set; }
        public string LCR_LockNo { get; set; }
        public int LCR_KeyId { get; set; }
        public int LCR_Round { get; set; }
        public string LCR_Time { get; set; }
        public string LCR_Sign { get; set; }
        public DateTime LCR_CreateTime { get; set; }
    }
}
