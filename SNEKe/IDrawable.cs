using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    public interface IDrawable
    {
        void Draw(Graphics g, Bitmap bitmap);
    }
}
