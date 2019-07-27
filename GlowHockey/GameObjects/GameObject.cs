using System.Drawing;

namespace GlowHockey.GameObjects
{
    public abstract class GameObject
    {
        public Color innerColor;
        public Color outerColor;
        
        public float x;
        public float y;

        public float defaultX;
        public float defaultY;

        public GameObject(float defaultX, float defaultY, Color innerColor, Color outerColor)
        {
            this.defaultX = defaultX;
            this.defaultY = defaultY;
            
            this.innerColor = innerColor;
            this.outerColor = outerColor;

        }
        public abstract void drawShape(Graphics g);
    }
}