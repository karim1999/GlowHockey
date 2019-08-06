using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace GlowHockey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Cnct_btn_Click(object sender, EventArgs e)
        {
            ClientThread ct = new ClientThread(IPtxt.Text, Convert.ToInt32(Porttxt.Text));
            Thread th = new Thread(ct.handle);
            this.Hide();
            th.Start();

        }
    }
}
