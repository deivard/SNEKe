using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SNEKe;

namespace SNEKeGUI
{
    public partial class PlayField : UserControl
    {
        private SnakeEngine Game;
        Brush brush = new SolidBrush(Color.White);

        public PlayField(SnakeEngine gameEngine, int width, int height)
        {
            Game = gameEngine;
           
            DoubleBuffered = true;

            BackColor = Color.FromArgb(30,30,30);
            BorderStyle = BorderStyle.None;
            Width = width;
            Height = height;
            InitializeComponent();

            Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            Game.Draw(paintEventArgs);
        }
        
        private void OnPaintWinner(object sender, PaintEventArgs paintEventArgs)
        {
            Game.DrawWinnerSnake(paintEventArgs);

            string WinnerText = "WINNER";
            string FinalScore = $"{Game.Winner.Score} PTS";

            paintEventArgs.Graphics.DrawString(WinnerText, new Font(GameForm.pfc.Families[0], 50), brush, 25, 25);
            paintEventArgs.Graphics.DrawString(FinalScore, new Font(GameForm.pfc.Families[0], 35), brush, 25 , 100);
        }

        public void SwitchToPaintWinnerSnake()
        {
            Paint -= OnPaint;
            Paint += OnPaintWinner;
        }
        public void SwitchToPaintAllSnakes()
        {
            Paint += OnPaint;
            Paint -= OnPaintWinner;
        }
    }
}
