using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LockInit
    {
        public int LS_Id { get; set; }

        public int LS_AI_Id { get; set; }
        public string LS_AccountNo { get; set; }
        public string LS_Token { get; set; }
        public string LS_LockNo { get; set; }
        public string LS_OldLockNo { get; set; }
        public string LS_BleMac { get; set; }
        public int LS_KeyId { get; set; }
        public int LS_Round { get; set; }
        public string Ls_Time { get; set; }
        public string LS_OldKey { get; set; }
        public string LS_NewKey { get; set; }
        public string LS_Sign { get; set; }
        public int LS_UsedFlag { get; set; }
        public int LS_Status { get; set; }
        public DateTime LS_CreateTime { get; set; }
        public int LS_Change { get; set; }
        public int LS_IsDel { get; set; }
    }
}
