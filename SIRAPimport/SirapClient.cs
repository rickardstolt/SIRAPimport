using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIRAPimport
{
    internal class SirapClient : IDisposable
    {
        private readonly IPEndPoint iPEndPoint;
        private TcpClient tcpClient;

        public SirapClient(IPEndPoint iPEndPoint)
        {
            this.iPEndPoint = iPEndPoint;
        }

        public async Task SendPunch(ushort control, DateTime punchTime, uint chipNo)
        {
            try
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(iPEndPoint.Address, iPEndPoint.Port);
                }

                await SendSirapPunch(chipNo, punchTime, control, tcpClient);
            }
            catch(Exception ex)
            {
                tcpClient.Close();
                tcpClient = null;
                throw;
            }
        }

        private static async Task SendSirapPunch(uint chipNo, DateTime punchTime, ushort control, TcpClient client)
        {
            DateTime zeroTime = punchTime.Date.AddHours(punchTime.Hour < 12 ? 0 : 12);
            //DateTime ZeroTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //ZeroTime.AddHours(zeroTime.Hour);
            //ZeroTime.AddMinutes(zeroTime.Minute);
            NetworkStream ns = client.GetStream();
            byte[] msg = new byte[15];
            msg[0] = (byte)0x00;
            msg[1] = (byte)(control & 0xff);  // == CSI Low
            msg[2] = (byte)((control >> 8) & 0xff); ; // CSI Hi
            msg[3] = (byte)(chipNo & 0xff);
            msg[4] = (byte)((chipNo >> 8) & 0xff);
            msg[5] = (byte)((chipNo >> 16) & 0xff);
            msg[6] = (byte)((chipNo >> 24) & 0xff);
            msg[7] = 0;
            msg[8] = 0;
            msg[9] = 0;
            msg[10] = 0;
            int time = (int)(punchTime.TimeOfDay.TotalMilliseconds / 100);// - zeroTime.TimeOfDay.TotalMilliseconds / 100);
            //if (time < 0)
            //    time += 10 * 60 * 60 * 24;
            msg[11] = (byte)(time & 0xff);
            msg[12] = (byte)((time >> 8) & 0xff);
            msg[13] = (byte)((time >> 16) & 0xff);
            msg[14] = (byte)((time >> 24) & 0xff);
            await ns.WriteAsync(msg, 0, 15);
        }

        public void Dispose()
        {
            if(tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }
    }
}
