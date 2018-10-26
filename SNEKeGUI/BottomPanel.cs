using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using SNEKe;

namespace SNEKeGUI
{
	public partial class BottomPanel : UserControl
	{
		private SnakeEngine game;
	    private Font BitNoireFont;
		private Brush brush = new SolidBrush(Color.White);
        
		public BottomPanel(SnakeEngine game, Font font)
		{
		    BitNoireFont = font;   
			var flow = new FlowLayoutPanel();
			Controls.Add(flow);
			this.Size = new Size(736, 90);
			this.BackColor = Color.FromArgb(30,30,30);
			this.Anchor = AnchorStyles.Bottom;
			this.game = game;
			Paint += TitleScreen;
		}

		public void SwitchToPrintScore()
		{
			Paint -= TitleScreen;

			switch (game.Players.Count)
			{
				case 0:
					break;
				case 1:					
					Paint += OnePLayer_paint;
					break;
				case 2:					
					Paint += TwoPlayers_paint;
					break;
				case 3:
					Paint += ThreePlayers_paint;
					break;
                case 4:
                    Paint += FourPlayers_paint;
                    break;
			}

		}

		private void TitleScreen(object sender, PaintEventArgs e)
		{
			var s = $"SNEKE";
			e.Graphics.DrawString(s, BitNoireFont, brush, new Point(300, 20));
		}

		private void OnePLayer_paint(object sender, PaintEventArgs e)
		{
			var s = $": {game.Players[0].Score}";
			e.Graphics.DrawString(s, BitNoireFont, brush, new Point(350, 25));
			e.Graphics.DrawImage(game.Players[0].HeadTextures[0], new Point(300, 25));
			e.Graphics.DrawImage(game.Players[0].BodyTextures[0], new Point(300, 31));
		}

		private void TwoPlayers_paint(object sender, PaintEventArgs e)
		{
			var s = $": {game.Players[0].Score}";
			var s2 = $": {game.Players[1].Score}";
			e.Graphics.DrawString(s, BitNoireFont, brush, new Point(220, 25));
			e.Graphics.DrawString(s2, BitNoireFont, brush, new Point(420, 25));

			e.Graphics.DrawImage(game.Players[0].HeadTextures[0], new Point(200, 25));
			e.Graphics.DrawImage(game.Players[0].BodyTextures[0], new Point(200, 41));

			e.Graphics.DrawImage(game.Players[1].HeadTextures[0], new Point(400, 25));
			e.Graphics.DrawImage(game.Players[1].BodyTextures[0], new Point(400, 41));
		}

		private void ThreePlayers_paint(object sender, PaintEventArgs e)
		{
			var s = $": {game.Players[0].Score}";
			var s2 = $": {game.Players[1].Score}";
			var s3 = $": {game.Players[2].Score}";
			e.Graphics.DrawString(s, BitNoireFont, brush, new Point(220, 25));
			e.Graphics.DrawString(s2, BitNoireFont, brush, new Point(370, 25));
			e.Graphics.DrawString(s3, BitNoireFont, brush, new Point(520, 25));

			e.Graphics.DrawImage(game.Players[0].HeadTextures[0], new Point(200, 25));
			e.Graphics.DrawImage(game.Players[0].BodyTextures[0], new Point(200, 41));

			e.Graphics.DrawImage(game.Players[1].HeadTextures[0], new Point(350, 25));
			e.Graphics.DrawImage(game.Players[1].BodyTextures[0], new Point(350, 41));
		
			e.Graphics.DrawImage(game.Players[2].HeadTextures[0], new Point(500, 25));
			e.Graphics.DrawImage(game.Players[2].BodyTextures[0], new Point(500, 41));
		}

	    private void FourPlayers_paint(object sender, PaintEventArgs e)
	    {
	        var s = $": {game.Players[0].Score}";
	        var s2 = $": {game.Players[1].Score}";
	        var s3 = $": {game.Players[2].Score}";
	        var s4 = $": {game.Players[3].Score}";
	        e.Graphics.DrawString(s, BitNoireFont, brush, new Point(220, 25));
	        e.Graphics.DrawString(s2, BitNoireFont, brush, new Point(320, 25));
	        e.Graphics.DrawString(s3, BitNoireFont, brush, new Point(420, 25));
	        e.Graphics.DrawString(s4, BitNoireFont, brush, new Point(520, 25));

	        e.Graphics.DrawImage(game.Players[0].HeadTextures[0], new Point(200, 25));
	        e.Graphics.DrawImage(game.Players[0].BodyTextures[0], new Point(200, 41));

	        e.Graphics.DrawImage(game.Players[1].HeadTextures[0], new Point(300, 25));
	        e.Graphics.DrawImage(game.Players[1].BodyTextures[0], new Point(300, 41));

	        e.Graphics.DrawImage(game.Players[2].HeadTextures[0], new Point(400, 25));
	        e.Graphics.DrawImage(game.Players[2].BodyTextures[0], new Point(400, 41));

	        e.Graphics.DrawImage(game.Players[3].HeadTextures[0], new Point(500, 25));
	        e.Graphics.DrawImage(game.Players[3].BodyTextures[0], new Point(500, 41));
	    }
    }
}
