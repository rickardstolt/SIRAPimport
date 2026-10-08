using PowerArgs;
using System;

namespace SIRAPimport
{
    internal class PunchArgs
    {
        [ArgRequired, ArgDescription("Control/station code")]
        public ushort Control { get; set; }

        [ArgRequired, ArgDescription("Chip/card number")]
        public uint Chip { get; set; }

        [ArgDescription("Punch time (local time). Defaults to the current time if omitted")]
        public DateTime Time { get; set; }

        [ArgDescription("Host for SIRAP receiver"), ArgDefaultValue("127.0.0.1")]
        public string SirapHost { get; set; }

        [ArgDescription("Port for SIRAP receiver"), ArgDefaultValue(10001)]
        public int SirapPort { get; set; }
    }
}
