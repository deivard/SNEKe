using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SNEKe
{
    public interface ICollideable : IDrawable
    {
        Position Position { get; }
    }
}
