using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LockUpStatusByMobil
    {
        public int LUM_Id { get; set; }
        public int LUM_DataType { get; set; }
        public int LUM_AI_Id { get; set; }
        public string LUM_AccountNo { get; set; }
        public int LUM_LC_Id { get; set; }
        public string LUM_LockNo { get; set; }
        public Decimal LUM_Voltage { get; set; }
        public string LUM_LockStatus { get; set; }
        public DateTime LUM_CreateTime { get; set; }

    }
}
