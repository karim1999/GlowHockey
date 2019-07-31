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

        private bool isRunning;
        
        public GameForm()
        {
            InitializeComponent();
            
            this.Size= new Size(600+10, 800+30);
            
            initializeGameObjects();
            
            timer = new Timer();
            timer.Interval = 1000/60;
            timer.Tick+= TimerOnTick;
            timer.Start();
            
            this.MouseMove += OnMouseMove;
            
        }

        public void initializeGameObjects(bool withScores = false)
        {
            int player1Score = 0;
            int player2Score = 0;

            if (withScores)
            {
                player1Score = player1.score;
                player2Score = player2.score;

            }
            g = this.CreateGraphics();
            frame = new Frame( 0, 0, Color.Black,  Color.Yellow, this.screenWidth, this.screenHeight);
            player1 = new Player( screenWidth/2 - Player.radius, screenHeight/4 - Player.radius, Color.Green, Color.Gray, Player.Type.Top);
            goal1= new Goal(screenWidth/2 - Goal.width/2, 0, screenWidth / 2 + Goal.width/2, Goal.height, player1);

            player2 = new Player( screenWidth/2 - Player.radius, screenHeight/4*3  - Player.radius, Color.Red,  Color.Gray, Player.Type.Bottom);
            goal2= new Goal(screenWidth/2 - Goal.width/2, screenHeight, screenWidth / 2 + Goal.width/2, screenHeight - Goal.height, player2);

            ball = new Ball( screenWidth/2 - Ball.radius, screenHeight/2 - Ball.radius, Color.Blue,  Color.Blue);

            player1.setOpponent(player2);
            player2.setOpponent(player1);
            if (withScores)
            {
                player1.score = player1Score;
                player2.score = player2Score;
            }
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
            ball.drawShape(g);

            move(frame, new Player[]{player1, player2}, new Goal[]{goal1, goal2});

            checkGameStatus();
        }

        public void move(Frame frame, Player[] players, Goal[] goals)
        {
            PointF ballCenter= new PointF(ball.x+Ball.radius, ball.y+Ball.radius);
            if ((ballCenter.X) - frame.x <= Ball.radius || frame.width - (ballCenter.X) <= Ball.radius )
            {
                ball.speedX *= -1;
            }
            if ((ballCenter.Y) - frame.y <= Ball.radius || frame.height - (ballCenter.Y) <= Ball.radius )
            {
                ball.speedY *= -1;
            }

            foreach (Player player in players)
            {
                PointF playerCenter= new PointF(player.x+Player.radius, player.y + Player.radius);
                double distance = Math.Sqrt(Math.Pow(ballCenter.X - playerCenter.X, 2) + Math.Pow(ballCenter.Y - playerCenter.Y, 2));
                double angle = angleOf(ballCenter, playerCenter);
//                Console.WriteLine(angle);
                if (distance <= Player.radius + Ball.radius )
                {
                    ball.speedX = Math.Cos(angle)* ball.maxSpeed;
                    ball.speedY = Math.Sin(angle)* ball.maxSpeed;
//                    Console.WriteLine("Math.Cos("+angle+"): "+ Math.Cos(angle));
//                    Console.WriteLine("speed: "+speedX+ ","+ speedY);
                    
                }
            }

            foreach (Goal goal in goals)
            {
                if (ballCenter.X  <= goal.x2 && ballCenter.X >= goal.x)
                {
                    if (goal.player.type == Player.Type.Top && ballCenter.Y - Ball.radius <= goal.y)
                    {
                        goal.player.opponent.score += 1;
                        initializeGameObjects(true);
                    }else if (goal.player.type == Player.Type.Bottom && ballCenter.Y + Ball.radius >= goal.y)
                    {
                        goal.player.opponent.score += 1;
                        initializeGameObjects(true);
                    }
                }
            }
            
            ball.x += (float)ball.speedX;
            ball.y += (float)ball.speedY;
            
        }
        
        public static double angleOf(PointF p1, PointF p2) {
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p1.X - p2.X);
            double result = Math.Atan2(deltaY, deltaX); 
            return result;
        }


        private void checkGameStatus()
        {
            if (player1.score >= 7)
            {
                //Player 1 is the winner stop the game
                initializeGameObjects();
            }
            if (player2.score >= 7)
            {
                //Player 1 is the winner stop the game
                initializeGameObjects();
            }
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
        }
    }
}
