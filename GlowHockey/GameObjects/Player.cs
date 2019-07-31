using System.Drawing;

namespace GlowHockey.GameObjects
{
    public class Player : GameObject

    {

        public static float radius= 30;
        
        public float width;
        public float height;
        
        public Brush brush;
        public Pen pen;

        public Goal goal;

        public int score;
        public enum Type
        {
            Top,
            Bottom
        }

        public Type type;
        
        public Player(float defaultX, float defaultY, Color innerColor, Color outerColor, Type type) : base(defaultX, defaultY, innerColor, outerColor)
        {
            brush= new SolidBrush(innerColor);
            pen= new Pen(outerColor, 100);

            this.type = type;
            this.width = radius*2;
            this.height = radius*2;

            x = defaultX;
            y = defaultY;
            
        }
        public override void drawShape(Graphics g)
        {
            g.FillEllipse(brush, x, y, width, height);
        }
    }
}