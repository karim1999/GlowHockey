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

            X = defaultX;
            Y = defaultY;

        }

        public override void drawShape(Graphics g)
        {
            RectangleF reactangle= new RectangleF(X, Y, width, height);
            g.FillEllipse(brush, reactangle);

        }

    }
}