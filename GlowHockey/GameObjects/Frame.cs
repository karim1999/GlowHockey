using System.Drawing;
using System.Drawing.Drawing2D;

namespace GlowHockey.GameObjects
{
    public class Frame : GameObject
    {
        public int width;
        public int height;
        
        public Brush brush;
        public Pen pen;


        public Frame(float defaultX, float defaultY, Color innerColor, Color outerColor, int width, int height) : base(defaultX, defaultY, innerColor, outerColor)
        {
            brush= new SolidBrush(innerColor);
            pen= new Pen(outerColor, 100);

            this.width = width;
            this.height = height;
            x = defaultX;
            y = defaultY;
        }
        public override void drawShape(Graphics g)
        {
            RectangleF reactangle= new RectangleF(x, y, width, height);
            g.FillRectangle(brush, reactangle);
//            Rectangle reactangle2= new Rectangle(0, 0, width, height);
//            g.DrawRectangle(pen, reactangle2);
        }
    }
}