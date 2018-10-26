using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNEKeGUI
{
    public partial class MainMenu : FlowLayoutPanel
    {
        public MainMenuButton OnePlayer { get; }
        public MainMenuButton TwoPlayers { get; }
        public MainMenuButton ThreePlayers { get; }
        public MainMenuButton FourPlayers { get; }
        public MainMenuButton ExitButton { get; }

        private Font font;

        public MainMenu() : base()
        {
            font = new Font(GameForm.pfc.Families[0], 10);
            #region This Flow's settings

            InitializeComponent();
            FlowDirection = FlowDirection.TopDown;
            BorderStyle = BorderStyle.None;
            BackColor = Color.DarkOrange;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            #endregion

            #region Buttons
            OnePlayer = new MainMenuButton("ONE  PLAYER",180, 40);
            OnePlayer.Font = font;
            OnePlayer.UseCompatibleTextRendering = true;

            TwoPlayers = new MainMenuButton("TWO  PLAYERS", 180, 40);
            TwoPlayers.Font = font;
            TwoPlayers.UseCompatibleTextRendering = true;

            ThreePlayers = new MainMenuButton("THREE  PLAYERS", 180, 40);
            ThreePlayers.Font = font;
            ThreePlayers.UseCompatibleTextRendering = true;

            FourPlayers = new MainMenuButton("FOUR  PLAYERS", 180, 40);
            FourPlayers.Font = font;
            FourPlayers.UseCompatibleTextRendering = true;

            ExitButton = new MainMenuButton("EXIT", 180, 40);
            ExitButton.Font = font;
            ExitButton.UseCompatibleTextRendering = true;


            #endregion

            Controls.Add(OnePlayer);
            Controls.Add(TwoPlayers);
            Controls.Add(ThreePlayers);
            Controls.Add(FourPlayers);
            Controls.Add(ExitButton);

            KeyDown += KeyPressedEventHandler;
            
        }

        private void KeyPressedEventHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
        }
    }
}
