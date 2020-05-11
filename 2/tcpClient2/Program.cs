﻿using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace tcpClient2
{
    public class chatClient
    {
        private static string userName;
        private const string Host = "127.0.0.1";
        private const int Port = 777;
        private static TcpClient client;
        private static NetworkStream stream;

        private static void Main()
        {
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(Host, Port);
                stream = client.GetStream();

                var message = userName;
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                var receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
                Console.WriteLine("Добро пожаловать, {0}", userName);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        private static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                var message = Console.ReadLine();
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

        private static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var data = new byte[64];
                    var builder = new StringBuilder();
                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    var message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Подключение прервано");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        private static void Disconnect()
        {
            stream?.Close();
            client?.Close();
            Environment.Exit(0);
        }
    }
}