using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    /// <summary>
    ///  若上一条命令未下发或下发失败，当前命令的操作方式。
    ///  主要原因是因为集中器不能主动把命令发给地锁，只能先缓存命令到本地，再等待地锁上报状态，而且集中器又只能缓存一条命令
    /// </summary>
    public class RecentCmdFailOp
    {
        /// <summary>
        /// 若上一条命令未下发或下发失败，回复服务器当前命令下发失败
        /// </summary>
        public const int CurrentCmdFail = 0;

        /// <summary>
        /// 若上一条命令未下发或下发失败，直接覆盖上一条命令，并回复服务器当前命令下发并覆盖成功
        /// </summary>
        public const int OverWrite = 1;
    }
}
