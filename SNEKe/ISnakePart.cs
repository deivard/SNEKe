using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    public interface ISnakePart : IDrawable, ICollideable //, IEatable
    {
        SnakeEngine.Direction Orientation { get; }
        void UpdatePos(Position p, SnakeEngine.Direction newOrientation);
        void UpdatePos(SnakeEngine.Direction newOrientation);
    }
}
