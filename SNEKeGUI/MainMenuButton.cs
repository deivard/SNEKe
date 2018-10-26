using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNEKeGUI
{
    public class MainMenuButton : Button
    {
        private Color ForeGround = Color.White;
        private Color BackGround = Color.Black;

        public MainMenuButton() : base()
        {
        }

        public MainMenuButton(String text, int width, int height)
        {
            Width = width;
            Height = height;
            Text = text;
            ForeColor = ForeGround;
            BackColor = BackGround;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }
    }
}
