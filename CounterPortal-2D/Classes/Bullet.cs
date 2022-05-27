using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace CounterPortal_2D.Classes
{
    internal class Bullet
    {
        [JsonProperty("position")]
        private Vector2 position;

        [JsonProperty("isVisible")]
        public bool isVisible = true;

        [JsonIgnore]
        public static Texture2D texture;
        public Rectangle boundingBox;
        private Vector2 speed;


        public Bullet(Vector2 position, Vector2 speed)
        {
            this.position = position;
            this.speed = speed;
        }


        public void Update()
        {
            if (isVisible)
            {
                position += speed;
                boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                CheckCollision();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }

        public void CheckCollision()
        {
            foreach (var wall in Game1._walls)
            {
                if (boundingBox.Intersects(wall.wallRect))
                {
                    isVisible = false;
                }
            }
            foreach (var opponent in Game1._opponents)
            {
                if (boundingBox.Intersects(opponent.playerRect))
                {
                    opponent.isAlive = false;
                    isVisible = false;
                }
            }
        }
    }
}
