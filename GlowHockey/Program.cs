using System;
using System.Net.Sockets;
using System.Net;
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
                //                Opponent opponent = (Opponent)bf.Deserialize(ns);
                //                Console.WriteLine("Your Opponent Port is " + opponent.Ip.Port);
                String msg = sr.ReadLine();
                String[] op= msg.Split(',');
                IPEndPoint opIp = new IPEndPoint(IPAddress.Parse(op[0]), Convert.ToInt32(op[1]));
                Opponent.PlayerType type = op[2] == "T" ? Opponent.PlayerType.Top : Opponent.PlayerType.Bottom;
                IPEndPoint currentIp = new IPEndPoint(IPAddress.Parse(op[3]), Convert.ToInt32(op[4]));
                Application.Run(new GameForm(new Opponent(opIp, type, currentIp)));

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
            Application.Run(new Form1());   
        }
    }
}