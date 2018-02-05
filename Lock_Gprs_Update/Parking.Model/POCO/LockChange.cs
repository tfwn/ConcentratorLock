using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public  class ps_LockChange
    {
        public int LCG_Id { get; set; }
        public int LCG_AI_Id { get; set; }
        public string LCG_AccountNo { get; set; }
        public string LCG_Token { get; set; }
        public int LCG_Old_LC_Id { get; set; }
        public string LCG_OldLockNo { get; set; }
        public string LCG_OldBleMac { get; set; }
        public int LCG_OldKeyId { get; set; }
        public string LCG_OldKey { get; set; }
        public int LCG_New_LC_Id { get; set; }
        public string LCG_NewLockNo { get; set; }
        public string LCG_NewBleMac { get; set; }
        public int LCG_NewKeyId { get; set; }
       public string LCG_NewKey { get; set; }
       public DateTime LCG_CreateTime { get; set; }
    }
}
