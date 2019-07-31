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
        private Goal goal1;
        private Goal goal2;
        private Ball ball;

        private int screenWidth= 600;
        private int screenHeight= 800;
        
        public GameForm()
        {
            InitializeComponent();
            
            this.Size= new Size(600+10, 800+30);
            
            g = this.CreateGraphics();
            frame = new Frame( 0, 0, Color.Black,  Color.Yellow, this.screenWidth, this.screenHeight);
            player1 = new Player( screenWidth/2 - Player.radius, screenHeight/4 - Player.radius, Color.Green, Color.Gray, Player.Type.Top);
            goal1= new Goal(screenWidth/2 - Goal.width/2, 0, screenWidth / 2 + Goal.width/2, Goal.height, player1);

            player2 = new Player( screenWidth/2 - Player.radius, screenHeight/4*3  - Player.radius, Color.Red,  Color.Gray, Player.Type.Bottom);
            goal2= new Goal(screenWidth/2 - Goal.width/2, screenHeight, screenWidth / 2 + Goal.width/2, screenHeight - Goal.height, player2);

            ball = new Ball( screenWidth/2, screenHeight/2, Color.Blue,  Color.Blue);
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick+= TimerOnTick;
            timer.Start();
            
            this.MouseMove += OnMouseMove;
            
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(e.X < screenWidth - player1.width)
                player1.x = e.X;
            if(e.Y < screenHeight/2 - player1.height)
                player1.y = e.Y;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            frame.drawShape(g);
            g.DrawLine(new Pen(Color.Yellow, 5), new PointF(0, screenHeight/2), new PointF(screenWidth, screenHeight/2));

            player2.drawShape(g);
            player1.drawShape(g);
            goal1.drawShape(g);
            goal2.drawShape(g);
            ball.move(frame, new Player[]{player1, player2});
            
            ball.drawShape(g);
            
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
        }
    }
}