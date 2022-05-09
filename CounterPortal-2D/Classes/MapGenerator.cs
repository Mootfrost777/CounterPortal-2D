using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace CounterPortal_2D.Classes
{
    internal class MapGenerator
    {
        private Random random;
        private int borderWidth;

        public MapGenerator(int seed, int borderWidth)
        {
            random = new Random(seed);
            this.borderWidth = borderWidth;
        }

        public List<Wall> GenerateMap()
        {
            List<Wall> walls = new List<Wall>();
            for (int i = 0; i < 7; i++)
            {
                int width = random.Next(10, 100);
                int height = random.Next(10, 100);
                walls.Add(new Wall(new Rectangle(random.Next(borderWidth, Game1.screenWidth - width), random.Next(borderWidth, Game1.screenHeight), width, height)));
            }
            walls.Add(new Wall(new Rectangle(0, 0, Game1.screenWidth, borderWidth)));
            walls.Add(new Wall(new Rectangle(0, 0, borderWidth, Game1.screenWidth)));
            walls.Add(new Wall(new Rectangle(0, Game1.screenHeight - borderWidth, Game1.screenWidth, borderWidth)));
            walls.Add(new Wall(new Rectangle(Game1.screenWidth - borderWidth, 0, Game1.screenWidth - borderWidth, Game1.screenHeight)));
            return walls;
        }
    }
}
