using System;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using OpponentLibrary;
using System.Runtime.Serialization.Formatters.Binary;

namespace GlowHockey
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TcpClient soc = new TcpClient("127.0.0.1", 6691);
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
}