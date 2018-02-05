using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
   public  class ps_KeyIdList
    {
        public int KL_Id { get; set; }
        public int KL_AI_Id { get; set; }
        public string KL_AccountNo { get; set; }
        public string KL_Token { get; set; }
        public int KL_LC_Id { get; set; }
        public string KL_LockNo { get; set; }
        public int KL_KeyId { get; set; }
        public DateTime KL_CreateTime { get; set; }
    }
}
