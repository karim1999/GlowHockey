using System;
using System.Drawing;
using System.Windows.Forms;
using GlowHockey.GameObjects;

namespace GlowHockey
{
    public partial class GameForm : Form
    {
        private Graphics g;
        private Timer timer;
        private Frame frame;
        private Player player1;
        private Player player2;
        private Ball ball;
        public GameForm()
        {
            InitializeComponent();
            this.Width = 600;
            this.Height = 800;
            g = this.CreateGraphics();
            frame = new Frame( 0, 0, Color.White,  Color.Black, this.Width, this.Height);
            player1 = new Player( Width/2, Height/4, Color.Black,  Color.Gray, 30, Player.Type.Top);
            player2 = new Player( Width/2, Height/4*3, Color.Black,  Color.Gray, 30, Player.Type.Bottom);
            ball = new Ball( Width/2, Height/2, Color.Blue,  Color.Blue, 15);
            timer = new Timer();
            timer.Interval = 1000 / 60;
            timer.Tick+= TimerOnTick;
            timer.Start();
            
            this.MouseMove += OnMouseMove;
            
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(e.X < frame.width - player1.width)
                player1.x = e.X;
            if(e.Y < frame.height - player1.height)
                player1.y = e.Y;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            frame.drawShape(g);
            player1.drawShape(g);
            player2.drawShape(g);
            ball.move(frame, new Player[]{player1, player2});
            
            ball.drawShape(g);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
        }
    }
}