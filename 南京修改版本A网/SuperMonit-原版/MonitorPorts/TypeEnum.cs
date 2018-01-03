using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorPorts
{
    public enum SendDir :int
    {
        /// <summary>
        /// 车载发送给地面
        /// </summary>
        VOBCToGround = 1,
        /// <summary>
        /// 地面发送给车载
        /// </summary>
        GroundToVOBC = 2
    }
}
