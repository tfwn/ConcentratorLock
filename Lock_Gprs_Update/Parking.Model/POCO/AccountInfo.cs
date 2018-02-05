using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions.Mapper;

namespace Parking.Model.POCO
{
    public class ps_AccountInfo
    {
        public int AI_Id { get;set;}

        public string AI_AccountNo { get; set; }
        public string AI_Password { get; set; }
        public string AI_Token { get; set; }
        /// <summary>
        /// 登录方式,0手机登录；1网页登录；
        /// </summary>
        public int? AI_LoginType { get; set; }

        public DateTime? AI_LoginTime { get; set; }
        public string AI_LoginIp { get; set; }
        public DateTime? AI_LogoutTime { get; set; }
        /// <summary>
        /// 0未登录；1已登录；
        /// </summary>
        public int? AI_LoginStatus { get; set; }

        /// <summary>
        ///账户锁定状态 int	0未锁定；1已锁定；
        /// </summary>
        public int? AI_IsLock { get; set; }
        public DateTime? AI_CreateTime { get; set; }
        public int AI_CreateBy { get; set; }
        public DateTime? AI_ModifyTime { get; set; }
        public int? AI_ModifyBy { get; set; }
        /// <summary>
        /// 删除标志,0未删除；1已删除；
        /// </summary>
        public int? AI_IsDel { get; set; }
        public int AI_AccountType { get; set; }
    }

    //class AccountInfoMapper : ClassMapper<AccountInfo>
    //{
    //    public AccountInfoMapper()
    //    {
    //        Table("ps_AccountInfo");
    //        // Map(c => c.Id).Column("AI_Id").Key(KeyType.Assigned);
    //        AutoMap();
    //    }
    //}

    public class LoginStatus
    {
        public const int NotLogin = 0;
        public const int Logined = 1;
    }

    public class LoginType
    {
        public const int Mobile = 0;
        public const int Web = 1;
    }

    public class LUG_DataType
    {
        /// <summary>
        /// 开
        /// </summary>
        public const int Open = 0;
        /// <summary>
        /// 关
        /// </summary>
        public const int Close = 1;
        /// <summary>
        /// 状态上报
        /// </summary>
        public const int UploadStatus = 2;

        public const int InitStatus = 3;

        public const int ChangeStatus = 4;
    }
}
