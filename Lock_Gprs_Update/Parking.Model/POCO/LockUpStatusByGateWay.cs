using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
     public class ps_LockUpStatusByGateWay
    {
        public int LUG_Id { get; set; }
        public string LUG_GateWayNo { get; set; }
        public int LUG_DataType { get; set; }
        public int LUG_AI_Id { get; set; }
        public string LUG_AccountNo { get; set; }
        public int LUG_LC_Id { get; set; }
        public string LUG_LockNo { get; set; }
        public string LUG_BleMac { get; set; }
        public string LUG_DataTime { get; set; }
        public decimal LUG_Voltage { get; set; }
        public string LUG_LockStatus { get; set; }
        public int LUG_UpRssi { get; set; }
        public int LUG_DownRssi { get; set; }
        public DateTime LUG_CreateTime { get; set; }
        

    }
}
