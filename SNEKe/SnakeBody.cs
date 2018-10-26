using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    class SnakeBody : ISnakePart
    {
        public SnakeEngine.Direction Orientation { get; protected set; }
        public Position Position { get; protected set; }

        public SnakeBody(Position pos, SnakeEngine.Direction orientation)
        {
            Position = pos;
            Orientation = orientation;
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
            // Not used
        }
    }
}
