using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNEKe
{
    public static class Settings
    {
        // Score increase and grow amount
        public static int NormalFoodValue { get; } = 1;
        public static int GoldenFoodValue { get; } = 3;
        public static int SpeedFoodValue { get; } = 1;
        public static int SlowFoodValue { get; } = 1;
        public static int MegaSpeedInitialValue { get; } = 7;
        public static int MegaSpeedEndValue { get; } = 3;

        public static int MaxFoodOnBoard { get; set; } = 10;
        public static int SpeedModifier { get; } = 1;
        public static int SquareSize { get; } = 16;
        public static int FPS { get; } = 60;
        public static int MegaSpeedDuration { get; } = FPS * 5; // In seconds

        #region Player Keybindings
        //Player 1
        public const Keys P1KeyUp = Keys.Up;
        public const Keys P1KeyRight = Keys.Right;
        public const Keys P1KeyDown = Keys.Down;
        public const Keys P1KeyLeft = Keys.Left;
        // Player 2
        public const Keys P2KeyUp = Keys.W;
        public const Keys P2KeyRight = Keys.D;
        public const Keys P2KeyDown = Keys.S;
        public const Keys P2KeyLeft = Keys.A;
        // Player 3
        public const Keys P3KeyUp = Keys.I;
        public const Keys P3KeyRight = Keys.L;
        public const Keys P3KeyDown = Keys.K;
        public const Keys P3KeyLeft = Keys.J;
        // Player 4
        public const Keys P4KeyUp = Keys.T;
        public const Keys P4KeyRight = Keys.H;
        public const Keys P4KeyDown = Keys.G;
        public const Keys P4KeyLeft = Keys.F;

        #endregion
    }
}
