using System;
using System.Drawing;

namespace GlowHockey.GameObjects
{
    public class Ball : GameObject
    {
        public static float radius= 15;

        private float width;
        private float height;
        
        public Brush brush;
        public Pen pen;

        public double speedX;
        public double speedY;

        public double maxSpeed= 10;
        
        public Ball(float defaultX, float defaultY, Color innerColor, Color outerColor) : base(defaultX, defaultY, innerColor, outerColor)
        {
            brush= new SolidBrush(innerColor);
            pen= new Pen(outerColor, 100);

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

        public void move(Frame frame, Player[] players, Goal[] goals)
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
                PointF playerCenter= new PointF(player.x+Player.radius, player.y + Player.radius);
                double distance = Math.Sqrt(Math.Pow(ballCenter.X - playerCenter.X, 2) + Math.Pow(ballCenter.Y - playerCenter.Y, 2));
                double angle = angleOf(ballCenter, playerCenter);
//                Console.WriteLine(angle);
                if (distance <= Player.radius + radius )
                {
                    speedX = Math.Cos(angle)* maxSpeed;
                    speedY = Math.Sin(angle)* maxSpeed;
//                    Console.WriteLine("Math.Cos("+angle+"): "+ Math.Cos(angle));
//                    Console.WriteLine("speed: "+speedX+ ","+ speedY);
                    
                }
            }

            foreach (Goal goal in goals)
            {
                if (ballCenter.X  <= goal.x2 && ballCenter.X >= goal.x)
                {
                    if (goal.player.type == Player.Type.Top && ballCenter.Y - radius <= goal.y)
                    {
                        goal.player.opponent.score += 1;

                    }else if (goal.player.type == Player.Type.Bottom && ballCenter.Y + radius >= goal.y)
                    {
                        goal.player.opponent.score += 1;
                    }
                }
            }
            
                x += (float)speedX;
                y += (float)speedY;
            
        }
        public static double angleOf(PointF p1, PointF p2) {
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p1.X - p2.X);
            double result = Math.Atan2(deltaY, deltaX); 
            return result;
        }
    }
}