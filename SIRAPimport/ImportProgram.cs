using Newtonsoft.Json;
using NLog;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SIRAPimport
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class ImportProgram
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Import punches from SportIdent Center")]
        public async Task SportIdentCenter(SportIdentCenterArgs args)
        {
            try
            {
                using (var sirapClient = new SirapClient(new IPEndPoint(IPAddress.Parse(args.SirapHost), args.SirapPort)))
                {
                    var afterId = args.AfterId;

                    while (true)
                    {
                        try
                        {
                            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(args.Timeout) })
                            {
                                UriBuilder uriBuilder = new UriBuilder(args.Url);
                                IList<string> queryList = new List<string>();
                                queryList.Add($"afterId={afterId}");
                                if (args.After != DateTime.MinValue)
                                {
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                                    queryList.Add($"after={(args.After - epoch).TotalMilliseconds}");
                                }
                                if (args.Modem != null)
                                {
                                    queryList.Add($"modem={args.Modem}");
                                }
                                uriBuilder.Query = string.Join("&", queryList);

                                var httpResponseMessage = await httpClient.GetAsync(uriBuilder.Uri);

                                if (httpResponseMessage.IsSuccessStatusCode)
                                {
                                    var json = await httpResponseMessage.Content.ReadAsStringAsync();

                                    var punches = JsonConvert.DeserializeObject<Punch[]>(json);

                                    foreach (var punch in punches)
                                    {
                                        await sirapClient.SendPunch((ushort)punch.Code, punch.GetTime(), uint.Parse(punch.Card));
                                        if (afterId < punch.Id)
                                        {
                                            afterId = punch.Id;
                                        }
                                        Log.WithProperty("afterId", afterId).Info($"Punch\t{punch.Code}\t{punch.Card}\t{punch.GetTime().ToString("yyyy-MM-dd HH:mm:ss")}");
                                    }
                                }
                                else
                                {
                                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                                    content = content?.Trim();

                                    if(!string.IsNullOrEmpty(content))
                                    {
                                        Log.WithProperty("afterId", afterId).Error($"Error response from API: {content}");
                                    }
                                    else
                                    {
                                        httpResponseMessage.EnsureSuccessStatusCode();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WithProperty("afterId", afterId).Error(ex, "Error importing punches.");
                        }

                        await Task.Delay(args.Period);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WithProperty("afterId", args.AfterId).Fatal(ex, $"Failed to execute action {nameof(SportIdentCenter)}.");
            }
        }
    }
}
