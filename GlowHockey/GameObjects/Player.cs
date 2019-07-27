using System.Drawing;

namespace GlowHockey.GameObjects
{
    public class Player : GameObject

    {
        public float radius;
        public int width;
        public int height;
        
        public Brush brush;
        public Pen pen;

        public enum Type
        {
            Top,
            Bottom
        }

        
        public Player(float defaultX, float defaultY, Color innerColor, Color outerColor, int radius, Type type) : base(defaultX, defaultY, innerColor, outerColor)
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
    }
}