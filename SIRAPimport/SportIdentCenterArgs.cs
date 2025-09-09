using PowerArgs;
using System;

namespace SIRAPimport
{
    internal class SportIdentCenterArgs
    {
        [ArgDescription("List of modem serial numbers (strings), separated by comma")]
        public string Modem { get; set; }

        [ArgDescription("One single event id")]
        public string EventId { get; set; }

        [ArgDescription("A single user name. (Refers to the modem user, not the modem owner)")]
        public string User { get; set; }

        [ArgDescription("Fetch punches after (>=) this punch time (local time)")]
        public DateTime After { get; set; }

        [ArgDescription("Get all punches after (>) this id"), ArgDefaultValue(0)]
        public ulong AfterId { get; set; }

        [ArgDescription("Url for retrieving punches"), ArgDefaultValue("https://center-origin.sportident.com/api/rest/v1/punches")]
        public string Url { get; set; }

        [ArgDescription("Host for SIRAP receiver"), ArgDefaultValue("127.0.0.1")]
        public string SirapHost { get; set; }

        [ArgDescription("Port for SIRAP receiver"), ArgDefaultValue(10001)]
        public int SirapPort { get; set; }

        [ArgDescription("Fetch new punches every period (milliseconds)"), ArgDefaultValue(5000)]
        public int Period { get; set; }

        [ArgDescription("Connection timeout for connection to SportIdent Center"), ArgDefaultValue(10000)]
        public int Timeout { get; set; }
    }
}
