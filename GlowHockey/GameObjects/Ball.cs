using System;
using System.Drawing;

namespace GlowHockey.GameObjects
{
    public class Ball : GameObject
    {
        private float radius;
        private int width;
        private int height;
        
        public Brush brush;
        public Pen pen;

        public double speedX= 20;
        public double speedY= 20;

//        public double maxSpeed= 10;
        
        public Ball(float defaultX, float defaultY, Color innerColor, Color outerColor, int radius) : base(defaultX, defaultY, innerColor, outerColor)
        {
            brush= new SolidBrush(innerColor);
            pen= new Pen(outerColor, 100);

            this.radius = radius;
            this.width = radius*2;
            this.height = radius*2;

            x = defaultX;
            y = defaultY;

        }

        public override void drawShape(Graphics g)
        {
            RectangleF reactangle= new RectangleF(x, y, width, height);
            g.FillEllipse(brush, reactangle);

        }

        public void move(Frame frame, Player[] players)
        {
            PointF ballCenter= new PointF(x+radius, y+radius);
            if ((ballCenter.X) - frame.x <= radius || frame.width - (ballCenter.X) <= radius )
            {
                speedX *= -1;
            }
            if ((ballCenter.Y) - frame.y <= radius || frame.height - (ballCenter.Y) <= radius )
            {
                speedY *= -1;
            }

            foreach (Player player in players)
            {
                PointF playerCenter= new PointF(player.x+player.radius, player.y + player.radius);
                double distance = Math.Sqrt(Math.Pow(ballCenter.X - playerCenter.X, 2) + Math.Pow(ballCenter.Y - playerCenter.Y, 2));
                if (distance <= player.radius + radius)
                {
                    if (ballCenter.X - playerCenter.X != 0)
                    {
                        double angle = angleOf(ballCenter, playerCenter);
                        
                        if (angle < 90)
                        {
                            speedX = -Math.Abs(speedX);
                            speedY = Math.Abs(speedY);
//                            speedX = -Math.Sin(angle) * maxSpeed;
//                            speedY = Math.Cos(angle) * maxSpeed;
                            Console.WriteLine(speedX + "," + speedY);
                        }else if (angle < 180)
                        {
                            speedX = Math.Abs(speedX);
                            speedY = Math.Abs(speedY);
//                            speedX = Math.Sin(angle) * maxSpeed;
//                            speedY = -Math.Cos(angle) * maxSpeed;
                        }else if (angle < 270)
                        {
                            speedX = Math.Abs(speedX);
                            speedY = -Math.Abs(speedY);
//                            speedX = -Math.Sin(angle) * maxSpeed;
//                            speedY = -Math.Cos(angle) * maxSpeed;
                        }
                        else
                        {
                            speedX = -Math.Abs(speedX);
                            speedY = -Math.Abs(speedY);
//                            speedX = Math.Sin(angle) * maxSpeed;
//                            speedY = -Math.Cos(angle) * maxSpeed;
                        }
//                        Console.WriteLine("SpeedX: "+speedX);
//                        Console.WriteLine("SpeedY: "+speedY);
                    }
                }
            }
            
            
            x += (float)speedX;
            y += (float)speedY;
        }
        public static double angleOf(PointF p1, PointF p2) {
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p2.X - p1.X);
            double result = Math.Atan2(deltaY, deltaX)* (180.0 / Math.PI);; 
            return (result < 0) ? (360d + result) : result;
        }
    }
}