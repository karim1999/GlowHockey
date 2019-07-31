using System;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace GlowHockey
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            Application.Run(new GameForm());
            TcpClient soc = new TcpClient("127.0.0.1", 6691);

            StreamReader sr = new StreamReader(soc.GetStream());
            StreamWriter sw = new StreamWriter(soc.GetStream());


            //communicate with server
            while (true)
            {
                Console.WriteLine("please enter a message:");
                String userMsg = Console.ReadLine();
                sw.WriteLine(userMsg);
                sw.Flush();
                String serverResponse = sr.ReadLine();

                Console.WriteLine(serverResponse);
                if (userMsg.Equals("bye"))
                    break;
            }


            sr.Close();
            sw.Close();
            soc.Close();
        }
    }
}