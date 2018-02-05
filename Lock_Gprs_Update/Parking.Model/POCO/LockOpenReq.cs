using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LockOpenReq
    {
       public int LOR_Id { get; set; }
       public int LOR_AI_Id { get; set; }
      public string LOR_AccountNo { get; set; }
      public string LOR_Token { get; set; }

      public int LOR_LC_Id { get; set; }
      public string LOR_LockNo { get; set; }
      public DateTime LOR_CreateTime { get; set; }
      public string LOR_Time { get; set; }

        public string LOR_Sign { get; set; }
    }
}
