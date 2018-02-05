using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LogOut
    {
        public int LO_Id { get; set; }
        public int LO_AI_Id { get; set; }
        public string LO_AccountNo { get; set; }
        public string LO_Token { get; set; }
        public DateTime LO_LogOutTime{get;set;}
    }
    //class LogOutMapper : ClassMapper<LogOut>
    //{
    //    public LogOutMapper()
    //    {
    //        Table("ps_LogOut");
    //        // Map(c => c.Id).Column("AI_Id").Key(KeyType.Assigned);
    //        AutoMap();
    //    }
    //}
}
