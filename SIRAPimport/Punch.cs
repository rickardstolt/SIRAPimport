using System;

namespace SIRAPimport
{
    internal class Punch
    {
        public ulong Id { get; set; }
        public string Card { get; set; }
        public ulong Time { get; set; }
        public uint Code { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Modem { get; set; } = string.Empty;
        public ulong ReceptionTimeUtc { get; set; }

        public DateTime GetTime()
        {
            DateTime baseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

            return baseDateTime.AddMilliseconds(Time);
        }
    }
}
