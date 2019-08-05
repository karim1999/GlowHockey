using System;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using OpponentLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace GlowHockey
{
    class ClientThread
    {
        String ip;
        int port;

        public ClientThread(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void handle()
        {
            TcpClient soc = new TcpClient(ip, port);
            NetworkStream ns = soc.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            BinaryFormatter bf = new BinaryFormatter();


            //communicate with server
            bool isWaiting = true;
            while (isWaiting)
            {
                Opponent opponent = (Opponent)bf.Deserialize(ns);
                //                Console.WriteLine("Your Opponent Port is " + opponent.Ip.Port);
                Application.Run(new GameForm(opponent));

                isWaiting = false;

            }
            Console.WriteLine("stop");


            sr.Close();
            sw.Close();
            soc.Close();
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            ClientThread ct = new ClientThread("127.0.0.1", 6691);
            Thread th = new Thread(ct.handle);
            th.Start();
        }
    }
}