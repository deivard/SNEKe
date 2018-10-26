using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using SNEKe;

namespace SNEKeGUI
{
    public partial class GameForm : Form
    {
        
        int playfieldWidth = 736;
        int playfieldHeight = 640;
        public int borderThickness { get; } = 10;
        
        private int MovesPerSecond = 10;
        private SnakeEngine game;

        private PlayField playField;
        private BottomPanel bottomPanel;
        private MainMenu mainMenu;
        private Button restartButton;
        private FlowLayoutPanel flow;
        private Timer timer;
        
        // For custom font
        public static PrivateFontCollection pfc { get; protected set; }

        #region Bitmaps
        // All bitmaps that the game use
        private Bitmap P1Head = new Bitmap((Image) Properties.Resources.ResourceManager.GetObject("JHeadGreen2"));
        private Bitmap P1Body = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JBodyGreen2"));

        private Bitmap P2Head = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JHeadBlue1"));
        private Bitmap P2Body = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JBodyBlue1"));

        private Bitmap P3Head = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JHeadYellow1"));
        private Bitmap P3Body = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JBodyYellow1"));

        private Bitmap P4Head = new Bitmap((Image) Properties.Resources.ResourceManager.GetObject("JHeadGray1"));
        private Bitmap P4Body = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("JBodyGray1"));


        // NOTE: Bitmap order MUST match the enum: FoodType { Normal, Golden, Speed, Slow, Mega }
        private List<Bitmap> FoodBitmaps = new List<Bitmap>()
        {
            new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("NormalApple1")),
            new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("GoldenApple1")),
            new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("SpeedUp3")),
            new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("SlowDown2")),
            new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("Megaspeed"))
        };

        #endregion

        public GameForm() : base()
        {
            Width = playfieldWidth + borderThickness * 2;
            Height = Width; // It's a square
            BackColor = Color.DarkOrange;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            InitCustomFont();

            game = new SnakeEngine(playfieldWidth, playfieldHeight, Settings.FPS, MovesPerSecond, FoodBitmaps);
            game.GameOverEventHandler += DisplayWinner;

            timer = new Timer();
            timer.Interval = 1000 / Settings.FPS;
            timer.Tick += Timer_Tick;

            flow = new FlowLayoutPanel();
            flow.FlowDirection = FlowDirection.TopDown;
            flow.Location = new Point(borderThickness-3, borderThickness-3);
            flow.AutoSize = true;
            flow.BackColor = Color.Gray;
            flow.BorderStyle = BorderStyle.None;

            restartButton = new Button();
            restartButton.AutoSize = true;
            restartButton.BackColor = Color.FromArgb(30,30,30);
            restartButton.ForeColor = Color.White;
            restartButton.Text = "RESTART";
            restartButton.UseCompatibleTextRendering = true;
            restartButton.Font = new Font(pfc.Families[0], 12);
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Location = new Point(borderThickness*2, Height - restartButton.Height*2 - borderThickness);
            
            restartButton.Click += RestartButton_Click;
            
            playField = new PlayField(game, playfieldWidth, playfieldHeight);
            flow.Controls.Add(playField);

            bottomPanel = new BottomPanel(game, new Font(GameForm.pfc.Families[0], 20));
            flow.Controls.Add(bottomPanel);
            
            mainMenu = new MainMenu();

            Controls.Add(flow);
            Controls.Add(mainMenu);

            mainMenu.Location = new Point((Width - mainMenu.Width) / 2, (Height - mainMenu.Height) / 2);
            mainMenu.BringToFront();

            // Assign handlers for the main menu buttons.
            mainMenu.OnePlayer.Click += OnePlayer_Click;
            mainMenu.TwoPlayers.Click += TwoPlayers_Click;
            mainMenu.ThreePlayers.Click += ThreePlayers_Click;
            mainMenu.FourPlayers.Click += FourPlayers_Click;
            mainMenu.ExitButton.Click += ExitButton_Click;
            
            // Gets fired before the KeyDown event
            PreviewKeyDown += GameForm_PreviewKeyDown;
            KeyPreview = true;

        }

        // Can specify which keys to accept as valid, for now all keys are accepted.
        private void GameForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void DisplayWinner(object sender, EventArgs e)
        {
            timer.Stop();
            playField.SwitchToPaintWinnerSnake();
            Controls.Add(restartButton);
            restartButton.BringToFront();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Application.Restart(); // Ghetto restart - small game, computers are fast...
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnePlayer_Click(object sender, EventArgs e)
        {
            game.AddPlayer(P1Head, P1Body, new Position(playfieldWidth / 2, playfieldHeight / 2), SnakeEngine.Direction.Left);

            bottomPanel.SwitchToPrintScore();
            foreach (var player in game.Players)
            {
                player.ScoreChangedEventHandler += bottomPanel.Refresh;
            }

            // Assign the correct KeyDown eventhandler for 1 player
            KeyDown += GameForm_KeyDown1Player;
            // Set the appropriate amount of max food on board. 2 pieces per snake
            Settings.MaxFoodOnBoard = SnakeEngine.Player * 2;

            //Controls.Remove(mainMenu); This apporach makes the arrowkeys not usable
            mainMenu.Visible = false;
            timer.Start();
        }

        private void TwoPlayers_Click(object sender, EventArgs e)
        {
            game.AddPlayer(P1Head, P1Body, new Position(playfieldWidth / 2 - 16, (playfieldHeight / 2) +16), SnakeEngine.Direction.Left);
            game.AddPlayer(P2Head, P2Body, new Position(playfieldWidth / 2 + 16, playfieldHeight / 2), SnakeEngine.Direction.Left);

            bottomPanel.SwitchToPrintScore();
            foreach (var player in game.Players)
            {
                player.ScoreChangedEventHandler += bottomPanel.Refresh;
            }

            // Assign the correct KeyDown eventhandler for 2 players
            KeyDown += GameForm_KeyDown2Player;
            // Set the appropriate amount of max food on board. 2 pieces per snake
            Settings.MaxFoodOnBoard = SnakeEngine.Player * 2;

            mainMenu.Visible = false;
            timer.Start();
        }

        private void ThreePlayers_Click(object sender, EventArgs e)
        {
            game.AddPlayer(P1Head, P1Body, new Position(playfieldWidth / 2 - 32, playfieldHeight/2), SnakeEngine.Direction.Left);
            game.AddPlayer(P2Head, P2Body, new Position(playfieldWidth / 2 , playfieldHeight/2 + 16), SnakeEngine.Direction.Left);
            game.AddPlayer(P3Head, P3Body, new Position(playfieldWidth / 2 + 32, playfieldHeight/2 + 32), SnakeEngine.Direction.Left);

            bottomPanel.SwitchToPrintScore();
            foreach (var player in game.Players)
            {
                player.ScoreChangedEventHandler += bottomPanel.Refresh;
            }

            // Assign the correct KeyDown eventhandler for 3 players
            KeyDown += GameForm_KeyDown3Player;
            // Set the appropriate amount of max food on board. 2 pieces per snake
            Settings.MaxFoodOnBoard = SnakeEngine.Player * 2;

            mainMenu.Visible = false;
            timer.Start();
        }

        private void FourPlayers_Click(object sender, EventArgs e)
        {
            game.AddPlayer(P1Head, P1Body, new Position(playfieldWidth / 2 - 32, playfieldHeight / 2), SnakeEngine.Direction.Left);
            game.AddPlayer(P2Head, P2Body, new Position(playfieldWidth / 2, playfieldHeight / 2 + 16), SnakeEngine.Direction.Left);
            game.AddPlayer(P3Head, P3Body, new Position(playfieldWidth / 2 + 32, playfieldHeight / 2 + 32), SnakeEngine.Direction.Left);
            game.AddPlayer(P4Head, P4Body, new Position(playfieldWidth / 2 + 64, playfieldHeight / 2 + 48), SnakeEngine.Direction.Left);

            bottomPanel.SwitchToPrintScore();
            foreach (var player in game.Players)
            {
                player.ScoreChangedEventHandler += bottomPanel.Refresh;
            }

            // Assign the correct KeyDown eventhandler for 4 players
            KeyDown += GameForm_KeyDown4Player;
            // Set the appropriate amount of max food on board. 2 pieces per snake
            Settings.MaxFoodOnBoard = SnakeEngine.Player * 2;

            mainMenu.Visible = false;
            timer.Start();
        }



        // MUCHO BOILERPLATE
        #region KeyDown Eventhandlers. P1, P2, P3, P4.

        // Eventhandler for singleplayer
        void GameForm_KeyDown1Player(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Settings.P1KeyUp:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P1KeyRight:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P1KeyDown:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P1KeyLeft:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                
                case Keys.Escape:
                    Application.Exit();
                    break;
            }

            e.Handled = true;
        }

        // Eventhandler for 2 players
        void GameForm_KeyDown2Player(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Settings.P1KeyUp:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P1KeyRight:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P1KeyDown:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P1KeyLeft:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P2KeyUp:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P2KeyRight:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P2KeyDown:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P2KeyLeft:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
            e.Handled = true;

        }

        // Eventhandler for 3 players
        void GameForm_KeyDown3Player(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Settings.P1KeyUp:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P1KeyRight:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P1KeyDown:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P1KeyLeft:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P2KeyUp:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P2KeyRight:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P2KeyDown:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P2KeyLeft:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P3KeyUp:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P3KeyRight:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P3KeyDown:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P3KeyLeft:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;

                #region Debug keys
                //case Keys.Z:
                //    game.Players[0].GrowSnake(1);
                //    game.Players[1].GrowSnake(1);
                //    game.Players[2].GrowSnake(1);

                //    break;
                //case Keys.X:
                //    game.Players[0].SpeedUp(1);
                //    game.Players[1].SpeedUp(1);
                //    game.Players[2].SpeedUp(1);
                //    break;
                //case Keys.C:
                //    game.Players[0].SlowDown(1);
                //    game.Players[1].SlowDown(1);
                //    game.Players[2].SlowDown(1);

                //    break;
                //case Keys.Q:
                //    game.Players[0].KillSelf();
                //    game.Players[1].KillSelf();
                //    game.Players[2].KillSelf();
                //    break;
                #endregion
            }
            e.Handled = true;
        }

        // KeyDown Eventhandler for 4 players
        private void GameForm_KeyDown4Player(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Settings.P1KeyUp:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P1KeyRight:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P1KeyDown:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P1KeyLeft:
                    game.Players[0].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P2KeyUp:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P2KeyRight:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P2KeyDown:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P2KeyLeft:
                    game.Players[1].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P3KeyUp:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P3KeyRight:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P3KeyDown:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P3KeyLeft:
                    game.Players[2].UpdateDirection(SnakeEngine.Direction.Left);
                    break;
                case Settings.P4KeyUp:
                    game.Players[3].UpdateDirection(SnakeEngine.Direction.Up);
                    break;
                case Settings.P4KeyRight:
                    game.Players[3].UpdateDirection(SnakeEngine.Direction.Right);
                    break;
                case Settings.P4KeyDown:
                    game.Players[3].UpdateDirection(SnakeEngine.Direction.Down);
                    break;
                case Settings.P4KeyLeft:
                    game.Players[3].UpdateDirection(SnakeEngine.Direction.Left);
                    break;

                case Keys.Escape:
                    Application.Exit();
                    break;
                
                
            }
            e.Handled = true;
        }
        #endregion


        private void Timer_Tick(object sender, EventArgs e)
        {
            game.Tick();
            playField.Refresh();
        }

        private void InitCustomFont()
        {
            //Create your private font collection object.
            pfc = new PrivateFontCollection();

            //Select your font from the resources.
            //My font here is "Digireu.ttf"
            int fontLength = Properties.Resources.BitNoire.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.BitNoire;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            pfc.AddMemoryFont(data, fontLength);
        }

    }

}
