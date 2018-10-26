using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Drawing;
using System.Security.Cryptography.X509Certificates;


namespace SNEKe
{
    public class Snake
    {
        private int BoardWidth;
        private int BoardHeight;
        // Stores 4 different orientations of the head and body bitmap. 0: Up, 1: Right, 2: Down, 3: Left (Clockwise rotation)
        // The order is very important since it is synced with the Direction and the RotaeFlipType enums.
        public Bitmap[] HeadTextures { get; }= new Bitmap[4];
        public Bitmap[] BodyTextures { get; }= new Bitmap[4];

        public bool IsAlive { get; set; } = true;
        public bool IsDieing { get; set; }= false;

        private Position StartingPosition;
        private int StartingSpeed;
        private Trigger MoveTrigger;
        private Trigger MegaSpeedTrigger;
        private Trigger DeathAnimationTrigger;
        public int PlayerNumber { get; protected set; }
        public SnakeEngine.Direction Direction { get; protected set; }
        public int Score { get; set; }
        public LinkedList<ISnakePart> BodyParts { get; protected set; } = new LinkedList<ISnakePart>();
        // Used to print win screen.
        public ISnakePart[] DeathScreenshot { get; protected set; }

        public delegate void ScoreChangedEvent();
        public event ScoreChangedEvent ScoreChangedEventHandler; // BottomPanel will sub to this event
        
        public Snake(int playerNumber, int startingSpeed, Bitmap headTexture, Bitmap bodyTexture, Position startingPosition,
            SnakeEngine.Direction startingDir, int boardWidth, int boardHeight)
        {
            PlayerNumber = playerNumber;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Score = 0;
            StartingSpeed = startingSpeed;
            StartingPosition = startingPosition;
            Direction = startingDir;

            // Create the HeadTextures for each direction based on the original bitmap that is facing up. (FACING UP IS IMPORTANT)
            for (int i = 0; i < 4; i++)
            {
                HeadTextures[i] = headTexture.Clone(new Rectangle(0, 0, 16, 16), headTexture.PixelFormat);
                // RotateFlipType enum is using number 0,1,2,3 for RotateNoneFlip,Rotate90Flip,Rotate180Flip,Rotate270Flip
                HeadTextures[i].RotateFlip((RotateFlipType)i);
            }
            // Same procedure for the bodyparts.
            for (int i = 0; i < 4; i++)
            {
                BodyTextures[i] = bodyTexture.Clone(new Rectangle(0, 0, 16, 16), bodyTexture.PixelFormat);
                // RotateFlipType enum is using number 0,1,2,3 for RotateNoneFlip,Rotate90Flip,Rotate180Flip,Rotate270Flip
                BodyTextures[i].RotateFlip((RotateFlipType)i);
            }

            MoveTrigger = new Trigger(startingSpeed);
            MoveTrigger.Triggered += Move;
            MegaSpeedTrigger = new Trigger(Settings.MegaSpeedDuration); // Will never trigger unless threshold changes
            MegaSpeedTrigger.Active = false;
            MegaSpeedTrigger.Triggered += DeactivateHyperDrive;

            DeathAnimationTrigger = new Trigger(startingSpeed/3);
            DeathAnimationTrigger.Triggered += RemoveBodyPart;
            
            BodyParts.AddFirst(new SnakeHead(StartingPosition, Direction));
            BodyParts.AddFirst(new SnakeBody(new Position(StartingPosition.X + Settings.SquareSize, startingPosition.Y), Direction));
        }

        private void RemoveBodyPart()
        {
            // If all body parts are removed
            if (BodyParts.First == null)
            {
                DeathAnimationTrigger.Threshold = 0;
                IsAlive = false;
            }
            else
            {
                BodyParts.RemoveFirst();
            }
        }

        public void KillSelf()
        {
            DeathScreenshot = new ISnakePart[BodyParts.Count];
            // Copy the final state of the snake before the death animation starts. 
            BodyParts.CopyTo(DeathScreenshot, 0);
            IsDieing = true;
        }

        public void Tick()
        {
            if (!IsDieing)
            {
                MoveTrigger.Tick();
                //DeathAnimationTrigger.Tick();
                if (MegaSpeedTrigger.Active)
                    MegaSpeedTrigger.Tick();
            }
            else
            {
                DeathAnimationTrigger.Tick();
            }
        }
        
        public void UpdateScore(int add)
        {
            Score += add;
            ScoreChangedEventHandler?.Invoke();
        }

        public void GrowSnake(int growAmount)
        {
            for (int i = 0; i < growAmount; i++)
            {
                SnakeBody snake = new SnakeBody(
                    new Position(BodyParts.First.Value.Position.X, BodyParts.First.Value.Position.Y), BodyParts.First.Value.Orientation);
                BodyParts.AddFirst(snake);
            }
        }

        private void SpeedUp(int speedMod)
        {
            if (MoveTrigger.Threshold  > speedMod)
                MoveTrigger.Threshold -= speedMod;
        }

        // Hyperdrive activates when MegaSpeed is eaten
        private void ActivateHyperDrive()
        {
            UpdateScore(Settings.MegaSpeedInitialValue);
            GrowSnake(Settings.MegaSpeedInitialValue);
            MoveTrigger.Threshold = 1;
            MegaSpeedTrigger.Active = true; // Start the timer for the mega speed duration
        }

        // The fun is over
        private void DeactivateHyperDrive()
        {
            UpdateScore(Settings.MegaSpeedEndValue); // Gets bonus points if player survived the megaspeed
            GrowSnake(Settings.MegaSpeedEndValue);
            MoveTrigger.Threshold = StartingSpeed;
            MegaSpeedTrigger.Active = false;
        }

        private void SlowDown(int speedMod)
        {
            MoveTrigger.Threshold += speedMod;
        }

        public void Eat(IEatable food)
        {
            switch (food.Type)
            {
                case Food.FoodType.Normal:
                    UpdateScore(Settings.NormalFoodValue);
                    GrowSnake(Settings.NormalFoodValue);
                    break;
                case Food.FoodType.Golden:
                    UpdateScore(Settings.GoldenFoodValue);
                    GrowSnake(Settings.GoldenFoodValue);
                    break;
                case Food.FoodType.Speed:
                    UpdateScore(Settings.SpeedFoodValue);
                    GrowSnake(Settings.SpeedFoodValue);
                    SpeedUp(Settings.SpeedModifier);
                    break;
                case Food.FoodType.Slow:
                    UpdateScore(Settings.SlowFoodValue);
                    GrowSnake(Settings.SlowFoodValue);
                    SlowDown(Settings.SpeedModifier);
                    break;
                case Food.FoodType.Megaspeed:
                    ActivateHyperDrive();
                    break;
            }
        }

        public bool IsPosCollidingWith(Snake snake)
        {
            // NOTE: The input snake can be this snake. Since the snake can collide with it's own body.

            // First node in the bodyPart list, we will use this head's position to check if we are colliding
            var thisHead = BodyParts.Last.Value;
            // Node to itterade the bodyPart list of input snake.
            var node = snake.BodyParts.First;
            
            while (node.Next != null)
            {
                if (thisHead.Position == node.Value.Position)
                    return true;
                node = node.Next;
            }
            // Check if this snakes head is colliding with the input snakes head, 
            // and check if it is its own head that it is colliding with.
            if (snake != this && thisHead.Position == node.Value.Position)
                return true;
            return false;
        }

        public bool IsPosCollidingWith(Position position)
        {
            return BodyParts.Last.Value.Position == position;
        }

        // Updates the snakes current direction if it is a valid direction and triggers the MoveTrigger.ResetAndInvoke.
        public void UpdateDirection(SnakeEngine.Direction newDir)
        {
            switch (Direction)
            {
                case SnakeEngine.Direction.Down:
                    if (newDir == SnakeEngine.Direction.Left || newDir == SnakeEngine.Direction.Right)
                    {
                        Direction = newDir;
                        MoveTrigger.ResetAndInvoke();
                    }
                    break;
                case SnakeEngine.Direction.Up:
                    if (newDir == SnakeEngine.Direction.Left || newDir == SnakeEngine.Direction.Right)
                    {
                        Direction = newDir;
                        MoveTrigger.ResetAndInvoke();
                    }
                    break;
                case SnakeEngine.Direction.Left:
                    if (newDir == SnakeEngine.Direction.Up || newDir == SnakeEngine.Direction.Down)
                    {
                        Direction = newDir;
                        MoveTrigger.ResetAndInvoke();
                    }
                    break;
                case SnakeEngine.Direction.Right:
                    if (newDir == SnakeEngine.Direction.Up || newDir == SnakeEngine.Direction.Down)
                    {
                        Direction = newDir;
                        MoveTrigger.ResetAndInvoke();
                    }
                    break;
            }
        }

        // Subscriber to MoveTrigger.Triggered event. Moves the snake 1 square in the direction that the Direction field contains.
        public void Move()
        {
            // Create a node variable that will be used to itterate the list.
            var node = BodyParts.First;

            if (node != null)
            {
                // Itterate until we are at the last node in the list.
                while (node.Next != null)
                {
                    // The SnakePart stored at the current node will update its own position based on
                    // the position of the SnakePart next in the list.
                    node.Value.UpdatePos(new Position(node.Next.Value.Position.X, node.Next.Value.Position.Y), node.Next.Value.Orientation);
                    node = node.Next;
                }

                // Update the position of the head with help of the Direction field.
                node.Value.UpdatePos(Direction);
                // Check if the snakes head is outside the board and then place it on the other side if it is.
                if (node.Value.Position.X >= BoardWidth)
                    node.Value.Position.X = 0;
                else if (node.Value.Position.X < 0)
                    node.Value.Position.X = BoardWidth - Settings.SquareSize;
                else if (node.Value.Position.Y >= BoardHeight)
                    node.Value.Position.Y = 0;
                else if (node.Value.Position.Y < 0)
                    node.Value.Position.Y = BoardHeight - Settings.SquareSize;
            }
        }

        public void Draw(Graphics g)
        {
            // Create a node variable that will be used to itterate the list.
            var node = BodyParts.First;
            if (node != null)
            {
                // Itterate until we are at the last node in the list.
                while (node.Next != null)
                {
                    // The SnakePart stored at the current node will draw itself
                    node.Value.Draw(g, BodyTextures[(int)node.Value.Orientation]);
                    node = node.Next;
                }

                // Draw the head based on the Direction of the snake
                node.Value.Draw(g, HeadTextures[(int)Direction]);
            }
        }

        // Draw the snake the way it looked when it died.
        public void DrawDeathScreenshotSnake(Graphics g)
        {
            // Draw all bodyparts (not the head)
            for (int i = 0; i < DeathScreenshot.Length-1; i++)
            {
                DeathScreenshot[i].Draw(g, BodyTextures[(int)DeathScreenshot[i].Orientation]);
            }

            if (DeathScreenshot.Length > 0)
            {
                // Draw the head
                DeathScreenshot[DeathScreenshot.Length - 1].Draw(g, HeadTextures[(int) Direction]);
            }
        }
    }
}
