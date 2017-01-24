using System;
using System.Diagnostics;
using System.Threading;

using Ava1.lib.core.config;
using Ava1.lib.core.utils;
using Ava1.streetgears.network.auth;
using System.Reflection;

namespace Ava1
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            Log.Start();

            streetgears.network.auth.message.Manager.Initialize(Assembly.GetExecutingAssembly());
            streetgears.network.mmo.message.Manager.Initialize(Assembly.GetExecutingAssembly());
            streetgears.network.relay.message.Manager.Initialize(Assembly.GetExecutingAssembly());

            Config.ReadSettings(AppDomain.CurrentDomain.BaseDirectory + "Ava1.ini");

            streetgears.network.mmo.engine.mmo.Chat.InitializeLevelScale();

            shared.database.Manager.Connect();

            streetgears.network.mmo.engine.clan.Clan.Initialize();
            shared.database.server.Server.Initialize();

            Server.Start();
            streetgears.network.mmo.Server.Start();
            streetgears.network.lobby.Server.Start();
            streetgears.network.relay.Server.Start();
            streetgears.network.msg.Server.Start();

            Log.WriteLog(Log.Type.Info, "Server is up ({0}ms)!", watch.ElapsedMilliseconds);
            Log.WriteLog(Log.Type.Info, "Type 'close' to close the server\n");

            Process sg = Process.Start("StreetGear.exe", "/enc /debug /locale:CP1147 /auth_ip:127.0.0.1 /auth_port:82 /window /debug");

            l_return:
            var readLine = Console.ReadLine();
            if (readLine == "close")
            {
                Log.WriteLog(Log.Type.Info, "Preparing to close the server in 5seconds...");
                streetgears.network.mmo.Server.Debug("Preparing to close the server in 5seconds...");
                Thread.Sleep(3500);
                streetgears.network.mmo.Server.Debug("Closing server...");
                Log.WriteLog(Log.Type.Info, "Ava1 is closing..");
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
            {
                Log.WriteLog(Log.Type.Info, "Unknow command typed"); goto l_return;
            }
        }
    }
}
