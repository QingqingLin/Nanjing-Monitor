using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UnpackPcap
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]

    public struct IPV6
    {

        public UInt32 VersionAndTraffic;

        public UInt16 PayLoadLength;

        public byte UdpLite;

        public byte HopLimit;

        public UInt64 SourceTop;

        public UInt64 SoruceDown;

        public UInt64 DestTop;

        public UInt64 DestDown;

    }
}
