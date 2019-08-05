using System.Drawing;

namespace GlowHockey.GameObjects
{
    public abstract class GameObject
    {
        private readonly object xLock = new object();
        private readonly object yLock = new object();

        public Color innerColor;
        public Color outerColor;
        
        public float x;
        public float y;

        public float X
        {
            set
            {
                lock (xLock)
                {
                    this.x = value;
                }
            }
            get
            {
                lock (xLock)
                {
                    return this.x;
                }
            }
        }
        public float Y
        {
            set
            {
                lock (yLock)
                {
                    this.y = value;
                }
            }
            get
            {
                lock (yLock)
                {
                    return this.y;
                }
            }
        }

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