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


            Application.Run(new GameForm());
        }
    }
}