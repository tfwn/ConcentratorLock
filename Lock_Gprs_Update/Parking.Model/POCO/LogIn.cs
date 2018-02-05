using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LogIn
    {
        public int LI_Id { get; set; }
        public int LI_AI_Id { get; set; }
        public string LI_AccountNo { get; set; }
        public string LI_Token { get; set; }
        public int LI_LoginType { get; set; }
        public DateTime LI_LoginTime { get; set; }
        public string LI_LoginIp { get; set; }
    }

    //class LogInMapper : ClassMapper<LogIn>
    //{
    //    public LogInMapper()
    //    {
    //        Table("ps_LogIn");
    //        // Map(c => c.Id).Column("AI_Id").Key(KeyType.Assigned);
    //        AutoMap();
    //    }
    //}
}
