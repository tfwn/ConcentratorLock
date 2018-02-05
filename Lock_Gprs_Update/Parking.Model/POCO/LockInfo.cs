using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LockInfo
    {
        public int LC_Id { get; set; }
        public int LC_AI_Id { get; set; }
        public string LC_LockNo { get; set; }
        public string LC_BleMac { get; set;  }
        public int LC_KeyId { get; set; }
        public string LC_Key { get; set; }
        public int LC_Status { get; set; }
        public int LC_Disable { get; set; }
        public string LC_PosNo { get; set; }
        public string LC_Position { get; set; }
        public DateTime LC_CreateTime { get; set; }
        public int LC_CreateBy { get; set; }
        public DateTime? LC_ModifyTime { get; set; }
        public int LC_ModifyBy { get; set; }
        public int LC_IsDel { get; set; }
    }

    //class LockInfoMapper : ClassMapper<LockInfo>
    //{
    //    public LockInfoMapper()
    //    {
    //        Table("ps_LockInfo");
    //        // Map(c => c.Id).Column("AI_Id").Key(KeyType.Assigned);
    //        AutoMap();
    //    }
    //}
}
