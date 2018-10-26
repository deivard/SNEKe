using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNEKe
{

    public class SnakeEngine
    {
        public enum Direction { Up, Right, Down, Left }; // Clockwise order

        int FPS;
        private int MovePerSecond;
        public static int Player { get; private set; } = 0;

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public List<Food> FoodOnBoard { get; protected set; } = new List<Food>();
        public List<Snake> Players = new List<Snake>();
        public Snake Winner { get; protected set; }
        private FoodFactory foodFactory;
        private List<Bitmap> FoodBitmaps;
        private Random rand = new Random();


        public EventHandler GameOverEventHandler;
        public EventHandler ScoreChangedEventHandler;


        public SnakeEngine(int width ,int height, int fps, int movePerSecond, List<Bitmap> foodBitmaps)
        {
            MovePerSecond = movePerSecond;
            Width = width;
            Height = height;
            FPS = fps;
            foodFactory = new FoodFactory(this);
            FoodBitmaps = foodBitmaps;


            GameOverEventHandler += FindWinner;

        }

        // Creates a new player and increments the Player integer that keeps track on how many players there is and identifies each player.
        public void AddPlayer(Bitmap head, Bitmap body, Position startingPosition, Direction startirection)
        {
            Player++;
            Players.Add(new Snake(Player, FPS/MovePerSecond, head, body, startingPosition, startirection, Width, Height));
        }

        // Add the food to the game
        protected void AddFood(Food food)
        {
            FoodOnBoard.Add(food);
        }

        protected void RemoveFood(Food food)
        {
            FoodOnBoard.Remove(food);
        }


        public void Tick()
        {
            if (CheckGameOver())
                GameOverEventHandler?.Invoke(null, EventArgs.Empty);
            else
            {
                TickPlayers();
                Collide();

                // Make sure there is 5 food on board at all times.
                while (FoodOnBoard.Count < Settings.MaxFoodOnBoard)
                {
                    AddFood(foodFactory.GenerateRandomFood());
                }

            }
        }

        protected void TickPlayers()
        {
            foreach (var snake in Players)
            {
                if (snake.IsAlive)
                    snake.Tick();
            }
        }


        protected bool CheckGameOver()
        {
            // Loop each snake and check if any of them is alive, then return false.
            foreach (var player in Players)
            {
                if (player.IsAlive)
                    return false;
            }
            // If no snake is alive it is game over.
            return true;
        }


        protected void Collide()
        {
            // Make a copy so we can remove food from the original FoodOnBoard while itterating
            Food[] copyOfFoodOnBoard = new Food[FoodOnBoard.Count];
            FoodOnBoard.CopyTo(copyOfFoodOnBoard);

            foreach (var snake in Players)
            {
                if (!snake.IsDieing)
                {
                    // Collision with snakes
                    foreach (var collidingSnake in Players)
                    {
                        if (!collidingSnake.IsDieing)
                        {
                            if (snake.IsPosCollidingWith(collidingSnake))
                            {
                                snake.KillSelf();
                                collidingSnake.UpdateScore(snake.Score/2);
                                collidingSnake.GrowSnake(snake.Score/2);
                            }
                        }
                    }
                    // Collision with food
                    foreach (var food in copyOfFoodOnBoard)
                    {
                        if (snake.IsPosCollidingWith(food.Position))
                        {
                            // If it is a megaspeed food, make a random snake eat it.
                            if (food.Type == Food.FoodType.Megaspeed)
                            {
                                int randomSnake = rand.Next(0, Players.Count);
                                while (!Players[randomSnake].IsAlive)
                                    randomSnake = rand.Next(0, Players.Count);
                                Players[randomSnake].Eat(food);
                            }
                            // Else make the colliding snake eat it.
                            else
                            {
                                snake.Eat(food);
                            }
                            RemoveFood(food);
                        }
                    }
                }
            }
        }

        public void Draw(PaintEventArgs e)
        {
            foreach (var snake in Players)
            {
                snake.Draw(e.Graphics);
            }
            foreach (var food in FoodOnBoard)
            {
                food.Draw(e.Graphics, FoodBitmaps[(int)food.Type]);
            }
        }

        public void DrawWinnerSnake(PaintEventArgs e)
        {
            Winner.DrawDeathScreenshotSnake(e.Graphics);
        }

        protected void FindWinner(Object sender, EventArgs e)
        {
            Winner = Players[0];
            foreach (var snake in Players)
            {
                // CBA BUG: If its a tie...
                if (snake.Score > Winner.Score)
                    Winner = snake;
            }
        }
    }

}
