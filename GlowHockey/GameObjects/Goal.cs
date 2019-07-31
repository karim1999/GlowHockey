using System;
using System.Drawing;

namespace GlowHockey.GameObjects
{
    public class Goal : GameObject
    {
        public Player player;
        public static int width= 110;
        public static int height= 40;
        
        public Pen pen;

        public float x2;
        public float y2;
        
        public enum Type
        {
            Top,
            Bottom
        }

        
        public Goal(float defaultX, float defaultY, float x2, float y2, Player player) : base(defaultX, defaultY, player.innerColor, player.innerColor)
        {
            pen= new Pen(outerColor, 100);
            
            x = defaultX;
            y = defaultY;
            this.x2 = x2;
            this.y2 = y2;

            this.player = player;
        }
        public override void drawShape(Graphics g)
        {
            g.DrawLine(new Pen(outerColor), new PointF(x, y), new PointF(x, y2));
            g.DrawLine(new Pen(outerColor), new PointF(x, y2), new PointF(x2, y2));
            g.DrawLine(new Pen(outerColor), new PointF(x2 , y2), new PointF(x2  , y));
            g.DrawLine(new Pen(outerColor,10), new PointF(x, y), new PointF(x2, y));
            if(player.type == Player.Type.Top)
                g.DrawString(Convert.ToString(player.score), new Font("Arial",16,FontStyle.Bold), new SolidBrush(innerColor), x + Goal.width/2, y+5);
            else
                g.DrawString(Convert.ToString(player.score), new Font("Arial",16,FontStyle.Bold), new SolidBrush(innerColor), x + Goal.width/2, y - Goal.height + 5);
        }
    }
}