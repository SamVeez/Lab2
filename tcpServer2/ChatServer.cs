using System;
using System.Threading;

namespace tcpServer2
{
    public class ChatServer
    {
        private static ServerObject server;
        private static Thread listenThread;
        private static void Main()
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(server.Listen);
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}