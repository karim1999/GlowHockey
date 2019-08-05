using System;
using System.Drawing;
using System.Windows.Forms;
using GlowHockey.GameObjects;
using OpponentLibrary;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace GlowHockey
{

    public partial class GameForm : Form
    {
        private UdpClient soc= new UdpClient();
        private System.Windows.Forms.Timer timer;
        private Frame frame;
        private Player player1;
        private Player player2;
        private Goal goal1;
        private Goal goal2;
        private Ball ball;

        private int screenWidth= 600;
        private int screenHeight= 800;

        private bool isRunning;
        private Opponent opponent;

        public GameForm(Opponent opponent)
        {
            this.DoubleBuffered = true;
            this.opponent = opponent;
            InitializeComponent();
            
            this.Size= new Size(600+10, 800+40);

            if (opponent.Type == Opponent.PlayerType.Top)
            {
                player1 = new Player(screenWidth / 2 - Player.radius, screenHeight / 4 * 3 - Player.radius, Color.Red, Color.Gray, Opponent.PlayerType.Bottom);
                goal1 = new Goal(screenWidth / 2 - Goal.width / 2, screenHeight, screenWidth / 2 + Goal.width / 2, screenHeight - Goal.height, player1);

                player2 = new Player(screenWidth / 2 - Player.radius, screenHeight / 4 - Player.radius, Color.Green, Color.Gray, Opponent.PlayerType.Top);
                goal2 = new Goal(screenWidth / 2 - Goal.width / 2, 0, screenWidth / 2 + Goal.width / 2, Goal.height, player2);
            }
            else
            {
                player1 = new Player(screenWidth / 2 - Player.radius, screenHeight / 4 - Player.radius, Color.Green, Color.Gray, Opponent.PlayerType.Top);
                goal1 = new Goal(screenWidth / 2 - Goal.width / 2, 0, screenWidth / 2 + Goal.width / 2, Goal.height, player1);

                player2 = new Player(screenWidth / 2 - Player.radius, screenHeight / 4 * 3 - Player.radius, Color.Red, Color.Gray, Opponent.PlayerType.Bottom);
                goal2 = new Goal(screenWidth / 2 - Goal.width / 2, screenHeight, screenWidth / 2 + Goal.width / 2, screenHeight - Goal.height, player2);
            }


            initializeGameObjects();

            UdpClient serv_soc = new UdpClient(opponent.CurrentIp.Port);
            ThreadHandler th = new ThreadHandler(player2, ball, serv_soc);
            Thread t = new Thread(th.handle);
            t.Start();


            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000 / 120;
            timer.Tick += TimerOnTick;
            timer.Start();

            this.MouseMove += OnMouseMove;
            this.Paint += GameForm_Paint;
            this.FormClosing += GameForm_FormClosing;
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void TimerOnTick(object sender, EventArgs e)
        {

            checkGameStatus();
            byte[] data;

            if (opponent.Type == Opponent.PlayerType.Top)
            {
                data = Encoding.Unicode.GetBytes(player1.X + "," + player1.Y);
            }
            else
            {
                move(frame, new Player[] { player1, player2 }, new Goal[] { goal1, goal2 });
                data = Encoding.Unicode.GetBytes(player1.X + "," + player1.Y + "," + ball.X + "," + ball.Y);
            }

            try
            {
                soc.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(opponent.Ip.Address.MapToIPv4().ToString()), opponent.Ip.Port));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.Invalidate();
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            frame.drawShape(e.Graphics);
            e.Graphics.DrawLine(new Pen(Color.Yellow, 5), new PointF(0, screenHeight / 2), new PointF(screenWidth, screenHeight / 2));

            player2.drawShape(e.Graphics);
            player1.drawShape(e.Graphics);
            goal1.drawShape(e.Graphics);
            goal2.drawShape(e.Graphics);
            ball.drawShape(e.Graphics);
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
            frame = new Frame( 0, 0, Color.Black,  Color.Yellow, this.screenWidth, this.screenHeight);

            player1.X = player1.defaultX;
            player1.Y = player1.defaultY;
            player2.X = player2.defaultX;
            player2.Y = player2.defaultY;

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
            if (e.X < screenWidth - player1.width)
                player1.X = e.X;
            if (opponent.Type == Opponent.PlayerType.Top)
            {
                if (e.Y > screenHeight / 2 && e.Y < screenHeight - player1.height)
                    player1.Y = e.Y;
                else if(e.Y < screenHeight / 2)
                    player1.Y = screenHeight/2;
            }
            else
            {
                if (e.Y < screenHeight / 2 - player1.height)
                    player1.Y = e.Y;
                else
                    player1.Y = screenHeight / 2 - player1.height;
            }

        }


        public void move(Frame frame, Player[] players, Goal[] goals)
        {
            PointF ballCenter= new PointF(ball.X+Ball.radius, ball.Y+Ball.radius);
            if ((ballCenter.X) - frame.X <= Ball.radius || frame.width - (ballCenter.X) <= Ball.radius )
            {
                ball.speedX *= -1;
            }
            if ((ballCenter.Y) - frame.Y <= Ball.radius || frame.height - (ballCenter.Y) <= Ball.radius )
            {
                ball.speedY *= -1;
            }

            foreach (Player player in players)
            {
                PointF playerCenter= new PointF(player.X+Player.radius, player.Y + Player.radius);
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
                if (ballCenter.X  <= goal.x2 && ballCenter.X >= goal.X)
                {
                    if (goal.player.type == Opponent.PlayerType.Top && ballCenter.Y - Ball.radius <= goal.Y)
                    {
                        goal.player.opponent.score += 1;
                        initializeGameObjects(true);
                    }else if (goal.player.type == Opponent.PlayerType.Bottom && ballCenter.Y + Ball.radius >= goal.Y)
                    {
                        goal.player.opponent.score += 1;
                        initializeGameObjects(true);
                    }
                }
            }
            
            ball.X += (float)ball.speedX;
            ball.Y += (float)ball.speedY;
            
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
         
    }
    public class ThreadHandler
    {
        Player player;
        Ball ball;
        UdpClient serv_soc;

        public ThreadHandler(Player player, Ball ball, UdpClient serv_soc)
        {
            this.player = player;
            this.ball = ball;
            this.serv_soc = serv_soc;
        }

        public void handle()
        {
            while (true)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                Byte[] data = serv_soc.Receive(ref sender);

                String clientmsg = System.Text.Encoding.Unicode.GetString(data);
                string[] coordinates = clientmsg.Split(',');
                Console.WriteLine(clientmsg);
                player.X = float.Parse(coordinates[0]);
                player.Y = float.Parse(coordinates[1]);
                if(coordinates.Length > 2)
                {
                    ball.X = float.Parse(coordinates[2]);
                    ball.Y = float.Parse(coordinates[3]);
                }

            }
        }

    }

}
