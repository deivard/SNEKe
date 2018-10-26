using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SNEKe
{
    public class Food : IEatable
    {
        public enum FoodType { Normal, Golden, Speed, Slow, Megaspeed }

        public FoodType Type { get; protected set; } 
        public Position Position { get; protected set; }
        
        public Food(FoodType type, Position position)
        {
            Type = type;
            Position = position;
        }

        public Food(FoodType type, int posX, int posY) : this(type, new Position(posX, posY))
        {
        }

        public void Draw(Graphics g, Bitmap bitmap)
        {
            g.DrawImage(bitmap, Position.X, Position.Y);
        }
    }
}
