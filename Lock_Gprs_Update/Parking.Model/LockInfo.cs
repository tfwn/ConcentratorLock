using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public  class LockInfo
    {
        public string AccountNo { get; set; }
        public string LockNo { get; set; }
        public string BleMac { get; set; }
        public string Time { get; set; }
        public string KeyId { get; set; }
        public string Round { get; set; }
        public string Sign { get; set; }

    }
}
