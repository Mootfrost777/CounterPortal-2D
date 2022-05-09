using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CounterPortal_2D.Classes
{
    internal class Aim
    {
        private Texture2D texture;
        public Vector2 position;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Aim");
        }

        public void Update(GameTime gameTime)
        {
            position = new Vector2(Mouse.GetState().X - texture.Width / 2, Mouse.GetState().Y - texture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
