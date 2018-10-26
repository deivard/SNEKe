using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    class SnakeHead : ISnakePart
    {
        public Position Position { get; protected set; }
        public SnakeEngine.Direction Orientation { get; protected set; }

        public SnakeHead(Position position, SnakeEngine.Direction orientation)
        {
            Position = position;
            Orientation = orientation;
            //FoodType = Food.FoodType.SnakePart;
        }

        public void Draw(Graphics g, Bitmap bitmap)
        {           
            g.DrawImage(bitmap, Position.X, Position.Y);
        }

        public void UpdatePos(Position p, SnakeEngine.Direction newOrientation)
        {
            Position = p;
            Orientation = newOrientation;
        }

        public void UpdatePos(SnakeEngine.Direction newOrientation)
        {
            Orientation = newOrientation;
            switch (Orientation)
            {
                case SnakeEngine.Direction.Down:
                    Position.Y += Settings.SquareSize;
                    break;
                case SnakeEngine.Direction.Left:
                    Position.X -= Settings.SquareSize;
                    break;
                case SnakeEngine.Direction.Right:
                    Position.X += Settings.SquareSize;
                    break;
                case SnakeEngine.Direction.Up:
                    Position.Y -= Settings.SquareSize;
                    break;
            }
        }
    }
}
