using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNEKe
{
    public class FoodFactory
    {
        private SnakeEngine Game;
        private Random rand = new Random();
        private int[,] OccupiedSquaresMatrix; // Occupied slot on playfield = 1, Available = 0.
        private int NumOfSquaresWidth;
        private int NumOfSquaresHeight;

        public FoodFactory(SnakeEngine game)
        {
            Game = game;
            NumOfSquaresWidth = Game.Width / Settings.SquareSize;
            NumOfSquaresHeight = Game.Height / Settings.SquareSize;
            OccupiedSquaresMatrix = new int[NumOfSquaresWidth + 1, NumOfSquaresHeight + 1]; // +1 to count in the zero
        }

        // Set all squares to 0, aka available.
        public void ResetMatix()
        {
            for (int x = 0; x < NumOfSquaresWidth; x++)
            {
                for (int y = 0; y < NumOfSquaresHeight; y++)
                {
                    OccupiedSquaresMatrix[x, y] = 0;
                }
            }
        }

        // Updates the OccupiedSquaresMatrix with all the positions that are currently occupied by snakes and food.
        public void UpdateMatrix()
        {
            ResetMatix();

            // Update the positions that the players occupy
            foreach (var snake in Game.Players)
            {
                foreach (var bodyPart in snake.BodyParts)
                {
                    OccupiedSquaresMatrix[bodyPart.Position.X / Settings.SquareSize, bodyPart.Position.Y / Settings.SquareSize] = 1;
                }
            }

            // Update the positions that the food occupy
            foreach (var food in Game.FoodOnBoard)
            {
                OccupiedSquaresMatrix[food.Position.X / Settings.SquareSize, food.Position.Y / Settings.SquareSize] = 1;
            }
        }

        public Position GetAvailablePosition()
        {
            UpdateMatrix();

            // Get a random number from 0 and the number of "squares" in a row.
            int randomX = rand.Next(0, NumOfSquaresWidth);
            // Get a random number from 0 and the number of "squares" in a collumn.
            int randomY = rand.Next(0, NumOfSquaresHeight);
            if (OccupiedSquaresMatrix[randomX, randomY] == 1)
            {
                // Loop while the position will be on an occupied square
                while (OccupiedSquaresMatrix[randomX, randomY] == 1)
                {
                    // Loop row by row until we find next available square
                    for (int i = 0; i < NumOfSquaresWidth; ++i)
                    {
                        // If right edge of playfields is reached, move to the other side
                        if (randomX >= NumOfSquaresHeight)
                            randomX = 0;
                        // "Move" one step to the right
                        randomX += 1;
                        // If we found an empty square, return the actual position by scaling it up
                        if (OccupiedSquaresMatrix[randomX, randomY] == 0)
                            return new Position(randomX * Settings.SquareSize, randomY * Settings.SquareSize);
                    }

                    // If bottom edge of playfields is reached, move to the top
                    if (randomY >= Game.Height)
                        randomY = 0;
                    // Move one step down
                    randomY += 1;
                }
                // If no avaiable slow was found, it means the gameboard is full, good job player.
                // Spawn the food top left just to make the SnakeEngine happy
                return new Position(0, 0);
            }
            // The random location is available, return a Position with the random X & Y coordinates scaled up.
            return new Position(randomX * Settings.SquareSize, randomY * Settings.SquareSize);
        }

        // Creates and returns a random food at an empty location.
        public Food GenerateRandomFood()
        {
            int randomInt = rand.Next(0, 100);
            Position pos = GetAvailablePosition();

            // 40% chance to spawn normal
            if (randomInt > 60)
                return new Food(Food.FoodType.Normal, pos);
            // 10% chance to spawn slow
            else if (randomInt > 50)
                return new Food(Food.FoodType.Slow, pos);
            // 15% chance to spawn golden
            else if (randomInt > 35)
                return new Food(Food.FoodType.Golden, pos);
            // 5% chance to spawn mega speed
            else if (randomInt > 30)
                return new Food(Food.FoodType.Megaspeed, pos);
           // 30% chance to spawn speed
            else
                return new Food(Food.FoodType.Speed, pos);

        }
    }
}